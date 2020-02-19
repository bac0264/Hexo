using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditListMap : MonoBehaviour {

    public GameObject ListContainer,ItemPrefab;
    List<string> listMapId=new List<string>();
    private void Awake()
    {
       // string listidstr=SaveSystem.GetString("ListMapId");
        string listidstr = PlayerPrefs.GetString("ListMapId");
        Debug.Log(listidstr);
        if (listidstr != "")
        {
            listMapId.AddRange(listidstr.Split('|'));
            foreach (string id in listMapId)
            {
                Debug.Log("id: "+id);
                GameObject item = Instantiate(ItemPrefab, ListContainer.transform);
                item.GetComponentInChildren<Text>().text = "Map ID " + id;
                item.GetComponent<Button>().onClick.AddListener(delegate { ItemClick(item, id); });
                item.GetComponentsInChildren<Button>()[1].onClick.AddListener(delegate { DelItem(item, id); });
            }
        }
        
    }
    void saveListMapId()
    {
        string s = "";
        foreach (string id in listMapId)
        {
            s += id+"|";
        }
        s = s.TrimEnd('|');
        PlayerPrefs.SetString("ListMapId", s);
    }
    public void BackClick()
    {
        SceneManager.LoadScene("Mode1");
       // Initiate.Fade("SelectPlanet", new Color(1, 1, 1), 5.0f);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) BackClick();
    }
    public void AddClick() {
        int id = PlayerPrefs.GetInt("LastId");
        PlayerPrefs.SetInt("LastId", id + 1);
        string strId = id.ToString();
        GameObject item=Instantiate(ItemPrefab, ListContainer.transform);
        item.GetComponentInChildren<Text>().text = "Map ID" + strId;
        item.GetComponent<Button>().onClick.AddListener(delegate { ItemClick(item, strId); });
        item.GetComponentsInChildren<Button>()[1].onClick.AddListener(delegate { DelItem(item, strId); });
        listMapId.Add(strId);
        saveListMapId();
        
    }
    void ItemClick(GameObject obj,string id)
    {
        EditMapHexGenerator.mapId = id;
        SceneManager.LoadScene("CreateMap");
        //Initiate.Fade("CreateMap", new Color(1, 1, 1), 5.0f);
    }
    void DelItem(GameObject obj,string id)
    {
        listMapId.Remove(id);
        saveListMapId();
        Destroy(obj);
    }
}
