using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectPlanetManager : MonoBehaviour
{
    bool touching = false, sceneChanging = false;
    Vector2 beginPos;
    public Transform PlanetContainer;
    public Transform SpaceShip;
    public List<Sprite> imageList;
    Vector2 ScrollVelocity;
    int complete;
    private void Awake()
    {
        _IsGameStartedForTheFirstTime();
        _setOpenPlanet();
        _setSpaceshipPosition();
    }
    void _IsGameStartedForTheFirstTime()
    {

        if (!PlayerPrefs.HasKey("IsGameStartedForTheFirstTime"))
        {
            PlayerPrefs.SetInt("PlayingPlanet", -1);
            PlayerPrefs.SetInt("CompleteLastPlanet", 0);
            PlayerPrefs.SetInt("IsGameStartedForTheFirstTime", 0);
        }

    }
    void _setSpaceshipPosition()
    {
        if (PlayerPrefs.GetInt("PlayingPlanet", -1) >= 0)
        {
            Camera.main.transform.position = new Vector3(PlanetContainer.GetChild(PlayerPrefs.GetInt("PlayingPlanet")).position.x, PlanetContainer.GetChild(PlayerPrefs.GetInt("PlayingPlanet")).position.y, -10f);
            SpaceShip.SetParent(PlanetContainer.GetChild(PlayerPrefs.GetInt("PlayingPlanet")).GetComponent<_Planet>().SpaceShipPosition);
            SpaceShip.localPosition = new Vector3();
            SpaceShip.localEulerAngles = new Vector3();
            SpaceShip.localScale = new Vector3(1, 1, 1);
            SpaceShip.DOScale(0.5f, 0.5f).SetEase(Ease.OutElastic).From();
        }
    }
    void _setOpenPlanet()
    {
        PlayerPrefs.SetInt("CompleteLastPlanet", 4);
        complete = PlayerPrefs.GetInt("CompleteLastPlanet");
        PlanetContainer.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = imageList[0];
        for (int i = 1; i < PlanetContainer.childCount; i++)
        {
            if (i <= complete)
            {
                PlanetContainer.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                PlanetContainer.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = imageList[i];
            }
            else
            {
                PlanetContainer.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                PlanetContainer.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = imageList[i + 3];
            }
        }
    }
    /* void touchBegin(Vector2 screenPosition)
     {
         beginPos = screenPosition;
     }
     void touchHold(Vector2 screenPosition)
     {
         Vector2 temp = Camera.main.ScreenToWorldPoint(screenPosition);
         temp = temp - (Vector2)Camera.main.ScreenToWorldPoint((Vector3)beginPos);
       //  if (screenPosition != beginPos)
       //  {
             if (DOTween.TweensById("MoveY", true) != null)
             {
                 foreach (Tween tween in DOTween.TweensById("MoveY", true))
                 {
                     tween.Kill();
                 }
             }
             Camera.main.transform.DOMoveY(Camera.main.transform.position.y - temp.y, 1f).SetId("MoveY");
       //  }
       //  else {
       //      touching = false;
       //  }

     }
     void touchEnd(Vector2 screenPosition)
     {
         //CheckClick
         if ((screenPosition - beginPos).magnitude < 50)
         {
             GameObject selectPlanet = ObjectClicked(screenPosition);
             if (selectPlanet != null)
             {
                 selectPlanet.GetComponent<_Planet>().Select();
                 sceneChanging = true;
                 StartCoroutine(ChangeToScene(selectPlanet.GetComponent<_Planet>().id, selectPlanet));
             }
         }

     }
     IEnumerator ChangeToScene(int id, GameObject selectPlanet)
     {
         //Khacs manf ddang chon
         yield return new WaitForSeconds(0.2f);
         if (id != PlayerPrefs.GetInt("PlayingPlanet", -1))
         {
             if (PlayerPrefs.GetInt("PlayingPlanet", -1) == -1)
             {
                 // trả camera về vị trí spaceship
                 Tween resetCamera = Camera.main.transform.DOMove(new Vector3(SpaceShip.position.x,
                                                                              SpaceShip.position.y + Camera.main.orthographicSize / 2,
                                                                             -10f), 1.2f).SetEase(Ease.InOutExpo);
                 yield return resetCamera.WaitForCompletion();
                 // Scale lại spaceship
                 Tween apear = SpaceShip.DOScale(1f, 1f).SetEase(Ease.OutElastic);
                 yield return apear.WaitForCompletion();
             }
             else
             {
                 // trả camera về vị trí spaceship
                 Tween resetCamera = Camera.main.transform.DOMove(new Vector3(PlanetContainer.GetChild(PlayerPrefs.GetInt("PlayingPlanet")).position.x,
                                                                          PlanetContainer.GetChild(PlayerPrefs.GetInt("PlayingPlanet")).position.y,
                                                                          -10f), 1.2f).SetEase(Ease.InOutExpo);
                 yield return resetCamera.WaitForCompletion();
                 // Chuyển hướng 
                 Tween prepare = SpaceShip.DORotate(new Vector3(0, 0,
                                                                Vector2.SignedAngle(Vector2.up, PlanetContainer.GetChild(id).position - SpaceShip.position)), 1f);
                 // Bay lên thêm 1 đoạn trước khi chạy animation
                 SpaceShip.DOLocalMove(new Vector3(0, 2f, 0), 1f);
                 yield return prepare.WaitForCompletion();
             }
             SpaceShip.SetParent(null);
             SpaceShip.GetComponentInChildren<Animator>().SetBool("Flying", true);
             // Move camera tới hành tinh được chọn
             Tween moveCamera = Camera.main.transform.DOMove(new Vector3(PlanetContainer.GetChild(id).position.x,
                                                                          PlanetContainer.GetChild(id).position.y,
                                                                          -10f), 1.1f).SetEase(Ease.InOutBack);
             SpaceShip.DOMove(new Vector3(PlanetContainer.GetChild(id).position.x,
                                          PlanetContainer.GetChild(id).position.y,
                                          0), 1f).SetEase(Ease.InOutQuad);
             yield return new WaitForSeconds(1f);
             SpaceShip.GetComponentInChildren<Animator>().SetBool("Flying", false);
             // Dừng bay
             yield return moveCamera.WaitForCompletion();


         }
         else
         {
             Tween resetCamera = Camera.main.transform.DOMove(new Vector3(PlanetContainer.GetChild(id).position.x,
                                                                         PlanetContainer.GetChild(id).position.y,
                                                                         -10f), 1f).SetEase(Ease.InOutExpo);
             yield return resetCamera.WaitForCompletion();
         }
         PlayerPrefs.SetInt("PlayingPlanet", id);
         if (id <= complete)
         {
             SelectLevelManager.setPlanetID(id);
             Initiate.Fade("SelectLevel", new Color(1, 1, 1), 5.0f);
         }
         else
         {

             _setSpaceshipPosition();
             sceneChanging = false;
         }

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

     private void FixedUpdate()
     {
         if (!sceneChanging)
         {
             if (touching)
             {
                 touchHold(Input.mousePosition);
             }
             if (Input.GetMouseButtonDown(0) && !touching)
             {
                 touching = true;
                 touchBegin(Input.mousePosition);
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
             //CheckLimitScroll
             if (Camera.main.transform.position.y > PlanetContainer.GetChild(PlanetContainer.childCount - 1).position.y)
             {
                 Camera.main.transform.DOMoveY(PlanetContainer.GetChild(PlanetContainer.childCount - 1).position.y, 0.2f).SetEase(Ease.InOutSine);
             }
             else if (Camera.main.transform.position.y < PlanetContainer.GetChild(0).position.y)
             {
                 Camera.main.transform.DOMoveY(PlanetContainer.GetChild(0).position.y, 0.2f).SetEase(Ease.InOutSine);
             }
             //Camera di chuyển theo phương X
             for (int i = 0; i < PlanetContainer.childCount - 1; i++)
             {
                 if (Camera.main.transform.position.y > PlanetContainer.GetChild(i).position.y && Camera.main.transform.position.y < PlanetContainer.GetChild(i + 1).position.y)
                 {
                     float ratio = (Camera.main.transform.position.y - PlanetContainer.GetChild(i).position.y) / (PlanetContainer.GetChild(i + 1).position.y - PlanetContainer.GetChild(i).position.y);
                     if (DOTween.TweensById("MoveX", true) != null)
                     {
                         foreach (Tween tween in DOTween.TweensById("MoveX", true))
                         {
                             tween.Kill();
                         }
                     }
                     Camera.main.transform.DOMoveX(PlanetContainer.GetChild(i).position.x + ratio * (PlanetContainer.GetChild(i + 1).position.x - PlanetContainer.GetChild(i).position.x), 0.5f).SetId("MoveX");
                     //Camera.main.transform.position = new Vector3(PlanetContainer.GetChild(i).position.x+ratio*(PlanetContainer.GetChild(i + 1).position.x - PlanetContainer.GetChild(i).position.x), Camera.main.transform.position.y, Camera.main.transform.position.z);
                     break;
                 }
             }
         }
     }*/
    void touchBegin(Vector2 screenPosition)
    {
        beginPos = Camera.main.ScreenToWorldPoint(screenPosition); ;
    }
    void touchHold(Vector2 screenPosition)
    {
        ScrollVelocity = Camera.main.ScreenToWorldPoint(screenPosition);
        ScrollVelocity = ScrollVelocity - beginPos;
    }
    void touchEnd(Vector2 screenPosition)
    {
        //CheckClick
        if ((screenPosition - (Vector2)Camera.main.WorldToScreenPoint(beginPos)).magnitude < 50)
        {
            GameObject selectPlanet = ObjectClicked(screenPosition);
            if (selectPlanet != null)
            {
                selectPlanet.GetComponent<_Planet>().Select();
                sceneChanging = true;
                StartCoroutine(ChangeToScene(selectPlanet.GetComponent<_Planet>().id, selectPlanet));
            }
        }
        else
        {

        }
    }
    IEnumerator ChangeToScene(int id, GameObject selectPlanet)
    {
        //Khacs manf ddang chon
        yield return new WaitForSeconds(0.2f);
        if (id != PlayerPrefs.GetInt("PlayingPlanet", -1))
        {
            //Lần đầu vào game
            if (PlayerPrefs.GetInt("PlayingPlanet", -1) == -1)
            {
                Tween resetCamera = Camera.main.transform.DOMove(new Vector3(SpaceShip.position.x,
                                                                             SpaceShip.position.y + Camera.main.orthographicSize / 2,
                                                                            -10f), 1.5f).SetEase(Ease.InOutExpo);
                yield return resetCamera.WaitForCompletion();
                Tween apear = SpaceShip.DOScale(1f, 1f).SetEase(Ease.OutElastic);
                yield return apear.WaitForCompletion();
            }
            else
            {
                Tween resetCamera = Camera.main.transform.DOMove(new Vector3(PlanetContainer.GetChild(PlayerPrefs.GetInt("PlayingPlanet")).position.x,
                                                                         PlanetContainer.GetChild(PlayerPrefs.GetInt("PlayingPlanet")).position.y,
                                                                         -10f), 1.5f).SetEase(Ease.InOutExpo);
                yield return resetCamera.WaitForCompletion();
                Tween prepare = SpaceShip.DORotate(new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, PlanetContainer.GetChild(id).position - SpaceShip.position)), 0.6f);
                SpaceShip.DOLocalMove(new Vector3(0, 2f, 0), 0.6f);
                yield return prepare.WaitForCompletion();
            }
            SpaceShip.SetParent(null);
            SpaceShip.GetComponentInChildren<Animator>().SetBool("Flying", true);
            Tween moveCamera = Camera.main.transform.DOMove(new Vector3(PlanetContainer.GetChild(id).position.x,
                                                                         PlanetContainer.GetChild(id).position.y,
                                                                         -10f), 1.8f).SetEase(Ease.InOutBack);
            SpaceShip.DOMove(new Vector3(PlanetContainer.GetChild(id).position.x,
                                         PlanetContainer.GetChild(id).position.y,
                                         0), 1.5f).SetEase(Ease.InOutCubic);
            yield return new WaitForSeconds(1.7f);
            SpaceShip.GetComponentInChildren<Animator>().SetBool("Flying", false);
            yield return moveCamera.WaitForCompletion();


        }
        else
        {
            Tween resetCamera = Camera.main.transform.DOMove(new Vector3(PlanetContainer.GetChild(id).position.x,
                                                                        PlanetContainer.GetChild(id).position.y,
                                                                        -10f), 2f).SetEase(Ease.InOutExpo);
            yield return resetCamera.WaitForCompletion();
        }
        PlayerPrefs.SetInt("PlayingPlanet", id);
        if (id <= complete)
        {
            SelectLevelManager.setPlanetID(id);
            Initiate.Fade("SelectLevel", new Color(1, 1, 1), 5.0f);
        }
        else
        {

            _setSpaceshipPosition();
            sceneChanging = false;
        }

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

    private void FixedUpdate()
    {
        if (!sceneChanging)
        {
            if (touching)
            {
                touchHold(Input.mousePosition);
            }
            if (Input.GetMouseButtonDown(0) && !touching)
            {
                touching = true;
                touchBegin(Input.mousePosition);
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
                    touching = true;
                    touchBegin(Input.GetTouch(0).position);
                }
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    touchHold(Input.GetTouch(0).position);
                }
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    touching = false;
                    touchEnd(Input.GetTouch(0).position);
                }
            }

            //Camera di chuyển theo phương X
            for (int i = 0; i < PlanetContainer.childCount - 1; i++)
            {
                if (Camera.main.transform.position.y > PlanetContainer.GetChild(i).position.y && Camera.main.transform.position.y < PlanetContainer.GetChild(i + 1).position.y)
                {
                    float ratio = (Camera.main.transform.position.y - PlanetContainer.GetChild(i).position.y) / (PlanetContainer.GetChild(i + 1).position.y - PlanetContainer.GetChild(i).position.y);
                    if (DOTween.TweensById("MoveX", true) != null)
                    {
                        foreach (Tween tween in DOTween.TweensById("MoveX", true))
                        {
                            tween.Kill();
                        }
                    }
                    Camera.main.transform.DOMoveX(PlanetContainer.GetChild(i).position.x + ratio * (PlanetContainer.GetChild(i + 1).position.x - PlanetContainer.GetChild(i).position.x), 0.2f).SetId("MoveX");
                    //Camera.main.transform.position = new Vector3(PlanetContainer.GetChild(i).position.x+ratio*(PlanetContainer.GetChild(i + 1).position.x - PlanetContainer.GetChild(i).position.x), Camera.main.transform.position.y, Camera.main.transform.position.z);
                    break;
                }
            }
            //Camera di chuyển theo phuong Y
            if (DOTween.TweensById("MoveY", true) != null)
            {
                foreach (Tween tween in DOTween.TweensById("MoveY", true))
                {
                    tween.Kill();
                }
            }
            Camera.main.transform.DOMoveY(Camera.main.transform.position.y - ScrollVelocity.y, 0.1f).SetId("MoveY");
            if (!touching)
                ScrollVelocity = Vector2.Lerp(ScrollVelocity, Vector2.zero, 0.125f);
            //CheckLimitScroll
            if (Camera.main.transform.position.y > PlanetContainer.GetChild(PlanetContainer.childCount - 1).position.y)
            {
                Camera.main.transform.DOMoveY(PlanetContainer.GetChild(PlanetContainer.childCount - 1).position.y, 0.1f);
            }
            else if (Camera.main.transform.position.y < PlanetContainer.GetChild(0).position.y)
            {
                Camera.main.transform.DOMoveY(PlanetContainer.GetChild(0).position.y, 0.1f);
            }
        }
    }
}
