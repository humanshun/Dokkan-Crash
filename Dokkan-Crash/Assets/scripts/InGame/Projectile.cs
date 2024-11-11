using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController1 player = collision.gameObject.GetComponent<PlayerController1>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
        
        Destroy(gameObject); // 弾を消す
    }
}
