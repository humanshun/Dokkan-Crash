using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundSelection : MonoBehaviour
{
    [SerializeField] private Button r1;
    [SerializeField] private Button r2;
    [SerializeField] private Button r3;
    [SerializeField] private Button r4;

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
        Debug.Log($"ラウンド数が {roundCount} に設定されました！");
    }
}

