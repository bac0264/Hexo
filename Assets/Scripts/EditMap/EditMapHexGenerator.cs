using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditMapHexGenerator : MonoBehaviour {

    [SerializeField]
    private GameObject HexPrefab;
    [SerializeField]
    private GameObject TrianglePrefab;
    int cols=GameSetting.cols, rows=GameSetting.rows;
    [HideInInspector]
    public GameObject[,] hexMatrix;
    [HideInInspector]
    public List<GameObject> listTri;
    public int[,] numTriInHex;
    public static float HexHeight,HexWidth;
    public static string mapId;
    // Use this for initialization
    private void Awake()
    {
        hexMatrix = new GameObject[cols, rows];
        numTriInHex = new int[cols, rows];
        HexHeight = HexPrefab.GetComponent<SpriteRenderer>().bounds.size.y*GameSetting.hexOffset;
        HexWidth = HexPrefab.GetComponent<SpriteRenderer>().bounds.size.x*GameSetting.hexOffset;
    }
    void Start () {
        transform.localPosition = new Vector3(-(cols - 1) * (3.1f * HexWidth / 4) / 2, (-(rows + 0.5f) / 2 + 1) * HexHeight, 10);
        string data = PlayerPrefs.GetString("data" + mapId.ToString());
        if (data != "")
        {
            string[] arr = data.Split('|');
            int numOfHex = int.Parse(arr[0]);
            int c = 0;
            for (int i = 0; i < numOfHex; i++)
            {
                c += 3;
                Vector2 pos = new Vector2(int.Parse(arr[c - 2]), int.Parse(arr[c - 1]));
                HexPrefab.GetComponent<Hex>().Num = int.Parse(arr[c]);
                HexPrefab.GetComponent<Hex>().Pos = pos;
                HexPrefab.transform.localPosition = new Vector3(pos.x * (3.1f * HexWidth / 4), pos.y * HexHeight - (pos.x % 2) * (HexHeight / 2));
                hexMatrix[(int)pos.x, (int)pos.y] = Instantiate(HexPrefab, transform);
            }
            c++;
            int numOfTri = int.Parse(arr[c]);
            for (int i = 0; i < numOfTri; i++)
            {
                c += 4;
                Vector2 pos = new Vector2(int.Parse(arr[c - 3]), int.Parse(arr[c - 2]));
                AddTri(hexMatrix[(int)pos.x, (int)pos.y], int.Parse(arr[c]));
            }
        }
        else
        {
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    HexPrefab.GetComponent<Hex>().Num = 0;
                    HexPrefab.GetComponent<Hex>().Pos = new Vector2(i, j);
                    HexPrefab.transform.localPosition = new Vector3(i * (3.1f * HexWidth / 4), j * HexHeight - (i % 2) * (HexHeight / 2));
                    hexMatrix[i, j] = Instantiate(HexPrefab, transform);
                }
            }
        }
    }
    
    void SaveData()
    {
        string data = "";
        data += cols * rows+"|";
        foreach(GameObject obj in hexMatrix)
        {
            data += obj.GetComponent<Hex>().Pos.x + "|";
            data += obj.GetComponent<Hex>().Pos.y + "|";
            data += obj.GetComponent<Hex>().Num+"|";
        }
        data += listTri.Count + "|";
        foreach(GameObject obj in listTri)
        {
            data += obj.GetComponent<Tri>().Pos.x + "|";
            data += obj.GetComponent<Tri>().Pos.y + "|";
            data += obj.GetComponent<Tri>().Num + "|";
            data += obj.GetComponent<Tri>().Direction + "|";
        }
        data = data.TrimEnd('|');
        PlayerPrefs.SetString("data"+mapId.ToString(), data);
    }
    private int calTri(Tri tri)
    {
        GameObject curHex = hexMatrix[(int)tri.Pos.x, (int)tri.Pos.y];
        int res = curHex.GetComponent<Hex>().Num;
        while (true)
        {
            curHex = getHex(curHex.GetComponent<Hex>().Pos, (tri.Direction + 3) % 6);
            if (curHex != null)
            {
                if (curHex.GetComponent<Hex>().Num > 0)
                {
                    res += curHex.GetComponent<Hex>().Num;
                }
                else break;
            }
            else break;
        }
        return res;
    }
    public void UpdateTri()
    {
        foreach(GameObject obj in listTri)
        {
            obj.GetComponent<Tri>().Num = calTri(obj.GetComponent<Tri>());
            obj.GetComponent<Tri>().Refresh();
        }
        SaveData();
    }
    public void AddTri(GameObject parent,int direction)
    {
        GameObject nearParent = getHex(parent.GetComponent<Hex>().Pos, direction);
        if (nearParent != null)
        {
            if (nearParent.GetComponent<Hex>().Num == 0)
            {
                TrianglePrefab.GetComponent<Tri>().Direction = direction;
                TrianglePrefab.GetComponent<Tri>().Pos = parent.GetComponent<Hex>().Pos;
                listTri.Add(Instantiate(TrianglePrefab, parent.transform));
                numTriInHex[(int)nearParent.GetComponent<Hex>().Pos.x, (int)nearParent.GetComponent<Hex>().Pos.y]++;
                UpdateTri();
            }
        }
        else
        {
            TrianglePrefab.GetComponent<Tri>().Direction = direction;
            TrianglePrefab.GetComponent<Tri>().Pos = parent.GetComponent<Hex>().Pos;
            listTri.Add(Instantiate(TrianglePrefab, parent.transform));
            UpdateTri();
        }
    }
    public void RemoveTri(GameObject obj)
    {
        listTri.Remove(obj);
        Destroy(obj);
        SaveData();
    }
    public GameObject getHex(Vector2 originPos,int direction)
    {
        switch(direction){
            case 0:
                {
                    if ((originPos.y + 1) < rows)
                        return hexMatrix[(int)originPos.x, (int) originPos.y + 1];
                }
                break;
            case 1:
                {
                    if ((originPos.x - 1) >= 0 && (originPos.y + (int)(originPos.x+1) % 2) < rows)
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
                    if ((originPos.y - 1) >=0)
                        return hexMatrix[(int)originPos.x, (int)originPos.y -1 ];
                }
                break;
            case 4:
                {
                    if ((originPos.x + 1) < cols && (originPos.y - (int)originPos.x % 2) >= 0)
                        return hexMatrix[(int)(originPos.x + 1), (int)(originPos.y - (int)originPos.x % 2)];
                }
                break;

            case 5:
                {
                    if ((originPos.x + 1) < cols && (originPos.y + (int)(originPos.x + 1) % 2) < rows)
                        return hexMatrix[(int)(originPos.x + 1), (int)(originPos.y + (int)(originPos.x + 1) % 2)];
                }
                break;
            default: break;
        }
        return null;
    }
}
