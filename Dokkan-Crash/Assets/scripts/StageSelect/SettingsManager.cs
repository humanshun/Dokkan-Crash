using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public List<PlayerData> playerDataList = new List<PlayerData>(); // プレイヤー名のリスト
    public int roundCount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetSettings() // リセット用のメソッド
    {
        playerDataList.Clear(); // プレイヤーデータをクリア
        roundCount = 0;         // ラウンドカウントをリセット
    }
}
