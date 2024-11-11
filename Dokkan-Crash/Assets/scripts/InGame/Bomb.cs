using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float explosionRadius = 1.5f;
    private Tilemap targetTile;
    public void SetUpTile(Tilemap tilemap){
        targetTile = tilemap;
    }
    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ground"))
    //     {
    //         //爆発時イメージを表示
    //         Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    //         // 爆風の範囲内のオブジェクトを検出
    //         Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
    //         foreach (Collider2D col in colliders)
    //         {
    //             // オブジェクトがTilemapに含まれている場合のみタイルを削除する
    //             Vector3Int cellPosition = targetTile.WorldToCell(col.transform.position);
    //             if (targetTile.HasTile(cellPosition))
    //             {
    //                 targetTile.SetTile(cellPosition, null); // タイルを削除する
    //             }
    //         }
    //         Destroy(gameObject); // 自分を破壊
    //     }
    // }
}
