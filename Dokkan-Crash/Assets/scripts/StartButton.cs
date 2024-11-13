using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AlphaButton : MonoBehaviour, IPointerClickHandler
{
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsPixelOpaque(eventData))
        {
            // クリック時の動作をここに記述
            Debug.Log("画像の不透明部分がクリックされました！");
        }
    }

    private bool IsPixelOpaque(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            image.rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        Rect rect = image.rectTransform.rect;
        int x = Mathf.FloorToInt((localPoint.x - rect.x) * image.sprite.pixelsPerUnit / rect.width);
        int y = Mathf.FloorToInt((localPoint.y - rect.y) * image.sprite.pixelsPerUnit / rect.height);

        // 画像の透明度を取得
        Color color = image.sprite.texture.GetPixel(x, y);
        return color.a > 0.1f; // 不透明度が高い場合のみクリック判定
    }
}
