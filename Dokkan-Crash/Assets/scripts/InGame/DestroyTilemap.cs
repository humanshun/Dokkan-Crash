using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyTilemap : MonoBehaviour
{
    public float explosionRadius = 1.5f;
    private Tilemap destroyTile;
    void Start()
    {
        destroyTile = GetComponent<Tilemap>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bomb"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector3 hitPosition = Vector3.zero;
                hitPosition.x = contact.point.x - 0.01f * contact.normal.x;
                hitPosition.y = contact.point.y - 0.01f * contact.normal.y;
                
                Vector3Int cellPosition = destroyTile.WorldToCell(hitPosition);
                destroyTile.SetTile(cellPosition, null); // タイルを削除
            }
            // Debug.Log("ボムが当たりました");
            // Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            // foreach (Collider2D col in colliders)
            // {
            //     // オブジェクトがTilemapに含まれている場合のみタイルを削除する
            //     Vector3Int cellPosition = destroyTile.WorldToCell(col.transform.position);
            //     if (destroyTile.HasTile(cellPosition))
            //     {
            //         destroyTile.SetTile(cellPosition, null); // タイルを削除する
            //     }
            // }
        }
    }
}
