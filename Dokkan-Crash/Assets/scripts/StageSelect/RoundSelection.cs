using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundSelection : MonoBehaviour
{
    [SerializeField] private Button r1;
    [SerializeField] private Button r2;
    [SerializeField] private Button r3;
    [SerializeField] private Button r4;
    [SerializeField] private TextMeshProUGUI roundCountText; // プレイヤー一覧を表示するテキスト

    private void Start()
    {
        r1.onClick.AddListener(() => SetRoundCount(1));
        r2.onClick.AddListener(() => SetRoundCount(2));
        r3.onClick.AddListener(() => SetRoundCount(3));
        r4.onClick.AddListener(() => SetRoundCount(4));
    }
    public void SetRoundCount(int roundCount)
    {
        SettingsManager.Instance.roundCount = roundCount;
        UpdatePlayerList();
    }
    private void UpdatePlayerList()
    {
        roundCountText.text = $"Round {SettingsManager.Instance.roundCount}\n";
    }
}

