using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // プレイヤーの移動速度
    public float jumpForce = 5f; // ジャンプの力

    Rigidbody2D rb;
    bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        FlipPlayer();
    }
    void Move()
    {
        // 地面に接触しているかどうかを確認
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        // 左右の移動
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // ジャンプ
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
    void FlipPlayer()
    {
        // 現在の移動方向を確認
        if (rb.velocity.x > 0.1f)
        {
            // 右を向く
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (rb.velocity.x < -0.1f)
        {
            // 左を向く
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
    void OnDrawGizmos()
    {
        // グラウンドチェックの範囲をギズモで表示する
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}
