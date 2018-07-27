using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hex : MonoBehaviour {
    [SerializeField]
    private GameObject TriAnglePrefab;
    [SerializeField]
    private List<Sprite> imgNum;
    public int Num;
    public Vector2 Pos;
	void Start () {
        transform.GetChild(0).gameObject.GetComponentInChildren<SpriteRenderer>().sprite = imgNum[Num];
	}
    public void changeNum()
    {
        Num = (Num + 1) % 4;
        transform.GetChild(0).gameObject.GetComponentInChildren<SpriteRenderer>().sprite = imgNum[Num];
    }
    //public void changeNum(int mapID)
    //{
    //    mapID = mapID % 2;
    //    Num = (Num + mapID + 2) % 8;
    //    transform.GetChild(0).gameObject.GetComponentInChildren<SpriteRenderer>().sprite = imgNum[Num];
    //}
}
