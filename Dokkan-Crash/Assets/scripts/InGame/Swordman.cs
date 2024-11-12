using UnityEngine;

public class Swordman : PlayerController
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

    private void Start()
    {
        m_CapsulleCollider = GetComponent<CapsuleCollider2D>();
        m_Anim = transform.Find("model").GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (isTurnActive && IsAlive)
        {
            CheckInput();
        }
    }

    public void StartTurn()
    {
        isTurnActive = true;
    }

    public void EndTurn()
    {
        isTurnActive = false;
        ResetCharge(); // ターン終了時にチャージをリセット
    }

    private void CheckInput()
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
                gameManager.EndTurn();
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

        // if (Input.GetKeyDown(KeyCode.Mouse0))
        // {
        //     StartCharging(); // チャージを開始
        // }

        // if (Input.GetKey(KeyCode.Mouse0))
        // {
        //     ChargeProjectile(); // チャージを継続
        // }

        // if (Input.GetKeyUp(KeyCode.Mouse0))
        // {
        //     ShootChargedProjectile(); // チャージが完了したら発射
        //     gameManager.EndTurn();
        // }
    }

    // チャージを開始する
    private void StartCharging()
    {
        isCharging = true;
        chargeTime = 0f; // チャージ時間をリセット
    }

    // チャージを継続する
    private void ChargeProjectile()
    {
        if (isCharging)
        {
            // チャージ時間を増加（ただし、最大値まで）
            chargeTime += Time.deltaTime;
            chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime);
        }
    }

    // チャージ時間に基づいて弾を発射
    private void ShootChargedProjectile()
    {
        if (!isCharging) return;

        // 最小速度と最大速度の間でスピードを計算
        float speed = Mathf.Lerp(minSpeed, maxSpeed, chargeTime / maxChargeTime);

        // 弾を生成し、発射方向と速度を設定
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile proj = projectile.GetComponent<Projectile>();
        Vector2 shootDirection = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        proj.SetDirection(shootDirection, speed);

        ResetCharge();
    }

    // チャージ状態をリセット
    private void ResetCharge()
    {
        isCharging = false;
        chargeTime = 0f;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            m_Anim.Play("Die");
            gameManager.CheckGameOver();
        }
    }

    protected override void LandingEvent()
    {
        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Run") && !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            m_Anim.Play("Idle");
    }
}
