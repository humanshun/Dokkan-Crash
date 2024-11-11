using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyTilemap : MonoBehaviour
{
    // 削除対象のタイルマップを設定
    public Tilemap tilemap;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 衝突したオブジェクトが "Bomb" タグを持っているか確認
        if (collision.gameObject.CompareTag("Bomb"))
        {
            // 衝突位置を取得
            Vector3 hitPosition = Vector3.zero;

            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x;
                hitPosition.y = hit.point.y;

                // ワールド座標からタイルマップのグリッド位置に変換
                Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);

                // タイルを消す
                tilemap.SetTile(tilePosition, null);
            }
        }
    }
}
