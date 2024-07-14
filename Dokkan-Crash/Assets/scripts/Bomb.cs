using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionPrefab;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject)
        {
            //爆発時イメージを表示
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject); // 自分を破壊
            Destroy(collision.gameObject); // 当たったオブジェクトを破壊
        }
    }
}
