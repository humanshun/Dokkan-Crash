using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleInstanceBomb : MonoBehaviour
{
    public GameObject bombPrefab;
    public float throwForce = 500f;

    private void Start()
    {
        GameObject instance = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * throwForce);
        }
        else
        {
            Debug.LogWarning("リジッドボディがアタッチされてない");
        }
    }
}
