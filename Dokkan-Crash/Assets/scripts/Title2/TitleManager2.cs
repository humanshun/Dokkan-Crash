using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager2 : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    public Button startButton;
    void Start()
    {
        startButton.onClick.AddListener(OnButtonClick);
    }
    void OnButtonClick()
    {
        FadeManager.Instance.FadeToScene(nextSceneName);
    }
}
