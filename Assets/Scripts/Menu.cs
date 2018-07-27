using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    public static Menu instance;
    public GameObject ButtonPref;
    public GameObject LockPref;
    public GameObject[] planet;
    public List<Sprite> imageList;
    public Sprite replay;
    List<string> listPlanetComplete = new List<string>();
    private void Awake()
    {
       // PlayerPrefs.SetString("PlanetComplete", "");
        //PlayerPrefs.SetInt("PlanetIsPlaying", -1);
       // PlayerPrefs.SetInt("PlayerLevel", 0);
        IsGameStartedForTheFirstTime();
        _spaceshipSetup();
        _makeInstance();
        _SetUpPlanetID();
    }
    void IsGameStartedForTheFirstTime()
    {

            if (!PlayerPrefs.HasKey("IsGameStartedForTheFirstTime"))
            {
                PlayerPrefs.SetInt("PlanetIsPlaying", -1);
                PlayerPrefs.SetInt("IsGameStartedForTheFirstTime", 0);
            }

    }
    private void _spaceshipSetup()
    {
        int index = PlayerPrefs.GetInt("PlanetIsPlaying");
        if (index >= 0)
        {
            Spaceship.instance.transform.position = planet[index].transform.position;
            Camera.main.transform.position = new Vector3(Spaceship.instance.transform.position.x, Spaceship.instance.transform.position.y, Camera.main.transform.position.z);
        }
    }
    private void _makeInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    private void _SetUpPlanetID()
    {
        if (planet != null)
        {
            int i = 0;
            foreach (GameObject _planet in planet)
            {
                _planet.GetComponent<Planet>().PlanetID = i;
                if (i == 0)
                {
                    GameObject item = Instantiate(ButtonPref, _planet.transform);
                    item.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate { ItemClick(item, _planet, _planet.GetComponent<Planet>().PlanetID); });
                }
                i++;
            }
            PlayerPrefs.SetInt("LastPlanetID", i);
            string listidstr = PlayerPrefs.GetString("PlanetComplete");
            listidstr = listidstr.TrimEnd('|');
            //Debug.Log("LIST PLANET:" +listidstr);
            int index = 0;
            bool isLooked = false;
            if (listidstr != "")
            {
                listPlanetComplete.AddRange(listidstr.Split('|'));
                foreach (GameObject _planet in planet)
                {
                    if (index != 0)
                    {
                        foreach (string id in listPlanetComplete)
                        {

                            if (_planet.GetComponent<Planet>().PlanetID == int.Parse(id))
                            {
                                isLooked = true;
                            }

                        }
                        if (isLooked == false)
                        { 
                            _planet.transform.GetChild(0).gameObject.SetActive(false);
                            _planet.GetComponent<SpriteRenderer>().sprite = imageList[_planet.GetComponent<Planet>().PlanetID + 2];
                            GameObject item = Instantiate(LockPref, _planet.transform);
                        }
                        else
                        {
                            _planet.GetComponent<SpriteRenderer>().sprite = imageList[_planet.GetComponent<Planet>().PlanetID -1];
                            GameObject item = Instantiate(ButtonPref, _planet.transform);
                            item.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate { ItemClick(item, _planet, _planet.GetComponent<Planet>().PlanetID); });
                            isLooked = false;
                        }
                    }
                    index++;
                }
            }
            else
            {
                int k = 0;
                foreach (GameObject _planet in planet)
                {
                    if (k != 0)
                    {
                        _planet.transform.GetChild(0).gameObject.SetActive(false);
                        _planet.GetComponent<SpriteRenderer>().sprite = imageList[_planet.GetComponent<Planet>().PlanetID + 2];
                        GameObject item = Instantiate(LockPref, _planet.transform);
                        //  item.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate { ItemClick(item, _planet, _planet.GetComponent<Planet>().PlanetID); });
                    }
                    k++;
                }
            }
        }
    }
    public void _MakeButton(GameObject _planet)
    {
        Transform lockpr =_planet.transform.FindChild("LockPref(Clone)");
        if (lockpr != null) Destroy(lockpr.gameObject);
        _planet.GetComponent<SpriteRenderer>().sprite = imageList[_planet.GetComponent<Planet>().PlanetID - 1];
        GameObject item = Instantiate(ButtonPref, _planet.transform);
        _planet.transform.GetChild(0).gameObject.SetActive(true);
        item.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate { ItemClick(item, _planet, _planet.GetComponent<Planet>().PlanetID); });
    }
    void ItemClick(GameObject obj, GameObject _planet, int id)
    {
        //FollowPosition.start = true;
        SelectLevelManager.setPlanetID(id);
        _planet.GetComponent<Planet>()._Move();
        StartCoroutine(TimeToPlay());
    }
    IEnumerator TimeToPlay()
    {
        yield return new WaitForSeconds(2f);
        Initiate.Fade("SelectLevel", new Color(1, 1, 1), 5.0f);
        gameObject.SetActive(false);
    }

    public void DestroyMenu()
    {
        Destroy(gameObject);
    }
}
