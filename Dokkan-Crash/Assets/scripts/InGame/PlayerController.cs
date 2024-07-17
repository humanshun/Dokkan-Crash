using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerController : MonoBehaviour
{
    public bool IsSit = false; // プレイヤーが座っているかどうか
    public int currentJumpCount = 0; // 現在のジャンプ回数
    public bool isGrounded = false; // プレイヤーが地面に接触しているかどうか
    public bool OnceJumpRayCheck = false; // 一度ジャンプしたかどうかをチェック

    public bool Is_DownJump_GroundCheck = false; // ダウンジャンプ時に地面かどうかをチェックするフラグ
    protected float m_MoveX; // プレイヤーのX軸の移動量
    public Rigidbody2D m_rigidbody; // プレイヤーのRigidbody2Dコンポーネント
    protected CapsuleCollider2D m_CapsulleCollider; // プレイヤーのカプセルコライダー
    protected Animator m_Anim; // プレイヤーのアニメーター

    [Header("[Setting]")]
    public float MoveSpeed = 6; // 移動速度
    public int JumpCount = 2; // ジャンプの回数
    public float jumpForce = 15f; // ジャンプの力



/// <summary>
/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////変更点
/// </summary>

    public GameObject Player1;
    public GameObject Player2;

    public Transform firePoint; // 射撃位置
    public float minPower = 10f; // 最小の力
    public float maxPower = 100f; // 最大の力
    public float powerIncreaseRate = 10f; // 1秒間に溜まる力
    public Slider powerSlider; // スライダーの参照を追加

    private bool isPlayer1Turn = true;
    private float fireAngle = 0f;
    public float currentPower = 0f;
    private bool isCharging = false;
    private PlayerManager playerManager;

    public void SetPlayer(bool isTurn)
    {
        isPlayer1Turn = isTurn;
        powerSlider.gameObject.SetActive(isTurn); // ターンが開始/終了時にスライダーを表示/非表示
        if (!isTurn)
        {
            powerSlider.value = 0; // ターンが終わったらスライダーをリセット
        }
    }

    public void HandlePlayerInput()
    {
        // if (!isPlayer1Turn) return;

        
        //プレイヤーマネージャーからcurrentStateの値を取ってきて
        //ターンPlayer１とPlayer２を振り分け
        // if (playerManager.instance.currentState != "PLAYER1TURN")
        // {
        //     Player1.return;
        // }


        // 矢印キーで方角を調整
        if (Input.GetKey(KeyCode.W))
        {
            fireAngle += 0.1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            fireAngle -= 0.1f;
        }

        // 左クリックで発射威力を調整
        if (Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            currentPower = minPower;  // 初期威力を設定
            powerSlider.minValue = minPower;
            powerSlider.maxValue = maxPower;
        }

        if (Input.GetMouseButton(0) && isCharging)
        {
            currentPower += powerIncreaseRate * Time.deltaTime;
            currentPower = Mathf.Clamp(currentPower, minPower, maxPower);
            powerSlider.value = currentPower; // スライダーの値を更新
        }

        // 左クリックを離すと弾を発射
        if (Input.GetMouseButtonUp(0))
        {
            isCharging = false;
            FindObjectOfType<InGame_GM>().SwitchTurn();
            FindObjectOfType<InGame_GM>().FireBullet();
        }

        // 発射の向きを更新
        firePoint.rotation = Quaternion.Euler(0, 0, fireAngle);
    }


/// <summary>
/// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// </summary>







    // アニメーションの更新メソッド
    protected void AnimUpdate()
    {
        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                m_Anim.Play("Attack"); // 攻撃アニメーション
            }
            else
            {
                if (m_MoveX == 0)
                {
                    if (!OnceJumpRayCheck)
                        m_Anim.Play("Idle"); // 待機アニメーション
                }
                else
                {
                    m_Anim.Play("Run"); // 走りアニメーション
                }
            }
        }
    }

    // プレイヤーの向きを反転するメソッド
    protected void Filp(bool bLeft)
    {
        transform.localScale = new Vector3(bLeft ? 1 : -1, 1, 1);
    }

    // ジャンプを実行するメソッド
    protected void prefromJump()
    {
        m_Anim.Play("Jump"); // ジャンプアニメーション
        m_rigidbody.velocity = new Vector2(0, 0);
        m_rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // 上向きに力を加える
        OnceJumpRayCheck = true;
        isGrounded = false;
        currentJumpCount++;
    }

    // ダウンジャンプを実行するメソッド
    protected void DownJump()
    {
        if (!isGrounded)
            return;

        if (!Is_DownJump_GroundCheck)
        {
            m_Anim.Play("Jump"); // ジャンプアニメーション
            m_rigidbody.AddForce(-Vector2.up * 10); // 下向きに力を加える
            isGrounded = false;
            m_CapsulleCollider.enabled = false; // カプセルコライダーを無効にする
            StartCoroutine(GroundCapsulleColliderTimmerFuc()); // コルーチンを開始
        }
    }

    // カプセルコライダーを再度有効にするコルーチン
    IEnumerator GroundCapsulleColliderTimmerFuc()
    {
        yield return new WaitForSeconds(0.3f); // 0.3秒待機
        m_CapsulleCollider.enabled = true; // カプセルコライダーを有効にする
    }

    // バーチャルチェックレイキャスト
    Vector2 RayDir = Vector2.down;

    float PretmpY;
    float GroundCheckUpdateTic = 0;
    float GroundCheckUpdateTime = 0.01f;

    // 地面チェックの更新メソッド
    protected void GroundCheckUpdate()
    {
        if (!OnceJumpRayCheck)
            return;

        GroundCheckUpdateTic += Time.deltaTime;

        if (GroundCheckUpdateTic > GroundCheckUpdateTime)
        {
            GroundCheckUpdateTic = 0;

            if (PretmpY == 0)
            {
                PretmpY = transform.position.y;
                return;
            }

            float reY = transform.position.y - PretmpY; // 高さの変化を計算

            if (reY <= 0)
            {
                if (isGrounded)
                {
                    LandingEvent(); // 着地イベントを実行
                    OnceJumpRayCheck = false;
                }
            }

            PretmpY = transform.position.y;
        }
    }

    // 抽象メソッド：着地イベント
    protected abstract void LandingEvent();
}