using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour, BaseBomb
{
    public GameObject explosion;
    public float damage = 0.2f; // 弾が与えるダメージ量
    private bool onDamage = false; //ダメージを与えたどうか
    public float explosionRadius = 1f; // タイル削除の範囲（半径）
    public float playerDamageRadius = 2.0f; // プレイヤーにダメージを与える範囲
    private Rigidbody2D rb;
    private GameManager gameManager;
    private bool hasExploded = false; // 爆発処理が行われたかを確認するフラグ

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
        if (hasExploded) return; // 処理が1度実行された場合は何もしない
        hasExploded = true; // 処理中にフラグを設定

        // 共通の爆発処理（プレイヤーやタイル破壊の有無にかかわらず実行）
        Destroy(gameObject); // 爆弾を消す
        Instantiate(explosion, transform.position, Quaternion.identity);

        // タイルマップの破壊処理
        Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();
        if (tilemap != null)
        {
            // 衝突した位置を取得
            Vector3 hitPosition = collision.contacts[0].point;
            Vector3Int centerCell = tilemap.WorldToCell(hitPosition);

            // 爆発範囲の半径を整数に変換
            int radius = Mathf.CeilToInt(explosionRadius);

            // 範囲内のタイルを削除
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    // 中心セルからの相対距離
                    int dx = x;
                    int dy = y;

                    // 距離の二乗が爆発半径の二乗以下の場合、範囲内と判断
                    if (dx * dx + dy * dy <= explosionRadius * explosionRadius)
                    {
                        Vector3Int tilePos = new Vector3Int(centerCell.x + x, centerCell.y + y, centerCell.z);
                        tilemap.SetTile(tilePos, null); // タイルを削除
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
                onDamage = true; //onDamageをオンに（爆風の判定とかぶらないように
                player.TakeDamage(damage * 2); //直撃は難しいからダメージ２倍
            }
        }

        

        // プレイヤーにダメージを与える範囲攻撃処理
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, playerDamageRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                PlayerController player = collider.GetComponent<PlayerController>();
                if (player != null && !onDamage) //onDamageをオンに（直撃ダメージの判定とかぶらないように
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
