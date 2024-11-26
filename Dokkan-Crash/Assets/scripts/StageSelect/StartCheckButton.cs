using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartCheckButton : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // 遷移先のシーン名

    public GameObject playerNameObj;
    public GameObject roundCountObj;

    public Button startButton;                    // スタートボタン
    public Button nextButton;                     // ネクストボタン
    public Button backButton;                     // バックボタン
    public Button fightButton;                    // ファイトボタン 
    public Button addButton;                      // プレイヤー名設定ボタン
    public Button[] roundCountButtons;            // ラウンドカウントボタン群
    private bool roundCountButtonClick = false;   // ラウンドカウントが設定されたか

    private bool playerName = false;
    private bool roundCount = false;

    void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        backButton.onClick.AddListener(OnBackButtonClick);
        nextButton.onClick.AddListener(OnNextButtonClick);

        foreach (Button button in roundCountButtons)
        {
            button.onClick.AddListener(() =>
            {
                //ラウンドボタン群が押されたときの処理
                roundCountButtonClick = true;
            });
        }
    }
    void OnBackButtonClick()
    {
        if (playerName == false && roundCount == true)
        {
            playerName = true;
            roundCount = false;
            roundCountObj.transform.DOLocalMove(new Vector3(1800, -120, 0), 1f);
            playerNameObj.transform.DOLocalMove(new Vector3(0, -120, 0), 1f);
        }
    }
    void OnNextButtonClick()
    {
        if (playerName == true && roundCount == false)
        {
            playerName = false;
            roundCount = true;
            roundCountObj.transform.DOLocalMove(new Vector3(0, -120, 0), 1f);
            playerNameObj.transform.DOLocalMove(new Vector3(-1800, -120, 0), 1f);
        }
    }

    void OnStartButtonClick()
    {
        if (playerName == false && roundCount == false)
        {
            playerName = true;
            playerNameObj.transform.DOLocalMove(new Vector3(0, -120, 0), 1f);
        }
        // プレイヤー名またはラウンドカウントが未設定の場合、警告を出して処理を中止
        else if (roundCountButtonClick == false || SettingsManager.Instance.playerDataList.Count < 2)
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
