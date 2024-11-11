using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject exprosion;
    public int damage = 1; // 弾が与えるダメージ量
    private Rigidbody2D rb;

    // 発射方向と速度を設定するメソッド
    public void SetDirection(Vector2 direction, float speed)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController1 player = collision.gameObject.GetComponent<PlayerController1>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
        
        Destroy(gameObject); // 弾を消す
        Instantiate(exprosion, transform.position, Quaternion.Euler(0, 0, 0));
    }
}
