using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isGrounded; // 地面に接地しているかどうかのフラグ
    public LayerMask groundLayer; // 地面として扱うレイヤーマスク
    private Collider2D playerCollider; // プレイヤーのコライダー
    private Rigidbody2D rb; // プレイヤーの Rigidbody2D

    void Start()
    {
        // 初期化処理
        playerCollider = GetComponent<Collider2D>(); // プレイヤーのコライダーを取得
        rb = GetComponent<Rigidbody2D>(); // プレイヤーの Rigidbody2D を取得
    }

    void Update()
    {
        // 地面にいるかどうかを判定する
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.1f, groundLayer);

        if (isGrounded)
        {
            // 地面にいる時の処理
            playerCollider.enabled = true; // コライダーを有効にする（地面と衝突する）
            rb.isKinematic = false; // 物理挙動を有効にする（重力が影響する）
        }
        else
        {
            // 地面にいない時の処理（すり抜ける）
            playerCollider.enabled = false; // コライダーを無効にする（地面と衝突しない）
            rb.isKinematic = true; // 物理挙動を無効にする（重力が影響しない）
        }
    }
}
