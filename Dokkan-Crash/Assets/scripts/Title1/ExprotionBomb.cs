using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExprotionBomb : MonoBehaviour
{
    public GameObject exprotoinPrefab;

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Block"))
        {
            Destroy(gameObject);
            GameObject instance = Instantiate(exprotoinPrefab, transform.position, Quaternion.identity);
        }
    }
}
