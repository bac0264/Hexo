using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tri : MonoBehaviour {
    public int Num;
    public int Direction;
    public Vector2 Pos;
    [SerializeField]
    private SpriteRenderer HexSprite;
    // Use this for initialization
    void Start () {
        float HexHeight = HexSprite.bounds.size.y * GameSetting.hexOffset;
        Vector2 TrianglePosition = (Quaternion.Euler(0, 0, (60 * Direction)) * Vector2.up) * (HexHeight / 2) * 0.785f;
        transform.eulerAngles = new Vector3(0, 0, (60 * Direction));
        transform.localPosition = new Vector3(0,0,0.1f);
        transform.DOLocalMove(new Vector3(TrianglePosition.x, TrianglePosition.y, 0.1f), 0.5f).SetEase(Ease.OutCirc);
        transform.GetChild(0).eulerAngles = Vector3.zero;
        Refresh();
    }
    public void Refresh()
    {
        GetComponentInChildren<TextMesh>().text = Num.ToString();
    }
}
