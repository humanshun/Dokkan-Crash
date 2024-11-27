using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StartCheckButton : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // 遷移先のシーン名

    public GameObject playerNameObj;
    public GameObject roundCountObj;

    public Button startButton;                    // スタートボタン
    public Button nextButton;                     // ネクストボタン
    public Button backButton;                     // バックボタン
    public Button fightButton;                    // ファイトボタン 
    public GameObject WarningImage;
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
        fightButton.onClick.AddListener(OnFightButtonClick);

        nextButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        fightButton.gameObject.SetActive(false);
        WarningImage.SetActive(false);

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
            nextButton.gameObject.SetActive(true);
            backButton.gameObject.SetActive(false);
            fightButton.gameObject.SetActive(false);
            WarningImage.SetActive(false);
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
            backButton.gameObject.SetActive(true);
            fightButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);
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
            startButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(true);
            playerName = true;
            playerNameObj.transform.DOLocalMove(new Vector3(0, -120, 0), 1f);
            return;
        }
    }

    void OnFightButtonClick()
    {
        // プレイヤー名またはラウンドカウントが未設定の場合、警告を出して処理を中止
        if (roundCountButtonClick == false || SettingsManager.Instance.playerDataList.Count < 2)
        {
            WarningImage.SetActive(true);
            return;
        }
        else
        {
            // 条件を満たしている場合にシーン遷移
            FadeManager.Instance.FadeToScene(nextSceneName);
        }
    }
}
