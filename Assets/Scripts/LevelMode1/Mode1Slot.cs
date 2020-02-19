using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mode1Slot : MonoBehaviour
{
    public Button btn;
    private int id;
    public int ID
    {
        set
        {
            id = value;
            level.text = "Level: " + (id + 1).ToString();
        }
        get
        {
            return id;
        }
    }
    public Text level;

    public void Setup()
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(delegate
        {
            SetupBtn();
        });
    }
    void SetupBtn()
    {
        PlayerPrefs.SetInt("LevelPickedUp", ID);
        SceneManager.LoadScene("MainGame");
    }
}
