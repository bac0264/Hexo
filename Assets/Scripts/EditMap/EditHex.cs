using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditHex : MonoBehaviour {
    [SerializeField]
    private EditMapHexGenerator HexGenObj;

    private bool touching = false;
    private GameObject SelectedObject = null;
    //This method returns the game object that was clicked using Raycast 2D
    GameObject ObjectClicked(Vector2 screenPosition)
    {
        //Converting Mouse Pos to 2D (vector2) World Pos
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
        Vector2 rayPos = new Vector2(worldPos.x, worldPos.y);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
        if (hit)
        {
            return hit.transform.gameObject;
        }
        else return null;
    }
    bool checkHasTri(GameObject obj, int direction)
    {
        if (obj.transform.childCount > 1)
        {
            for (int i = 1; i < obj.transform.childCount; i++)
            {
                if (obj.transform.GetChild(i).GetComponent<Tri>().Direction == direction)
                {
                    GameObject nearHex = HexGenObj.getHex(obj.GetComponent<Hex>().Pos, direction);
                    if (nearHex != null)
                        HexGenObj.numTriInHex[(int)nearHex.GetComponent<Hex>().Pos.x, (int)nearHex.GetComponent<Hex>().Pos.y]--;
                    HexGenObj.RemoveTri(obj.transform.GetChild(i).gameObject);
                    return true;
                }
            }
        }
        return false;
    }
    void touchBegin(Vector2 screenPosition)
    {
        SelectedObject= ObjectClicked(screenPosition);
    }
    void touchHold(Vector2 screenPosition)
    {
     
    }
    void touchEnd(Vector2 screenPosition)
    {
        GameObject curSelect = ObjectClicked(screenPosition);
        //edit hex
        if (SelectedObject != null && curSelect==SelectedObject)
        {
            if (HexGenObj.numTriInHex[(int)SelectedObject.GetComponent<Hex>().Pos.x, (int)SelectedObject.GetComponent<Hex>().Pos.y] == 0)
            {
                SelectedObject.GetComponent<Hex>().changeNum();
                if(SelectedObject.GetComponent<Hex>().Num == 0)
                {
                    Tri[] listTri=SelectedObject.GetComponentsInChildren<Tri>();
                    foreach (Tri tri in listTri)
                    {
                        GameObject nearHex = HexGenObj.getHex(tri.GetComponent<Tri>().Pos, tri.GetComponent<Tri>().Direction);
                        if (nearHex != null)
                            HexGenObj.numTriInHex[(int)nearHex.GetComponent<Hex>().Pos.x, (int)nearHex.GetComponent<Hex>().Pos.y]--;
                        HexGenObj.RemoveTri(tri.gameObject);
                    }
                }
                HexGenObj.UpdateTri();
                SelectedObject.transform.GetChild(0).DOScale(0.8f, 0.5f).SetEase(Ease.OutElastic).From();
            }
        }
        //tao tam giac
        if (SelectedObject != null && curSelect!=SelectedObject)
        {
            if (SelectedObject.GetComponent<Hex>().Num > 0)
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
                Vector2 directionVector = new Vector2(worldPos.x - SelectedObject.transform.position.x, worldPos.y - SelectedObject.transform.position.y);
                int Angle = Mathf.RoundToInt(Vector2.SignedAngle(Vector2.up, directionVector) / 60) * 60;
                Angle = Angle < 0 ? Angle += 360 : Angle;
                if (!checkHasTri(SelectedObject, Angle / 60))
                {
                    HexGenObj.AddTri(SelectedObject, Angle / 60);
                }
            }
        }
        SelectedObject = null;
    }

    public void BackBtn()
    {
        SceneManager.LoadScene("ListMap");
        //Initiate.Fade("ListMap", new Color(1, 1, 1), 5.0f);
    }
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackBtn();
        }
        if (Input.GetMouseButtonDown(0) && !touching)
        {
            touching = true;
            touchBegin(Input.mousePosition);
        }
        if (touching)
        {
            touchHold(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            touching = false;
            touchEnd(Input.mousePosition);
        }
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                touchBegin(Input.GetTouch(0).position);
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                touchHold(Input.GetTouch(0).position);
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                touchEnd(Input.GetTouch(0).position);
            }
        }
    }
}
