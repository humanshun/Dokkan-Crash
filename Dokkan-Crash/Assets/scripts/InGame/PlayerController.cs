using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerControllerはキャラクターの動作を管理する抽象クラスです。
// 他のクラスでこのクラスを継承することで、共通の移動やジャンプの機能を実装できます。
public abstract class PlayerController : MonoBehaviour
{
    // === パブリック変数 ===
    public bool IsSit = false; // キャラクターが座っている状態かどうか
    public int currentJumpCount = 0; // 現在のジャンプ回数
    public bool isGrounded = false; // キャラクターが地面についているかどうか
    public bool OnceJumpRayCheck = false; // 一度ジャンプをした後に地面チェックを行うかどうか

    public bool Is_DownJump_GroundCheck = false; // ダウンジャンプが可能かの判定フラグ
    protected float m_MoveX; // 横方向の移動入力
    public Rigidbody2D m_rigidbody; // キャラクターのRigidbody2Dコンポーネント
    protected CapsuleCollider2D m_CapsulleCollider; // キャラクターのコライダー
    protected Animator m_Anim; // キャラクターのアニメーション制御

    // === 設定用の変数 ===
    [Header("[Setting]")]
    public float MoveSpeed = 6; // キャラクターの移動速度
    public int JumpCount = 2; // ジャンプ可能回数
    public float jumpForce = 15f; // ジャンプ時の力
    public int health = 3;  // キャラクターの体力
    public bool IsAlive => health > 0;  // 体力が0より大きいかの確認

    // === メソッド ===

    // AnimUpdateは、キャラクターのアニメーションを更新します。
    protected void AnimUpdate()
    {
        // 現在のアニメーションが「Attack」でない場合
        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            // 左クリックで攻撃アニメーションを再生
            if (Input.GetKey(KeyCode.Mouse0))
            {
                m_Anim.Play("Attack");
            }
            else
            {
                // 水平方向の入力がない場合は「Idle」、ある場合は「Run」を再生
                if (m_MoveX == 0)
                {
                    if (!OnceJumpRayCheck)
                        m_Anim.Play("Idle");
                }
                else
                {
                    m_Anim.Play("Run");
                }
            }
        }
    }

    // Filpはキャラクターの向きを左右に反転させます
    protected void Filp(bool bLeft)
    {
        // ローカルスケールのX軸を反転して左右を変更
        transform.localScale = new Vector3(bLeft ? 1 : -1, 1, 1);
    }

    // prefromJumpは、ジャンプアクションを実行します
    protected void prefromJump()
    {
        m_Anim.Play("Jump"); // ジャンプアニメーションを再生
        m_rigidbody.velocity = new Vector2(0, 0); // 現在の速度をリセット
        m_rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // 上向きにジャンプ力を加える

        OnceJumpRayCheck = true; // ジャンプ後の地面チェックを有効にする
        isGrounded = false; // キャラクターが地面から離れた状態にする
        currentJumpCount++; // ジャンプ回数を1回増加
    }

    // DownJumpは、下方向へのジャンプアクション（降下）を実行します
    protected void DownJump()
    {
        if (!isGrounded) return; // 地面にいない場合は終了

        if (!Is_DownJump_GroundCheck) // ダウンジャンプ可能な場合
        {
            m_Anim.Play("Jump"); // ジャンプアニメーションを再生
            m_rigidbody.AddForce(-Vector2.up * 10); // 下方向に力を加える
            isGrounded = false; // 地面から離れた状態にする

            m_CapsulleCollider.enabled = false; // 一時的にコライダーを無効化
            StartCoroutine(GroundCapsulleColliderTimmerFuc()); // コライダーの有効化を遅らせるコルーチンを開始
        }
    }

    // GroundCapsulleColliderTimmerFucはコライダーを再度有効にするまでの待機時間を設定します
    IEnumerator GroundCapsulleColliderTimmerFuc()
    {
        yield return new WaitForSeconds(0.3f); // 0.3秒待機
        m_CapsulleCollider.enabled = true; // コライダーを再度有効にする
    }

    // === 地面チェック関連 ===
    Vector2 RayDir = Vector2.down; // 地面チェック用のレイキャストの方向（下向き）

    float PretmpY; // 前のフレームでのY座標
    float GroundCheckUpdateTic = 0; // 地面チェックのタイマー
    float GroundCheckUpdateTime = 0.01f; // 地面チェックの間隔

    // GroundCheckUpdateはキャラクターが地面にいるかどうかを確認します
    protected void GroundCheckUpdate()
    {
        if (!OnceJumpRayCheck) return; // ジャンプ後でない場合はチェックを行わない

        GroundCheckUpdateTic += Time.deltaTime; // タイマーを更新

        if (GroundCheckUpdateTic > GroundCheckUpdateTime)
        {
            GroundCheckUpdateTic = 0;

            if (PretmpY == 0) // 最初のフレームではY位置を記録
            {
                PretmpY = transform.position.y;
                return;
            }

            float reY = transform.position.y - PretmpY; // 前回のY座標との差を計算

            if (reY <= 0) // 落下している場合
            {
                if (isGrounded) // 地面に着地している場合
                {
                    LandingEvent(); // 着地イベントを発生させる
                    OnceJumpRayCheck = false;
                }
            }
            PretmpY = transform.position.y; // 現在のY位置を記録
        }
    }

    // 抽象メソッド: LandingEventは継承先で定義し、着地時の処理を行う
    protected abstract void LandingEvent();
}
