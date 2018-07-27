using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonManager : MonoBehaviour {

    public void _BackToMenu()
    {
        Initiate.Fade("SelectPlanet", new Color(1, 1, 1), 7.0f);
    }
    public void _nextresetGameScene()
    {
       // Menu.instance.DestroyMenu();
        SceneManager.LoadScene("ResetScene");
    }
    public void _resetGame() {
        PlayerPrefs.SetInt("PlayingPlanet", -1);
        PlayerPrefs.SetInt("PlayerLevel", 0);
        PlayerPrefs.SetInt("CompleteLastPlanet", 0);
        //PlayerPrefs.SetString("PlanetComplete", "");
    }
    public void CreateBtnClick()
    {
        //SceneManager.LoadScene("ListMap");
        // Initiate.Fade("ListMap", new Color(1, 1, 1), 5.0f);
        SceneManager.LoadScene("ListMap");
    }
}
