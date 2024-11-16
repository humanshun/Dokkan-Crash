using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { get; private set; }
    public Canvas fadeCanvas; // FadeManagerの子オブジェクトに配置されたCanvas
    public Image fadeImage;   // Canvasの子オブジェクトのFadeImage
    public float fadeDuration = 1.0f; // フェード時間

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // フェード用Canvasを非アクティブに設定
            if (fadeCanvas != null)
                fadeCanvas.gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutIn(sceneName));
    }

    private IEnumerator FadeOutIn(string sceneName)
    {
        // フェード用Canvasをアクティブに
        if (fadeCanvas != null)
            fadeCanvas.gameObject.SetActive(true);

        // フェードアウト（画面を暗くする）
        yield return StartCoroutine(Fade(1.0f));

        // シーン切り替え
        SceneManager.LoadScene(sceneName);

        // シーン切り替え後の初期化待ち
        yield return null;

        // フェードイン（画面を明るくする）
        yield return StartCoroutine(Fade(0.0f));

        // フェード終了後にCanvasを非アクティブに
        if (fadeCanvas != null)
            fadeCanvas.gameObject.SetActive(false);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeImage == null) yield break; // Imageが存在しない場合は処理を終了

        float startAlpha = fadeImage.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            if (fadeImage == null) yield break; // フェード中にImageが削除された場合をチェック

            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, newAlpha);
            yield return null;
        }

        if (fadeImage != null)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, targetAlpha);
        }
    }
}
