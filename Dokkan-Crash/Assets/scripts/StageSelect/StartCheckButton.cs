using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCheckButton : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // 遷移先のシーン名
    public Button startButton;                    // スタートボタン
    public Button addButton;                      // プレイヤー名設定ボタン
    public Button[] roundCountButtons;            // ラウンドカウントボタン群
    private bool roundCountButtonClick = false;   // ラウンドカウントが設定されたか

    void Start()
    {
        startButton.onClick.AddListener(OnButtonClick);

        foreach (Button button in roundCountButtons)
        {
            button.onClick.AddListener(() =>
            {
                //ラウンドボタン群が押されたときの処理
                roundCountButtonClick = true;
            });
        }
    }

    void OnButtonClick()
    {
        // プレイヤー名またはラウンドカウントが未設定の場合、警告を出して処理を中止
        if (roundCountButtonClick == false || SettingsManager.Instance.playerDataList.Count < 2)
        {
            Debug.LogWarning("PlayerNameまたはRoundCountが設定されていません。");
            return;
        }
        else
        {
            // 条件を満たしている場合にシーン遷移
            FadeManager.Instance.FadeToScene(nextSceneName);
        }
    }
}
