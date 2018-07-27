using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public void CreateBtnClick()
    {
        //SceneManager.LoadScene("ListMap");
        Initiate.Fade("ListMap", new Color(1, 1, 1), 5.0f);
    }
    public void StartBtnClick()
    {
       // SceneManager.LoadScene("SelectLevel");
        Initiate.Fade("SelectLevel", new Color(1, 1, 1), 5.0f);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
