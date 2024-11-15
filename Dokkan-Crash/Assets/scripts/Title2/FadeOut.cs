using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOut : MonoBehaviour
{
    public Image image; // フェードアウトさせるImageコンポーネント
    public float fadeDuration = 1.0f; // フェードアウトの時間（秒）

    private void Start()
    {
        // フェードアウトを開始
        StartCoroutine(StartFadeOut());
    }

    private IEnumerator StartFadeOut()
    {
        // 開始時のアルファ値
        float startAlpha = image.color.a;
        
        // 経過時間
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0, elapsed / fadeDuration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
            yield return null;
        }

        // 完全にフェードアウト後、アルファを0に固定
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }
}
