using UnityEngine;
using System;
using TMPro; // TextMeshProの名前空間を追加

public class GameManager : MonoBehaviour
{
    public PlayerMovement playerPrefab; // プレイヤーのプレハブを参照
    public TextMeshProUGUI turnText; // TextをTextMeshProUGUIに変更
    public Animator textAnimator;

    public PlayerMovement[] players;
    private int currentPlayerIndex = 0;
    public bool isGameOver = false;

    public int CurrentPlayerIndex
    {
        get { return currentPlayerIndex; }
    }

    private void Start()
    {
        InitializePlayers();
        UpdateTurnUI();
        StartTurn();
    }

    private void InitializePlayers()
    {
        int playerCount = SettingsManager.Instance.playerNames.Count;

        players = new PlayerMovement[playerCount];

        for (int i = 0; i < playerCount; i++)
        {
            // ランダムなスポーン位置を計算
            float randomX = UnityEngine.Random.Range(-17f, 17f); // X軸の範囲をランダムに選択
            Vector3 spawnPosition = new Vector3(randomX, 12f, 0f); // Y軸は固定値12

            // プレイヤーを生成
            PlayerMovement newPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            newPlayer.playerName = SettingsManager.Instance.playerNames[i]; // プレイヤー名を設定
            players[i] = newPlayer;
        }
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
        // 現在のプレイヤーのターンを終了
        players[currentPlayerIndex].EndTurn();

        // 次のプレイヤーにターンを回す
        do
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        }
        while (!players[currentPlayerIndex].IsAlive); // 生存しているプレイヤーが見つかるまでループ

        // ターンUIを更新
        UpdateTurnUI();

        // ゲームオーバーを確認
        CheckGameOver();

        // ゲームが終了していない場合に次のターンを開始
        if (!isGameOver)
        {
            Invoke("StartTurn", 1f);
        }
    }


    private void UpdateTurnUI()
    {
        turnText.text = "Player " + (currentPlayerIndex + 1) + "'s Turn";
        textAnimator.Play("TurnText");
    }

    public void CheckGameOver()
    {
        // 生存しているプレイヤーを確認
        int alivePlayers = 0;
        // 勝利したプレイヤーを探す
        PlayerMovement winningPlayer = null;
        foreach (var player in players)
        {
            if (player.IsAlive) // プレイヤーが生存している場合に勝者として設定
            {
                alivePlayers++;
                winningPlayer = player;
            }
            
        }

        // 生存プレイヤーが1人以下の場合にゲーム終了
        if (alivePlayers <= 1)
        {
            isGameOver = true; // ゲームオーバーフラグを設定
            textAnimator.Play("TextGameOver"); // ゲームオーバーアニメーションを再生

            if (winningPlayer != null)
            {
                // 勝者がいる場合
                turnText.text = "Player " + (Array.IndexOf(players, winningPlayer) + 1) + " Wins!";
            }
            else
            {
                // 勝者がいない場合（全員敗北）
                turnText.text = "Game Over - Draw!";
            }
        }
    }
}
