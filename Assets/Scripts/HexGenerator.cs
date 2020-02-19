using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HexGenerator : MonoBehaviour {
    [SerializeField]
    private GameObject HexPrefab;
    [SerializeField]
    private GameObject TrianglePrefab;
    public List<GameObject> listTri;
    public GameObject[,] hexMatrix= new GameObject[GameSetting.cols, GameSetting.rows];
    public int mapId;
    public static float HexHeight, HexWidth;
    public const int isWinning = 2;
    public bool isPlaying = true;
    public Text level;

    // Use this for initialization
    void Start()
    {
        mapId = PlayerPrefs.GetInt("LevelPickedUp");
        level.text = "Level: " + (mapId + 1);
        // Load data đã chọn từ slect map
        HexHeight = HexPrefab.GetComponent<SpriteRenderer>().bounds.size.y * GameSetting.hexOffset;
        HexWidth = HexPrefab.GetComponent<SpriteRenderer>().bounds.size.x * GameSetting.hexOffset;
        transform.localPosition = new Vector3(-(GameSetting.cols - 1) * (3.1f * HexWidth / 4) / 2, (-(GameSetting.rows + 0.5f) / 2 + 1) * HexHeight, 10);
        string data = PlayerPrefs.GetString("data" + mapId.ToString());
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
    public void HasChange()
    {
        bool[,] hexComplete=new bool[GameSetting.cols,GameSetting.rows];
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
                obj.transform.DOScale(0, 0.5f).SetEase(Ease.InBack).SetDelay(Random.Range(0f,0.5f));
            }
        }
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Mode1");
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
}
