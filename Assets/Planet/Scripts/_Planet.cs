using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Planet : MonoBehaviour {
    public int id;
    public Transform SpaceShipPosition;
    bool Selected = false;
    public void Select()
    {
        Selected = true;
        transform.DOScale(0.97f*transform.localScale,0.2f).From().OnComplete(SelectComplete);
    }
    void SelectComplete()
    {
        Selected = false;
    }
	void FixedUpdate () {
        if (!Selected)
        {
            float distaneToCamera = ((Vector2)transform.position - (Vector2)Camera.main.transform.position).magnitude;
            if (distaneToCamera < Camera.main.orthographicSize)
            {
                float size = 0.5f + 0.5f * (Camera.main.orthographicSize - distaneToCamera) / Camera.main.orthographicSize;
                transform.localScale = new Vector3(size, size);
            }
            else
            {
                transform.localScale = new Vector3(0.5f, 0.5f);
            }
        }
	}
}
