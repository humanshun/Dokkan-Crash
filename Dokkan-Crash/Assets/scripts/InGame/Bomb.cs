using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    public GameObject explosion;
    public float damage = 0.2f; // 弾が与えるダメージ量
    public float explosionRadius = 1f; // タイル削除の範囲（半径）
    public float playerDamageRadius = 2.0f; // プレイヤーにダメージを与える範囲
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
        // タイルマップの破壊処理
        Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();
        if (tilemap != null)
        {
            Vector3 hitPosition = collision.contacts[0].point;
            Vector3Int centerCell = tilemap.WorldToCell(hitPosition);

            for (int x = -Mathf.CeilToInt(explosionRadius); x <= Mathf.CeilToInt(explosionRadius); x++)
            {
                for (int y = -Mathf.CeilToInt(explosionRadius); y <= Mathf.CeilToInt(explosionRadius); y++)
                {
                    Vector3Int tilePos = new Vector3Int(centerCell.x + x, centerCell.y + y, centerCell.z);

                    if (Vector3Int.Distance(tilePos, centerCell) <= explosionRadius)
                    {
                        tilemap.SetTile(tilePos, null);
                    }
                }
            }
        }

        // プレイヤーに直接衝突した場合のダメージ処理
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }

        // 共通の爆発処理（プレイヤーやタイル破壊の有無にかかわらず実行）
        Destroy(gameObject); // 爆弾を消す
        Instantiate(explosion, transform.position, Quaternion.identity);

        // プレイヤーにダメージを与える範囲攻撃処理
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, playerDamageRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                PlayerController player = collider.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }
            }
        }

        // ターン終了処理
        if (gameManager != null && gameManager.isGameOver != true)
        {
            gameManager.EndTurn();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // プレイヤーにダメージを与える範囲を視覚化
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDamageRadius);
    }
}
