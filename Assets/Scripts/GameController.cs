using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    [SerializeField]
    HexGenerator hexGenerator;
    private bool touching = false;
    private GameObject SelectedObject = null;
    // Use this for initialization
    void Start () {
		
	}
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
    void touchBegin(Vector2 screenPosition)
    {

    }
    void touchHold(Vector2 screenPosition)
    {
        if (hexGenerator.isPlaying)
        {
            GameObject curSelect = ObjectClicked(screenPosition);
            if (curSelect != SelectedObject)
            {
                if (SelectedObject != null)
                {
                    SelectedObject.transform.GetChild(0).DOScale(1f, 0.3f).SetEase(Ease.OutCirc);
                }
                SelectedObject = curSelect;
                if (SelectedObject != null)
                {
                    SelectedObject.transform.GetChild(0).DOScale(0.8f, 0.3f).SetEase(Ease.OutCirc);
                }
            }
        }
       
    }
    void touchEnd(Vector2 screenPosition)
    {
        if (SelectedObject != null)
        {
            SelectedObject.GetComponent<Hex>().changeNum();
            SelectedObject.transform.GetChild(0).DOScale(1f, 0.5f).SetEase(Ease.OutElastic);
            hexGenerator.HasChange();
            SelectedObject = null;
        }
    }
    public void BackClick()
    {
        //SceneManager.LoadScene("SelectLevel");
        Initiate.Fade("SelectLevel", new Color(1, 1, 1), 5.0f);
    }
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackClick();
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
