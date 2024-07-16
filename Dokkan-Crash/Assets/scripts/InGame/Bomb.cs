using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionPrefab;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player"))
        {
            //爆発時イメージを表示
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject); // 自分を破壊
            Destroy(collision.gameObject); // 当たったオブジェクトを破壊
        }
    }
}
