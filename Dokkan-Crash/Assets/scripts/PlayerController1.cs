using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    private bool isGrounded; // 地面に接地しているかどうかのフラグ
    public LayerMask groundLayer; // 地面として扱うレイヤーマスク
    public LayerMask maskedGroundLayer; // マスクで消された地面のレイヤーマスク
    public float checkRadius;
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
        isGrounded = Physics2D.OverlapCircle(transform.position, checkRadius, groundLayer);

        if (isGrounded)
        {
            // 地面にいる時の処理
            playerCollider.enabled = true; // コライダーを有効にする（地面と衝突する）
            rb.isKinematic = false; // 物理挙動を有効にする（重力が影響する）
        }
        else if (Physics2D.OverlapCircle(transform.position, checkRadius, maskedGroundLayer))
        {
            // マスクで消された地面にいる時の処理（すり抜ける）
            playerCollider.enabled = false; // コライダーを無効にする（地面と衝突しない）
            rb.isKinematic = true; // 物理挙動を無効にする（重力が影響しない）
        }
        else
        {
            // どの地面にも接触していない時の処理（通常の空中状態）
            playerCollider.enabled = true; // コライダーを有効にする
            rb.isKinematic = false; // 物理挙動を有効にする（重力が影響する）
        }
    }

    private void OnDrawGizmos()
    {
        // Scene ビューでこの範囲を表示するための処理
        Gizmos.color = Color.red; // ギズモの色を赤に設定
        Gizmos.DrawWireSphere(transform.position, checkRadius); // 範囲をワイヤーフレームの球として描画
    }
}
