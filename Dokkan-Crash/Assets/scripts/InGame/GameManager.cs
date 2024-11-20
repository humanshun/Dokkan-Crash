using UnityEngine;
using System;
using TMPro;
using System.Collections; // TextMeshProの名前空間を追加

public class GameManager : MonoBehaviour
{
    public PlayerMovement playerPrefab;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI roundText;
    public static int roundNumber = 1;
    public Animator turnTextAnimator;
    public Animator roundTextAnimator;
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
        StartTurn();
        StartCoroutine(RoundText());
    }
    IEnumerator RoundText()
    {
        roundText.text = $"{roundNumber} Round";
        roundTextAnimator.Play("Round");
        yield return new WaitForSeconds(2);
        UpdateTurnUI();
    }

    private void InitializePlayers()
    {
        int playerCount = SettingsManager.Instance.playerDataList.Count;

        players = new PlayerMovement[playerCount];

        for (int i = 0; i < playerCount; i++)
        {
            // ランダムなスポーン位置を計算
            float randomX = UnityEngine.Random.Range(-17f, 17f); // X軸の範囲をランダムに選択
            Vector3 spawnPosition = new Vector3(randomX, 12f, 0f); // Y軸は固定値12

            // プレイヤーを生成
            PlayerMovement newPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

            // プレイヤー名を設定
            string playerName = SettingsManager.Instance.playerDataList[i].playerName;
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
            if (players[currentPlayerIndex].chargeSlider != null)
            {
                players[currentPlayerIndex].chargeSlider.gameObject.SetActive(true);
            }
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
        turnTextAnimator.Play("TurnText");
    }

    public void CheckGameOver()
    {
        // 生存しているプレイヤーを確認
        int alivePlayers = 0;
        PlayerMovement winningPlayer = null;

        foreach (var p in players)
        {
            if (p.IsAlive)
            {
                alivePlayers++;
                winningPlayer = p; // 生存プレイヤーを勝者として仮定
            }
        }

        // 生存プレイヤーが1人以下の場合にゲーム終了
        if (alivePlayers <= 1 && !isGameOver)
        {
            isGameOver = true; // ゲームオーバーフラグを設定
            turnTextAnimator.Play("TextGameOver"); // ゲームオーバーアニメーションを再生

            if (winningPlayer != null)
            {
                // 勝者がいる場合はその名前を表示
                turnText.text = $"{winningPlayer.playerName} Wins!";

                // 勝者の勝利カウントを増やす
                var playerData = SettingsManager.Instance.playerDataList.Find(p => p.playerName == winningPlayer.playerName);
                if (playerData != null)
                {
                    playerData.winCount++;
                    // デバッグログを追加
                    Debug.Log($"Player '{playerData.playerName}' won! Total wins: {playerData.winCount}");
                }
            }
            else
            {
                // 勝者がいない場合（全員敗北）
                turnText.text = "Game Over - Draw!";
            }

            // ラウンドカウントをデクリメント
            SettingsManager.Instance.roundCount--;
            roundNumber++;

            // 残りラウンドがある場合
            if (SettingsManager.Instance.roundCount > 0)
            {
                FadeManager.Instance.FadeToScene("InGame"); // 次のラウンドに移行
            }
            else
            {
                // ラウンドが終了した場合
                Debug.Log("All rounds completed.");
                // 必要に応じてタイトル画面やリザルト画面に遷移
                FadeManager.Instance.FadeToScene("result");
            }
        }
    }

}
