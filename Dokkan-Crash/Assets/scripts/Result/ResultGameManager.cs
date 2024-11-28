using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultGameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI victoryText;
    //１位プレイヤーのテキスト
    [SerializeField] private TextMeshProUGUI nameText1;
    [SerializeField] private TextMeshProUGUI pointText1;
    [SerializeField] private TextMeshProUGUI winsText1;
    [SerializeField] private TextMeshProUGUI killsText1;

    //２位プレイヤーのテキスト
    [SerializeField] private TextMeshProUGUI nameText2;
    [SerializeField] private TextMeshProUGUI pointText2;
    [SerializeField] private TextMeshProUGUI winsText2;
    [SerializeField] private TextMeshProUGUI killsText2;

    //３位プレイヤーのテキスト
    [SerializeField] private TextMeshProUGUI nameText3;
    [SerializeField] private TextMeshProUGUI pointText3;
    [SerializeField] private TextMeshProUGUI winsText3;
    [SerializeField] private TextMeshProUGUI killsText3;

    //ボタン
    [SerializeField] private Button backButton;

    void Start()
    {
        backButton.onClick.AddListener(BackToStageSelect);
        // プレイヤーデータを取得して表示
        DisplayTopResults();
    }

    private void DisplayTopResults()
    {
        if (SettingsManager.Instance != null)
        {
            // プレイヤーデータを取得して勝利数で降順にソート
            List<PlayerData> sortedList = new List<PlayerData>(SettingsManager.Instance.playerDataList);
            sortedList.Sort((a, b) => b.winCount.CompareTo(a.winCount));

            // 1位、2位、3位をそれぞれのテキストに表示
            if (sortedList.Count > 0)
            {
                victoryText.text = $"{sortedList[0].playerName}";
                nameText1.text = $"{sortedList[0].playerName}";
                pointText1.text = $"{sortedList[0].point}point";
                winsText1.text = $"{sortedList[0].winCount}wins";
                killsText1.text = $"{sortedList[0].killCount}kills";
            }
            else
                nameText1.text = "No player";

            if (sortedList.Count > 1)
            {
                nameText2.text = $"{sortedList[1].playerName}";
                pointText2.text = $"{sortedList[1].point}point";
                winsText2.text = $"{sortedList[1].winCount}wins";
                killsText2.text = $"{sortedList[1].killCount}kills";
            }
                
            else
                nameText2.text = "No player";

            if (sortedList.Count > 2)
            {
                nameText3.text = $"{sortedList[2].playerName}";
                pointText3.text = $"{sortedList[2].point}point";
                winsText3.text = $"{sortedList[2].winCount}wins";
                killsText3.text = $"{sortedList[2].killCount}kills";
            }
                
            else
                nameText3.text = "No player";
        }
        else
        {
            // データがない場合は全て「No results」と表示
            nameText1.text = "No results";
            nameText2.text = "No results";
            nameText3.text = "No results";
        }
    }
    private void BackToStageSelect()
    {
        GameManager.roundNumber = 1;
        // 設定をリセット
        SettingsManager.Instance.ResetSettings();

        FadeManager.Instance.FadeToScene("StageSelect");
    }
}
