using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    public GameObject explosionPrefab;
    [SerializeField]
    Tilemap destroyTile;
    public float explosionRadius = 1.5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //爆発時イメージを表示
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject); // 自分を破壊
            Destroy(collision.gameObject); // 当たったオブジェクトを破壊
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            //爆発時イメージを表示
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // 爆風の範囲内のオブジェクトを検出
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (Collider2D col in colliders)
            {
                // オブジェクトがTilemapに含まれている場合のみタイルを削除する
                Vector3Int cellPosition = destroyTile.WorldToCell(col.transform.position);
                if (destroyTile.HasTile(cellPosition))
                {
                    destroyTile.SetTile(cellPosition, null); // タイルを削除する
                }
            }
            Destroy(gameObject); // 自分を破壊
        }
    }
}
