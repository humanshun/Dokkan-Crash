using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : PlayerController
{
    public string playerName { get; set; } // プレイヤー名を保持するフィールド
    public TextMeshProUGUI playerNameText; // プレイヤー名前表示テキスト
    private bool isTurnActive = false;     // 今自分のターンかどうか
    private GameManager gameManager;

    //-----------------------------------

    //移動制限
    private float maxMoveTime = 3f;          // 横移動できる最大の合計時間（秒）
    private float totalMoveTime = 0f;        // 横移動の累積時間
    private bool canMoveHorizontally = true; // 横移動可能フラグ
    private float sliderMaxMoveTime = 0f;    // スライダーの合計時間（秒）
    private float sliderTotalMoveTime = 3f;  // スライダーの累積時間
    public Slider moveSlider;

    //-----------------------------------

    //爆弾
    [SerializeField] private SpriteRenderer spriteRenderer; //爆弾のスプライトコンポーネント
    [SerializeField] private Sprite bombSprite1; //通常の爆弾
    [SerializeField] private Sprite bombSprite2; //ワープ弾

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();

        if (m_rigidbody == null)
        {
            Debug.LogError("Rigidbody2D がアタッチされていません。");
        }

        // 最初全員のチャージスライドを非表示にしておく
        chargeSlider.gameObject.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        m_CapsulleCollider = GetComponent<CapsuleCollider2D>();
        m_Anim = transform.Find("model").GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();

        // チャージスライダー初期化
        if (chargeSlider != null)
        {
            chargeSlider.maxValue = maxChargeTime;
            chargeSlider.value = 0f;
        }

        // ムーブスライダー初期化
        if (moveSlider != null)
        {
            moveSlider.maxValue = maxMoveTime;
            moveSlider.value = 0;
        }

        // プレイヤー名を表示する
        if (playerNameText != null)
        {
            playerNameText.text = playerName; // 名前を設定
        }

        currentBombPrefab = bombPrefab1; // 通常の爆弾を初期設定にする
    }


    private void Update()
    {
        moveSlider.value = sliderTotalMoveTime;

        // 自分のターンで、自分が生きてたら
        if (isTurnActive && IsAlive)
        {
            CheckInput();             // 入力メソッド
            RotateFirePointToMouse(); // マウスの方向にFirePointを向けるメソッド
        }
    }

    public void StartTurn()
    {
        isTurnActive = true;
        canMoveHorizontally = true;

        //スライダーリセット
        moveSlider.value = sliderMaxMoveTime;

        //スライダー値リセット
        sliderTotalMoveTime = 3f;
        totalMoveTime = 0f;

        // ターン開始時に動けるようにする
        canMove = true; 

        if (chargeSlider != null)
        {
            chargeSlider.gameObject.SetActive(true); // 自分のターン開始時にスライダーを表示
        }

        // プレイヤー名を表示
        Debug.Log($"It's {playerName}'s turn!");

        // プレイヤーを押して落としてしまうので自分のターン以外はX軸だけ固定
        m_rigidbody.constraints = RigidbodyConstraints2D.None;
        m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void EndTurn()
    {
        isTurnActive = false;
        ResetCharge(); // ターン終了時にチャージをリセット
        if (chargeSlider != null)
        {
            chargeSlider.gameObject.SetActive(false); // ターン終了時にスライダーを非表示
        }

        // プレイヤーを押して落としてしまうので自分のターン以外はX軸だけ固定
        m_rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }
    protected override void OnDeath()
    {
        if (!IsAlive)
            return;
        // キャラクターが死亡した場合、ゲームオーバーを確認
        Debug.Log($"{playerName} has been eliminated!");
        gameManager.EndTurn();
        // 自身をGameManagerに通知
        if (gameManager != null)
        {
            gameManager.CheckGameOver();
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
        // チャージ処理
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCharging(); // チャージを開始
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            m_Anim.Play("Charging");
            ChargeProjectile(); // チャージを継続
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            m_Anim.Play("Attack");
            ShootChargedProjectile(); // チャージが完了したら発射
        }

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
                    // プレイヤーが地面にいる場合のみRunアニメーションを再生
                    if (isGrounded)
                    {
                        m_Anim.Play("Run"); // 走りアニメーション再生
                    }
                }
            }
        }

        // その他の移動入力処理
        if (Input.GetKey(KeyCode.D))
        {
            if (!isCharging)
            {
                totalMoveTime += Time.deltaTime; // 移動時間を加算
                sliderTotalMoveTime -= Time.deltaTime;
                if (totalMoveTime <= maxMoveTime && canMoveHorizontally)
                {
                    transform.transform.Translate(Vector2.right * m_MoveX * MoveSpeed * Time.deltaTime);
                }

                if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    return;

                if (!Input.GetKey(KeyCode.A))
                    Filp(false); // キャラクターを右向きに反転
            }
            
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (!isCharging)
            {
                totalMoveTime += Time.deltaTime; // 移動時間を加算
                sliderTotalMoveTime -= Time.deltaTime;
                if (totalMoveTime <= maxMoveTime && canMoveHorizontally)
                {
                    transform.transform.Translate(Vector2.right * m_MoveX * MoveSpeed * Time.deltaTime);
                }

                if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    return;

                if (!Input.GetKey(KeyCode.D))
                    Filp(true); // キャラクターを左向きに反転
            }
            
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
                    // プレイヤーが地面にいる場合のみRunアニメーションを再生
                    if (isGrounded)
                    {
                        m_Anim.Play("Run"); // 走りアニメーション再生
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetHasBomb(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetHasBomb(2);
        }
    }

    public void SetHasBomb(int bomb)
    {
        if (bomb == 1)
        {
            spriteRenderer.sprite = bombSprite1;
            currentBombPrefab = bombPrefab1; // 通常の爆弾に切り替え
        }
        if (bomb == 2)
        {
            spriteRenderer.sprite = bombSprite2;
            currentBombPrefab = bombPrefab2; // ワープ弾に切り替え
        }
    }

    protected override void LandingEvent()
    {
        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Run") && !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            m_Anim.Play("Idle");
    }
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Die"))
        {
            chargeSlider.gameObject.SetActive(false);
            TakeDamage(1f);
            
            // 自身をGameManagerに通知
            if (gameManager != null)
            {
                gameManager.CheckGameOver();
            }
        }
    }
}
