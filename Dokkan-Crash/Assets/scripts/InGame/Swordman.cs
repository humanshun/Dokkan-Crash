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
            HandleMovement();
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCharging(); // チャージを開始
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            ChargeProjectile(); // チャージを継続
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            ShootChargedProjectile(); // チャージが完了したら発射
            gameManager.EndTurn();
        }
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

    private void HandleMovement()
    {
        m_MoveX = Input.GetAxis("Horizontal");
        transform.Translate(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0);

        if (m_MoveX != 0)
        {
            m_Anim.Play("Run");
            Filp(m_MoveX < 0);
        }
        else
        {
            m_Anim.Play("Idle");
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
