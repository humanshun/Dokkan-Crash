using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
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
