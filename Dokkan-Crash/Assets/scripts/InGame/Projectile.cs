using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject exprosion;
    public float damage = 0.2f; // 弾が与えるダメージ量
    private Rigidbody2D rb;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // 発射方向と速度を設定するメソッド
    public void SetDirection(Vector2 direction, float speed)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
        if (collision.gameObject.CompareTag("Block"))
        {
            Destroy(collision.gameObject);
        }
        
        
        Destroy(gameObject); // 弾を消す
        Instantiate(exprosion, transform.position, Quaternion.Euler(0, 0, 0));
        gameManager.EndTurn();
    }
}
