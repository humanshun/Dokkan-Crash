using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// PlayerControllerはキャラクターの動作を管理する抽象クラスです。
// 他のクラスでこのクラスを継承することで、共通の移動やジャンプの機能を実装できます。
public abstract class PlayerController : MonoBehaviour
{
    // === パブリック変数 ===
    public bool IsSit = false; // キャラクターが座っている状態かどうか
    public int currentJumpCount = 0; // 現在のジャンプ回数
    public bool isGrounded = false; // キャラクターが地面についているかどうか
    public bool OnceJumpRayCheck = false; // 一度ジャンプをした後に地面チェックを行うかどうか
    protected float m_MoveX; // 横方向の移動入力
    public Rigidbody2D m_rigidbody; // キャラクターのRigidbody2Dコンポーネント
    protected CapsuleCollider2D m_CapsulleCollider; // キャラクターのコライダー
    protected Animator m_Anim; // キャラクターのアニメーション制御
    protected bool canMove = true; // キャラクターが移動可能かどうか

    // Projectile関連の変数
    public GameObject projectilePrefab; // 弾のプレハブ
    public Transform firePoint;         // 発射位置

    // チャージ関連変数
    protected float chargeTime = 0f;      // チャージ時間
    protected float maxChargeTime = 5f;   // 最大チャージ時間
    protected float minSpeed = 5f;        // 最小発射速度
    protected float maxSpeed = 20f;       // 最大発射速度
    protected bool isCharging = false;    // チャージ中かどうか
    public Slider chargeSlider;           // チャージ進行を表示するスライダー

    // === 設定用の変数 ===
    [Header("[Setting]")]
    public float MoveSpeed = 6; // キャラクターの移動速度
    public int JumpCount = 2; // ジャンプ可能回数
    public float jumpForce = 15f; // ジャンプ時の力
    public float maxHealth = 1;  // 最大HP
    public float health;         // 現在のHP
    public Slider healthSlider; // HPを表示するスライダー
    public bool IsAlive => health > 0;

    // === メソッド ===
    protected virtual void Start()
    {
        health = maxHealth;
        
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }
    }

    // TakeDamageメソッド：ダメージを受けてHPを減少させる
    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth); // HPが0未満にならないように

        // 小さな誤差を0として扱う
        if (health < 0.01f) 
        {
            health = 0;
        }

        UpdateHealthSlider(); // HPスライダーを更新

        if (health <= 0.01f)
        {
            m_Anim.Play("Die");
            OnDeath();
        }
    }


    // UpdateHealthSliderメソッド：HPスライダーを更新する
    private void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            // healthをスライダーに設定
            healthSlider.value = health;
        }
    }

    // 抽象メソッド：キャラクター死亡時の処理
    protected abstract void OnDeath();

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
                    // プレイヤーが地面にいる場合のみRunアニメーションを再生
                    if (isGrounded)
                    {
                        m_Anim.Play("Run"); // 走りアニメーション再生
                    }
                }
            }
        }
    }

    // Filpはキャラクターの向きを左右に反転させます
    protected void Filp(bool bLeft)
    {
        // ローカルスケールのX軸を反転して左右を変更
        transform.localScale = new Vector3(bLeft ? 1 : -1, 1, 1);

        // HPバーの反転処理
        if (healthSlider != null)
        {
            // HPバーの親オブジェクトのローカルスケールを取得
            Transform sliderTransform = healthSlider.transform.parent;

            if (sliderTransform != null)
            {
                // HPバーのX軸スケールをプレイヤーのローカルスケールに合わせる
                Vector3 sliderScale = sliderTransform.localScale;
                sliderScale.x = Mathf.Abs(sliderScale.x) * (bLeft ? 1 : -1); // プレイヤーの向きに合わせて反転
                sliderTransform.localScale = sliderScale;
            }
        }
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
    // チャージを開始する
    protected void StartCharging()
    {
        isCharging = true;
        chargeTime = 0f;
        UpdateChargeSlider();
    }

    // チャージを継続する
    protected void ChargeProjectile()
    {
        if (isCharging)
        {
            chargeTime += Time.deltaTime;
            chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime);
            UpdateChargeSlider();
        }
    }

    // スライダーの値を更新する
    protected void UpdateChargeSlider()
    {
        if (chargeSlider != null)
        {
            chargeSlider.value = chargeTime;
        }
    }

    // チャージ状態をリセット
    protected void ResetCharge()
    {
        isCharging = false;
        chargeTime = 0f;
        UpdateChargeSlider();
    }
    protected void ShootChargedProjectile()
    {
        if (!isCharging) return;

        float speed = Mathf.Lerp(minSpeed, maxSpeed, chargeTime / maxChargeTime);
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Projectileコンポーネントを取得して、発射方向と速度を設定
        Projectile proj = projectile.GetComponent<Projectile>();

        // firePointの向いている方向を基準に発射方向を決定
        Vector2 shootDirection = firePoint.right;

        // プレイヤーが左を向いている場合、方向を逆にする
        if (transform.localScale.x < 0)
        {
            shootDirection = -firePoint.right;
        }

        proj.SetDirection(shootDirection, speed);
        ResetCharge();

        canMove = false; // 発射後に動けなくする
    }

    // 抽象メソッド: LandingEventは継承先で定義し、着地時の処理を行う
    protected abstract void LandingEvent();
}
