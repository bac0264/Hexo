using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevelManager : MonoBehaviour {
    public GameObject ItemPrefab;
    public Sprite lockImage, unlockImage;
    public static int PlanetID;
    public List<string> listMapId = new List<string>();
    private const int MAX = 8;
    public Transform[] listContainer;
    // Use this for initialization
    private void Start()
    {
        //FollowPosition.start = false;
        string listidstr = PlayerPrefs.GetString("ListMapId");
        int playerLevel = PlayerPrefs.GetInt("PlayerLevel");
        
        if (listidstr != "")
        {
            listMapId.AddRange(listidstr.Split('|'));
            _SelectLvCase(PlanetID, playerLevel);
        }
    }
    public void _SelectLvCase(int mapID, int playerLevel)
    {
        int length = MAX * (mapID + 1) / PlayerPrefs.GetInt("LastPlanetID");
        switch (mapID) {
            case 0:
                _makeMap(mapID, HexGenerator.isWinning, playerLevel,length);
                break;
            case 1:
                _makeMap(mapID, HexGenerator.isWinning, playerLevel, length);
                break;
            case 2:
                _makeMap(mapID, HexGenerator.isWinning, playerLevel, length);
                break;
            case 3:
                _makeMap(mapID, HexGenerator.isWinning, playerLevel, length);
                break;
            default: break;
        };
    }
    public void _makeMap(int mapID, int isWinning, int playerLevel, int length) {
        for (int i = mapID *isWinning; i < length; i++)
        {
            GameObject item = Instantiate(ItemPrefab, listContainer[i % length]);
            int lv = i;
            item.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { LevelClick(item, lv); });
            if (i > playerLevel)
            {
                item.transform.GetChild(0).GetComponent<Image>().sprite = lockImage;
                item.transform.GetChild(0).GetComponent<Button>().enabled = false;
            }
            else
            {
                item.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
                item.transform.GetChild(0).GetComponent<Image>().sprite = unlockImage;
                item.transform.GetChild(0).GetComponent<Button>().interactable = true;
            }
        }
    }
    void LevelClick(GameObject obj, int lv)
    {
        Debug.Log("run");
        HexGenerator.mapId = listMapId[lv];
        HexGenerator.level = lv;
        Initiate.Fade("MainGame", new Color(1, 1, 1), 5.0f);
    }
    public void BackClick()
    {
       // PlayerPrefs.SetInt("PlanetIsPlaying", PlanetID);
        Initiate.Fade("SelectPlanet", new Color(1, 1, 1), 5.0f);
        //StartCoroutine(TimeToBack());
    }
    //IEnumerator TimeToBack()
    //{
    //    yield return new WaitForSeconds(1.0f);
    //  //  Menu.instance.gameObject.SetActive(true);
    //}
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) BackClick();
    }
    public static void setPlanetID(int x)
    {
        PlanetID = x;
    }

}
