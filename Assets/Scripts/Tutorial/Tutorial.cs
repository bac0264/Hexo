using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject HexPrefab;
    [SerializeField]
    private GameObject TrianglePrefab;
    public List<GameObject> listTri;
    public GameObject[,] hexMatrix = new GameObject[GameSetting.cols, GameSetting.rows];
    public string _data;
    public static float HexHeight, HexWidth;
    public const int isWinning = 2;
    public bool isPlaying = true;
    public int count;
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;
    public GameObject Loading;
    // Use this for initialization
    void Start()
    {
        _data = "77|0|0|0|0|1|0|0|2|0|0|3|0|0|4|0|0|5|0|0|6|0|0|7|0|0|8|0|0|9|0|0|10|0|1|0|0|1|1|0|1|2|0|1|3|0|1|4|0|1|5|0|1|6|0|1|7|0|1|8|0|1|9|0|1|10|0|2|0|0|2|1|0|2|2|0|2|3|0|2|4|0|2|5|0|2|6|0|2|7|0|2|8|0|2|9|0|2|10|0|3|0|0|3|1|0|3|2|0|3|3|0|3|4|0|3|5|0|3|6|1|3|7|0|3|8|0|3|9|0|3|10|0|4|0|0|4|1|0|4|2|0|4|3|0|4|4|0|4|5|0|4|6|0|4|7|0|4|8|0|4|9|0|4|10|0|5|0|0|5|1|0|5|2|0|5|3|0|5|4|0|5|5|0|5|6|0|5|7|0|5|8|0|5|9|0|5|10|0|6|0|0|6|1|0|6|2|0|6|3|0|6|4|0|6|5|0|6|6|0|6|7|0|6|8|0|6|9|0|6|10|0|1|3|6|1|5";
        if (LoadData(_data))
        {

        }
        else SceneManager.LoadScene("Mode1");
    }
    public virtual bool LoadData(string data)
    {
        count++;
        if (count == 1)
        {
            obj1.SetActive(true);
        }
        else if (count == 2)
        {
            obj1.SetActive(false);
            obj2.SetActive(true);
        }
        // Load data đã chọn từ slect map
        HexHeight = HexPrefab.GetComponent<SpriteRenderer>().bounds.size.y * GameSetting.hexOffset;
        HexWidth = HexPrefab.GetComponent<SpriteRenderer>().bounds.size.x * GameSetting.hexOffset;
        transform.localPosition = new Vector3(-(GameSetting.cols - 1) * (3.1f * HexWidth / 4) / 2, (-(GameSetting.rows + 0.5f) / 2 + 1) * HexHeight, 10);
        if (data == "") return false;
        string[] arr = data.Split('|');
        int numOfHex = int.Parse(arr[0]);
        int c = 0;
        for (int i = 0; i < numOfHex; i++)
        {
            c += 3;
            if (int.Parse(arr[c]) > 0)
            {
                Vector2 pos = new Vector2(int.Parse(arr[c - 2]), int.Parse(arr[c - 1]));
                HexPrefab.GetComponent<Hex>().Num = 0;
                HexPrefab.GetComponent<Hex>().Pos = pos;
                HexPrefab.transform.localPosition = new Vector3(pos.x * (3.1f * HexWidth / 4), pos.y * HexHeight - (pos.x % 2) * (HexHeight / 2));
                hexMatrix[(int)pos.x, (int)pos.y] = Instantiate(HexPrefab, transform);
            }
        }
        c++;
        int numOfTri = int.Parse(arr[c]);
        // Tạo tam giác dựa vào vị trí và hướng của hex
        for (int i = 0; i < numOfTri; i++)
        {
            c += 4;
            Vector2 pos = new Vector2(int.Parse(arr[c - 3]), int.Parse(arr[c - 2]));
            TrianglePrefab.GetComponent<Tri>().Direction = int.Parse(arr[c]);
            TrianglePrefab.GetComponent<Tri>().Pos = pos;
            TrianglePrefab.GetComponent<Tri>().Num = int.Parse(arr[c - 1]);
            listTri.Add(Instantiate(TrianglePrefab, hexMatrix[(int)pos.x, (int)pos.y].transform));
        }
        isPlaying = true;
        return true;
    }
    // Tính toán theo hướng của Tri
    private int calTri(Tri tri)
    {
        GameObject curHex = hexMatrix[(int)tri.Pos.x, (int)tri.Pos.y];
        int res = curHex.GetComponent<Hex>().Num;
        if (res == 0) return -1;
        while (true)
        {
            curHex = getHex(curHex.GetComponent<Hex>().Pos, (tri.Direction + 3) % 6);
            if (curHex != null)
            {
                if (curHex.GetComponent<Hex>().Num > 0)
                {
                    res += curHex.GetComponent<Hex>().Num;
                }
                else return -1;
            }
            else break;
        }
        return res;
    }
    public virtual void HasChange()
    {
        bool[,] hexComplete = new bool[GameSetting.cols, GameSetting.rows];
        int d = 0;
        foreach (GameObject tri in listTri)
        {
            if (tri.GetComponent<Tri>().Num == calTri(tri.GetComponent<Tri>()))
            {
                tri.GetComponent<SpriteRenderer>().color = GameSetting.hexToColor("84cceb");
                GameObject curHex = hexMatrix[(int)tri.GetComponent<Tri>().Pos.x, (int)tri.GetComponent<Tri>().Pos.y];
                hexComplete[(int)curHex.GetComponent<Hex>().Pos.x, (int)curHex.GetComponent<Hex>().Pos.y] = true;
                while (true)
                {
                    curHex = getHex(curHex.GetComponent<Hex>().Pos, (tri.GetComponent<Tri>().Direction + 3) % 6);
                    if (curHex != null)
                    {
                        hexComplete[(int)curHex.GetComponent<Hex>().Pos.x, (int)curHex.GetComponent<Hex>().Pos.y] = true;
                    }
                    else break;
                }
                d++;
            }
            else
            {
                tri.GetComponent<SpriteRenderer>().color = GameSetting.hexToColor("ffffff");
            }
        }
        for (int i = 0; i < GameSetting.cols; i++)
        {
            for (int j = 0; j < GameSetting.rows; j++)
            {
                if (hexMatrix[i, j] != null)
                {
                    if (hexComplete[i, j])
                    {
                        hexMatrix[i, j].GetComponent<SpriteRenderer>().color = GameSetting.hexToColor("84cceb");
                    }
                    else
                    {
                        hexMatrix[i, j].GetComponent<SpriteRenderer>().color = GameSetting.hexToColor("ffffff");
                    }
                }
            }
        }
        if (d == listTri.Count)
        {
            StartCoroutine(LevelComplete());
        }
    }
    IEnumerator LevelComplete()
    {
        isPlaying = false;
        foreach (GameObject obj in hexMatrix)
        {
            if (obj != null)
            {
                obj.transform.DOScale(0, 0.5f).SetEase(Ease.InBack).SetDelay(Random.Range(0f, 0.5f));
            }
        }

        yield return new WaitForSeconds(1f);
        if (count == 2)
        {
            obj3.SetActive(true);
            obj2.SetActive(false);
        }
        else
        {
            _data = "77|0|0|0|0|1|0|0|2|0|0|3|0|0|4|0|0|5|0|0|6|0|0|7|0|0|8|0|0|9|0|0|10|0|1|0|0|1|1|0|1|2|0|1|3|0|1|4|0|1|5|0|1|6|0|1|7|0|1|8|0|1|9|0|1|10|0|2|0|0|2|1|0|2|2|0|2|3|0|2|4|0|2|5|0|2|6|0|2|7|0|2|8|0|2|9|0|2|10|0|3|0|0|3|1|0|3|2|0|3|3|0|3|4|0|3|5|0|3|6|1|3|7|0|3|8|0|3|9|0|3|10|0|4|0|0|4|1|0|4|2|0|4|3|0|4|4|0|4|5|0|4|6|1|4|7|0|4|8|0|4|9|0|4|10|0|5|0|0|5|1|0|5|2|0|5|3|0|5|4|0|5|5|0|5|6|0|5|7|0|5|8|0|5|9|0|5|10|0|6|0|0|6|1|0|6|2|0|6|3|0|6|4|0|6|5|0|6|6|0|6|7|0|6|8|0|6|9|0|6|10|0|1|4|6|2|5";
            if (LoadData(_data))
            {

            }
        }
    }
    public GameObject getHex(Vector2 originPos, int direction)
    {
        switch (direction)
        {
            case 0:
                {
                    if ((originPos.y + 1) < GameSetting.rows)
                        return hexMatrix[(int)originPos.x, (int)originPos.y + 1];
                }
                break;
            case 1:
                {
                    if ((originPos.x - 1) >= 0 && (originPos.y + (int)(originPos.x + 1) % 2) < GameSetting.rows)
                        return hexMatrix[(int)(originPos.x - 1), (int)(originPos.y + (int)(originPos.x + 1) % 2)];
                }
                break;
            case 2:
                {
                    if ((originPos.x - 1) >= 0 && (originPos.y - (int)originPos.x % 2) >= 0)
                        return hexMatrix[(int)(originPos.x - 1), (int)(originPos.y - (int)originPos.x % 2)];
                }
                break;
            case 3:
                {
                    if ((originPos.y - 1) >= 0)
                        return hexMatrix[(int)originPos.x, (int)originPos.y - 1];
                }
                break;
            case 4:
                {
                    if ((originPos.x + 1) < GameSetting.cols && (originPos.y - (int)originPos.x % 2) >= 0)
                        return hexMatrix[(int)(originPos.x + 1), (int)(originPos.y - (int)originPos.x % 2)];
                }
                break;

            case 5:
                {
                    if ((originPos.x + 1) < GameSetting.cols && (originPos.y + (int)(originPos.x + 1) % 2) < GameSetting.rows)
                        return hexMatrix[(int)(originPos.x + 1), (int)(originPos.y + (int)(originPos.x + 1) % 2)];
                }
                break;
            default: break;
        }
        return null;
    }
    public void Menu()
    {
        Loading.SetActive(true);
    }
}
