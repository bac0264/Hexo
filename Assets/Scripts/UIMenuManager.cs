using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIMenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Mode1");
    }
}
