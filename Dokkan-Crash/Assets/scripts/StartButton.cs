using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour, IPointerDownHandler
{
    private CircleCollider2D circleCollider;
    public string sceneName;
    public Button startButton;

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        startButton.onClick.AddListener(SceneChange);
    }
    private void SceneChange()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 localMousePosition = transform.InverseTransformPoint(eventData.position);
        Debug.Log("Local Mouse Position: " + localMousePosition);
        if (circleCollider.OverlapPoint(localMousePosition))
        {
            Debug.Log("Round button clicked!");
            // ボタンのクリック処理をここに記述
        }
        else
        {
            Debug.Log("Clicked outside the round button.");
        }
    }
}
