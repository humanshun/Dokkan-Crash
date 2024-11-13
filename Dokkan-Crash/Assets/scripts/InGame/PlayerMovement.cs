using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : PlayerController
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    
    private bool isTurnActive = false;
    private GameManager gameManager;

    private float chargeTime = 0f; // チャージ時間
    private float maxChargeTime = 3f; // 最大チャージ時間
    private float minSpeed = 5f; // 最小発射速度
    private float maxSpeed = 20f; // 最大発射速度
    private bool isCharging = false; // チャージ中かどうか
    private bool canMove = true; // プレイヤーが動けるかどうかを制御するフラグ

    public Slider chargeSlider; // チャージ進行を表示するスライダー

    protected override void Start()
    {
        base.Start();
        m_CapsulleCollider = GetComponent<CapsuleCollider2D>();
        m_Anim = transform.Find("model").GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();

        // GameManagerのインスタンスを取得
        gameManager = FindObjectOfType<GameManager>();

        if (chargeSlider != null)
        {
            chargeSlider.maxValue = maxChargeTime; // スライダーの最大値を設定
            chargeSlider.value = 0f; // スライダーの初期値を設定
        }

        chargeSlider.gameObject.SetActive(false);
    }


    private void Update()
    {
        if (isTurnActive && IsAlive)
        {
            CheckInput();
            RotateFirePointToMouse(); // マウスの方向にFirePointを向ける
        }
    }

    public void StartTurn()
    {
        isTurnActive = true;
        canMove = true; // ターン開始時に動けるようにする
        if (chargeSlider != null)
        {
            chargeSlider.gameObject.SetActive(true); // ターン開始時にスライダーを表示
        }
    }

    public void EndTurn()
    {
        isTurnActive = false;
        ResetCharge(); // ターン終了時にチャージをリセット
        if (chargeSlider != null)
        {
            chargeSlider.gameObject.SetActive(false); // ターン終了時にスライダーを非表示
        }
    }
    private void RotateFirePointToMouse()
    {
        // マウス位置を取得してワールド座標に変換
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Z軸を0に固定

        // FirePointからマウス位置への方向ベクトルを計算
        Vector2 direction = (mousePosition - firePoint.position).normalized;

        // プレイヤーの向きを確認
        bool isFacingLeft = transform.localScale.x < 0;

        // 向きに基づいて角度を計算
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // プレイヤーが左を向いている場合、角度を反転
        if (isFacingLeft)
        {
            angle += 180f; // 左向きの場合、矢印を反転
        }

        // FirePointの回転を設定
        firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void CheckInput()
    {
        if (!canMove) return; // プレイヤーが動けないときは入力を受け付けない

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
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCharging(); // チャージを開始
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                m_Anim.Play("Charging"); // 攻撃アニメーション再生
                ChargeProjectile(); // チャージを継続
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                m_Anim.Play("Attack");
                ShootChargedProjectile(); // チャージが完了したら発射
            }
            if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Charging"))
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

    // チャージを開始する
    private void StartCharging()
    {
        isCharging = true;
        chargeTime = 0f; // チャージ時間をリセット
        UpdateChargeSlider(); // スライダーの初期値を設定
    }

    // チャージを継続する
    private void ChargeProjectile()
    {
        if (isCharging)
        {
            // チャージ時間を増加（ただし、最大値まで）
            chargeTime += Time.deltaTime;
            chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime);
            UpdateChargeSlider(); // スライダーの値を更新
        }
    }
    // スライダーの値を更新する
    private void UpdateChargeSlider()
    {
        if (chargeSlider != null)
        {
            chargeSlider.value = chargeTime;
        }
    }

    // チャージ時間に基づいて弾を発射
    private void ShootChargedProjectile()
    {
        if (!isCharging) return;

        float speed = Mathf.Lerp(minSpeed, maxSpeed, chargeTime / maxChargeTime);
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Projectileコンポーネントを取得して、発射方向と速度を設定
        Projectile proj = projectile.GetComponent<Projectile>();

        // firePointの向いている方向を基準に発射方向を決定
        Vector2 shootDirection = firePoint.right; // firePointの正面方向を取得

        // プレイヤーが左を向いている場合、方向を逆にする
        if (transform.localScale.x < 0)
        {
            shootDirection = -firePoint.right; // 左向きなら反転
        }

        proj.SetDirection(shootDirection, speed);
        ResetCharge();

        canMove = false; // 発射後に動けなくする
    }

    // チャージ状態をリセット
    private void ResetCharge()
    {
        isCharging = false;
        chargeTime = 0f;
        UpdateChargeSlider(); // スライダーをリセット
    }

    protected override void LandingEvent()
    {
        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Run") && !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            m_Anim.Play("Idle");
    }
    // キャラクター死亡時の処理
    protected override void OnDeath()
    {
        gameManager.CheckGameOver();
    }
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Die"))
        {
            chargeSlider.gameObject.SetActive(false);
            TakeDamage(1000f);
            OnDeath();
        }
    }
}
