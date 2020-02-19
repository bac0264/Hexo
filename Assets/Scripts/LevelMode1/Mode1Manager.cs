using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Mode1Manager : MonoBehaviour
{
    public void Home()
    {
        SceneManager.LoadScene("Menu");
    }
}
