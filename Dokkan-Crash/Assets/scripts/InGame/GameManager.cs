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

            // プレイヤー名を設定
            string playerName = SettingsManager.Instance.playerNames[i];
            newPlayer.playerName = playerName;

            // 名前のUIを設定
            if (newPlayer.playerNameText != null)
            {
                newPlayer.playerNameText.text = playerName; // 頭上の名前を表示
            }

            players[i] = newPlayer;
        }
    }



    private void StartTurn()
    {
        if (!isGameOver)
        {
            players[currentPlayerIndex].StartTurn();
            players[currentPlayerIndex].chargeSlider.gameObject.SetActive(true);
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
        // 現在のプレイヤー名を取得
        string currentPlayerName = players[currentPlayerIndex].playerName;

        // UIにプレイヤー名を表示
        turnText.text = $"{currentPlayerName}'s Turn";
        textAnimator.Play("TurnText");
    }

    public void CheckGameOver()
    {
        // 生存しているプレイヤーを確認
        int alivePlayers = 0;
        PlayerMovement winningPlayer = null;

        foreach (var player in players)
        {
            if (player.IsAlive)
            {
                alivePlayers++;
                winningPlayer = player; // 生存プレイヤーを勝者として仮定
            }
        }

        // 生存プレイヤーが1人以下の場合にゲーム終了
        if (alivePlayers <= 1)
        {
            isGameOver = true; // ゲームオーバーフラグを設定
            textAnimator.Play("TextGameOver"); // ゲームオーバーアニメーションを再生

            if (winningPlayer != null)
            {
                // 勝者がいる場合はその名前を表示
                turnText.text = $"{winningPlayer.playerName} Wins!";
            }
            else
            {
                // 勝者がいない場合（全員敗北）
                turnText.text = "Game Over - Draw!";
            }
        }
    }

}
