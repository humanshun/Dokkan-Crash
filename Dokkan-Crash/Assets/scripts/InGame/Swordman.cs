using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordman : PlayerController
{
    // 初期化処理
    private void Start()
    {
        m_CapsulleCollider = this.transform.GetComponent<CapsuleCollider2D>(); // カプセルコライダーを取得
        m_Anim = this.transform.Find("model").GetComponent<Animator>(); // アニメーターを取得
        m_rigidbody = this.transform.GetComponent<Rigidbody2D>(); // リジッドボディを取得
    }

    // 毎フレーム更新処理
    private void Update()
    {
        checkInput(); // 入力をチェック

        // 速度制限
        if (m_rigidbody.velocity.magnitude > 30)
        {
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x - 0.1f, m_rigidbody.velocity.y - 0.1f);
        }
    }

    // 入力をチェックするメソッド
    public void checkInput()
    {
        // Sキーが押された時の処理（座る動作）
        if (Input.GetKeyDown(KeyCode.S))
        {
            IsSit = true;
            m_Anim.Play("Sit");
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            m_Anim.Play("Idle");
            IsSit = false;
        }

        // 座りや死亡アニメーション中は他のアニメーションを再生しない
        if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Sit") || m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (currentJumpCount < JumpCount)  // ジャンプ可能回数チェック
                {
                    DownJump(); // 下方向へのジャンプ
                }
            }
            return;
        }

        m_MoveX = Input.GetAxis("Horizontal"); // 横方向の入力を取得

        GroundCheckUpdate(); // 地面チェック

        // 攻撃アニメーション中でない場合の処理
        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                m_Anim.Play("Attack"); // 攻撃アニメーション再生
            }
            else
            {
                if (m_MoveX == 0)
                {
                    if (!OnceJumpRayCheck)
                        m_Anim.Play("Idle"); // 立ちアニメーション再生
                }
                else
                {
                    m_Anim.Play("Run"); // 走りアニメーション再生
                }
            }
        }

        // 1キーが押された時の処理（死亡動作）
        if (Input.GetKey(KeyCode.Alpha1))
        {
            m_Anim.Play("Die");
        }

        // その他の移動入力処理
        if (Input.GetKey(KeyCode.D))
        {
            if (isGrounded) // 地面にいる場合
            {
                if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    return;

                transform.transform.Translate(Vector2.right * m_MoveX * MoveSpeed * Time.deltaTime); // 右方向への移動
            }
            else
            {
                transform.transform.Translate(new Vector3(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0)); // 空中での移動
            }

            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;

            if (!Input.GetKey(KeyCode.A))
                Filp(false); // キャラクターを右向きに反転
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (isGrounded) // 地面にいる場合
            {
                if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    return;

                transform.transform.Translate(Vector2.right * m_MoveX * MoveSpeed * Time.deltaTime); // 左方向への移動
            }
            else
            {
                transform.transform.Translate(new Vector3(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0)); // 空中での移動
            }

            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;

            if (!Input.GetKey(KeyCode.D))
                Filp(true); // キャラクターを左向きに反転
        }

        // スペースキーが押された時の処理（ジャンプ）
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;

            if (currentJumpCount < JumpCount) // ジャンプ可能回数チェック
            {
                if (!IsSit)
                {
                    prefromJump(); // ジャンプ動作
                }
                else
                {
                    DownJump(); // 下方向へのジャンプ
                }
            }
        }
    }

    // 着地イベント処理
    protected override void LandingEvent()
    {
        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Run") && !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            m_Anim.Play("Idle"); // 立ちアニメーション再生
    }
}
