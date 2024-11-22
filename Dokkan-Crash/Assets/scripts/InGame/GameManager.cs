using UnityEngine;
using System;
using TMPro; // TextMeshProの名前空間を追加
using System.Collections;

public class GameManager : MonoBehaviour
{
    // プレイヤープレハブを参照
    public PlayerMovement playerPrefab;
    
    // ターンおよびラウンドのUIテキスト
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI roundText;

    // 現在のラウンド番号（共有用静的変数）
    public static int roundNumber = 1;

    // アニメーション用のAnimator
    public Animator turnTextAnimator;
    public Animator roundTextAnimator;

    // ゲーム内のプレイヤーリスト
    public PlayerMovement[] players;

    // 現在のプレイヤーのインデックス
    private int currentPlayerIndex = 0;

    // ゲームが終了したかどうかのフラグ
    public bool isGameOver = false;

    // 現在のプレイヤーインデックスを外部から参照可能
    public int CurrentPlayerIndex
    {
        get { return currentPlayerIndex; }
    }

    private void Start()
    {
        InitializePlayers(); // プレイヤーを初期化
        ShufflePlayers(); // プレイヤー順をシャッフル
        StartTurn(); // 最初のターンを開始
        StartCoroutine(RoundText()); // ラウンドテキストの表示
    }

    // ラウンド開始時のテキストを表示
    IEnumerator RoundText()
    {
        roundText.text = $"{roundNumber} Round"; // ラウンド番号を設定
        roundTextAnimator.Play("Round"); // アニメーション再生
        yield return new WaitForSeconds(2); // 2秒待機
        UpdateTurnUI(); // ターンUIを更新
    }

    // プレイヤーを初期化する
    private void InitializePlayers()
    {
        int playerCount = SettingsManager.Instance.playerDataList.Count; // プレイヤーデータの数を取得
        players = new PlayerMovement[playerCount]; // プレイヤーリストを作成

        for (int i = 0; i < playerCount; i++)
        {
            // ランダムな位置にスポーン
            float randomX = UnityEngine.Random.Range(-17f, 17f); 
            Vector3 spawnPosition = new Vector3(randomX, 12f, 0f);

            // プレイヤーを生成
            PlayerMovement newPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

            // プレイヤー名を設定
            string playerName = SettingsManager.Instance.playerDataList[i].playerName;
            newPlayer.playerName = playerName;

            // プレイヤー名をUIに表示
            if (newPlayer.playerNameText != null)
            {
                newPlayer.playerNameText.text = playerName;
            }

            players[i] = newPlayer; // プレイヤーをリストに追加
        }
    }

    // プレイヤーリストをシャッフル
    private void ShufflePlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, players.Length); // ランダムなインデックスを取得
            var temp = players[i];
            players[i] = players[randomIndex];
            players[randomIndex] = temp;
        }

        // シャッフル結果をデバッグログに出力
        Debug.Log("Player Order:");
        foreach (var player in players)
        {
            Debug.Log(player.playerName);
        }
    }

    // ターンを開始する
    private void StartTurn()
    {
        if (!isGameOver)
        {
            players[currentPlayerIndex].StartTurn(); // 現在のプレイヤーのターン開始
            if (players[currentPlayerIndex].chargeSlider != null)
            {
                players[currentPlayerIndex].chargeSlider.gameObject.SetActive(true); // スライダーを表示
            }
        }
    }

    // ターンを終了する
    public void EndTurn()
    {
        players[currentPlayerIndex].EndTurn(); // 現在のプレイヤーのターンを終了

        // 次のプレイヤーを選択（生存しているプレイヤーにスキップ）
        do
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        }
        while (!players[currentPlayerIndex].IsAlive);

        UpdateTurnUI(); // ターンUIを更新
        CheckGameOver(); // ゲーム終了条件を確認

        if (!isGameOver)
        {
            Invoke("StartTurn", 1f); // 1秒後に次のターンを開始
        }
    }

    // ターンUIを更新
    private void UpdateTurnUI()
    {
        string currentPlayerName = players[currentPlayerIndex].playerName; // 現在のプレイヤー名を取得
        turnText.text = $"{currentPlayerName}'s Turn"; // UIに反映
        turnTextAnimator.Play("TurnText"); // アニメーション再生
    }

    // ゲーム終了条件を確認
    public void CheckGameOver()
    {
        int alivePlayers = 0; // 生存プレイヤーの数
        PlayerMovement winningPlayer = null;

        foreach (var p in players)
        {
            if (p.IsAlive)
            {
                alivePlayers++;
                winningPlayer = p; // 勝者候補
            }
        }

        if (alivePlayers <= 1 && !isGameOver)
        {
            isGameOver = true; // ゲーム終了フラグを立てる
            turnTextAnimator.Play("TextGameOver"); // ゲームオーバーアニメーション

            if (winningPlayer != null)
            {
                turnText.text = $"{winningPlayer.playerName} Wins!"; // 勝者名を表示
                var playerData = SettingsManager.Instance.playerDataList.Find(p => p.playerName == winningPlayer.playerName);
                if (playerData != null)
                {
                    playerData.winCount++; // 勝利数をカウント
                    Debug.Log($"Player '{playerData.playerName}' won! Total wins: {playerData.winCount}");
                }
            }
            else
            {
                turnText.text = "Game Over - Draw!"; // 引き分けメッセージ
            }

            // 残りラウンドを確認
            SettingsManager.Instance.roundCount--;
            roundNumber++;

            if (SettingsManager.Instance.roundCount > 0)
            {
                FadeManager.Instance.FadeToScene("InGame"); // 次ラウンドへ
            }
            else
            {
                FadeManager.Instance.FadeToScene("result"); // 全ラウンド終了
            }
        }
    }
}
