using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject circularMaskPrefab; // 円形マスクのプレハブ
    public LayerMask groundLayer; // 地面として扱うレイヤーマスク
    public float explosionRadius = 1.0f; // 爆発の半径

    public void Explode(Vector2 position)
    {
        // 爆発範囲の地面オブジェクトを取得
        Collider2D[] groundObjects = Physics2D.OverlapCircleAll(position, explosionRadius, groundLayer);

        // 円形マスクオブジェクトを生成
        Instantiate(circularMaskPrefab, position, Quaternion.identity);

        // 地面オブジェクトを破壊
        foreach (Collider2D groundObject in groundObjects)
        {
            Destroy(groundObject.gameObject); // 地面オブジェクトを破壊する
        }
    }
}
