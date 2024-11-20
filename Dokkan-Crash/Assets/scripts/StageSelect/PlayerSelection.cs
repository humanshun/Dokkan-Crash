using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSelection : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField; // 名前入力用の入力フィールド
    [SerializeField] private Button addButton; // プレイヤー追加ボタン
    [SerializeField] private TextMeshProUGUI playerListText; // プレイヤー一覧を表示するテキスト
    [SerializeField] private int maxPlayers = 4; // プレイヤーの最大人数

    private void Start()
    {
        // ボタンにクリック時の処理を登録
        addButton.onClick.AddListener(AddPlayer);
    }

    public void AddPlayer()
    {
        // プレイヤーの最大人数に達しているか確認
        if (SettingsManager.Instance.playerDataList.Count >= maxPlayers)
        {
            Debug.LogWarning($"The maximum number of players ({maxPlayers}) has been reached!");
            return;
        }

        string playerName = nameInputField.text;

        // 入力が空の場合は処理を中断
        if (string.IsNullOrWhiteSpace(playerName))
        {
            Debug.LogWarning("Please enter a player name!");
            return;
        }

        // 重複する名前が存在しないかチェック
        foreach (var player in SettingsManager.Instance.playerDataList)
        {
            if (player.playerName == playerName)
            {
                Debug.LogWarning($"Player '{playerName}' is already in the list!");
                return;
            }
        }

        // プレイヤー名をリストに追加
        SettingsManager.Instance.playerDataList.Add(new PlayerData(playerName));
        Debug.Log($"Player '{playerName}' has been added.");

        // プレイヤーリストを更新して表示
        UpdatePlayerList();

        // 入力フィールドをクリア
        nameInputField.text = "";
    }

    private void UpdatePlayerList()
    {
        // プレイヤーリストをテキスト形式に変換して表示
        playerListText.text = "Player List:\n";
        foreach (var player in SettingsManager.Instance.playerDataList)
        {
            playerListText.text += $"- {player.playerName} (Wins: {player.winCount})\n";
        }

        // 残りスロット数を表示
        int remainingSlots = maxPlayers - SettingsManager.Instance.playerDataList.Count;
        playerListText.text += $"\nRemaining Slots: {remainingSlots}";
    }
}
