using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour, BaseBomb
{
    public GameObject explosion; // 爆発エフェクト
    public GameObject debrisEffect; // 削除タイルの瓦礫エフェクト
    public float damage = 0.2f; // 弾が与えるダメージ量
    private bool onDamage = false; //ダメージを与えたどうか
    public float explosionRadius = 1f; // タイル削除の範囲（半径）
    public float playerDamageRadius = 2.0f; // プレイヤーにダメージを与える範囲
    private Rigidbody2D rb;
    private GameManager gameManager;

    private void Start()
    {
        onDamage = false;
        gameManager = FindObjectOfType<GameManager>();
    }

    // 発射方向と速度を設定するメソッド
    public void SetDirection(Vector2 direction, float speed)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction.normalized * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 共通の爆発処理
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);

        // タイル削除処理
        Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();
        if (tilemap != null)
        {
            RemoveTilesInRadius(tilemap, collision.contacts[0].point);
        }

        // プレイヤーへのダメージ処理
        DamagePlayersInRadius(collision);

        // ターン終了処理
        if (gameManager != null && !gameManager.isGameOver)
        {
            gameManager.EndTurn();
        }
    }

    private void RemoveTilesInRadius(Tilemap tilemap, Vector3 hitPosition)
    {
        Vector3Int centerCell = tilemap.WorldToCell(hitPosition);
        int radius = Mathf.CeilToInt(explosionRadius);

        // 範囲内のタイルを削除
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                Vector3Int tilePos = new Vector3Int(centerCell.x + x, centerCell.y + y);
                float distance = Vector3Int.Distance(tilePos, centerCell);

                if (distance <= explosionRadius)
                {
                    // タイル削除前のエフェクト
                    if (debrisEffect != null)
                    {
                        Vector3 tileWorldPos = tilemap.CellToWorld(tilePos) + tilemap.tileAnchor;
                        Instantiate(debrisEffect, tileWorldPos, Quaternion.identity);
                    }

                    tilemap.SetTile(tilePos, null); // タイルを削除
                }
            }
        }
    }

    private void DamagePlayersInRadius(Collision2D collision)
    {
        // プレイヤーへの直撃ダメージ処理
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage * 2); // 直撃時はダメージ2倍
            }
        }

        // 範囲攻撃のダメージ処理
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, playerDamageRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                PlayerController player = collider.GetComponent<PlayerController>();
                if (player != null && !onDamage)
                {
                    player.TakeDamage(damage); // 通常ダメージ
                    onDamage = true;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDamageRadius);
    }
}
