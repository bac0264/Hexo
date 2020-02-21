using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingGame : MonoBehaviour
{
    public Image LoadingBar;
    public string levelLoad;

    private void Start()
    {
        LoadingBar.fillAmount = 0f;
        if (PlayerPrefs.GetInt("IsTheFirst") == 0)
        {
            StartCoroutine(AsynchronousLoad("Tutorial"));
            PlayerPrefs.SetInt("IsTheFirst", 1);
        }
        else StartCoroutine(AsynchronousLoad(levelLoad));
    }
    public IEnumerator AsynchronousLoad(string scene)
    {
        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            LoadingBar.fillAmount = ao.progress;
            if (ao.progress == 0.9f)
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}