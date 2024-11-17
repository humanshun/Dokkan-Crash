using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        // SettingsManager が存在するかチェック
        if (SettingsManager.Instance == null)
        {
            Debug.LogError("SettingsManager のインスタンスが見つかりません。");
            return;
        }

        // プレイヤー人数とラウンド数を取得
        int playerCount = SettingsManager.Instance.playerNames.Count; // プレイヤー名リストを基準
        int roundCount = SettingsManager.Instance.roundCount;

        // 条件判定
        if (playerCount <= 0)
        {
            Debug.LogWarning("プレイヤー名が設定されていません！");
            return;
        }

        if (roundCount <= 0)
        {
            Debug.LogWarning("ラウンド数が設定されていません！");
            return;
        }

        // 条件を満たしている場合
        Debug.Log($"ゲームを開始します！ プレイヤー数: {playerCount}, ラウンド数: {roundCount}");
        SceneManager.LoadScene("GameScene"); // シーン名を設定
    }
}
