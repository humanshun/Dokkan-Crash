using UnityEngine;
using System;
using TMPro; // TextMeshProの名前空間を追加

public class GameManager : MonoBehaviour
{
    public PlayerMovement[] players; 
    public TextMeshProUGUI turnText; // TextをTextMeshProUGUIに変更
    public Animator textAnimator;

    private int currentPlayerIndex = 0;
    public bool isGameOver = false;

    public int CurrentPlayerIndex
    {
        get { return currentPlayerIndex; }
    }

    private void Start()
    {
        UpdateTurnUI();
        StartTurn();
    }

    private void StartTurn()
    {
        if (!isGameOver)
        {
            players[currentPlayerIndex].StartTurn();
        }
    }

    public void EndTurn()
    {
        players[currentPlayerIndex].EndTurn();
        
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        UpdateTurnUI();
        
        Invoke("StartTurn", 1f); 
    }

    private void UpdateTurnUI()
    {
        turnText.text = "Player " + (currentPlayerIndex + 1) + "'s Turn";
        textAnimator.Play("TurnText");
    }

    public void CheckGameOver()
    {
        // 勝利したプレイヤーを探す
        PlayerMovement winningPlayer = null;
        foreach (var player in players)
        {
            if (player.IsAlive) // プレイヤーが生存している場合に勝者として設定
            {
                winningPlayer = player;
                break;
            }
        }

        // アニメーション再生とゲームオーバーフラグの設定
        textAnimator.Play("TextGameOver");
        isGameOver = true;

        // 勝利したプレイヤーが存在するかチェック
        if (winningPlayer != null)
        {
            // 勝利プレイヤーを表示
            turnText.text = "Player " + (Array.IndexOf(players, winningPlayer) + 1) + " Wins!";
        }
        else
        {
            // 勝者がいない場合（例: 引き分け）
            turnText.text = "Game Over - Draw!";
        }
    }

}
