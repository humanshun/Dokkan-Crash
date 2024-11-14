using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleManager : MonoBehaviour
{
    public string sceneName = "Title2";
    public int sceneChangeSecond;
    void Start()
    {
        StartCoroutine(SceneChange());
    }
    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(sceneChangeSecond);
        SceneManager.LoadScene(sceneName);
    }
}
