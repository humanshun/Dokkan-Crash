using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpBomb : MonoBehaviour, BaseBomb
{
    private Rigidbody2D rb;          // Rigidbody2Dコンポーネント
    private GameManager gameManager; // GameManager参照
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // 爆弾の発射方向と速度を設定するメソッド
    public void SetDirection(Vector2 direction, float speed)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction.normalized * speed;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // GameManagerから現在のプレイヤーを取得してワープ
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            PlayerMovement currentPlayer = gameManager.players[gameManager.CurrentPlayerIndex];
            if (currentPlayer != null && currentPlayer.IsAlive)
            {
                // 現在のターンプレイヤーを爆発地点に移動
                currentPlayer.transform.position = transform.position;
                Debug.Log($"{currentPlayer.playerName} warped to the bomb's location!");
            }
        }

        Destroy(gameObject); // 爆弾を破壊

        // ターン終了処理
        if (gameManager != null && gameManager.isGameOver != true)
        {
            gameManager.EndTurn();
        }
    }
}
