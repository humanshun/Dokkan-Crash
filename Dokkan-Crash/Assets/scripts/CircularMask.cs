using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMask : MonoBehaviour
{
    public float radius = 1.0f; // 円形の範囲の半径
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 他のオブジェクトがこの範囲に入った時の処理
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(collision.gameObject); // "Ground" タグを持つオブジェクトを破壊する
        }
    }

    private void OnDrawGizmos()
    {
        // Scene ビューでこの範囲を表示するための処理
        Gizmos.color = Color.red; // ギズモの色を赤に設定
        Gizmos.DrawWireSphere(transform.position, radius); // 範囲をワイヤーフレームの球として描画
    }
}
