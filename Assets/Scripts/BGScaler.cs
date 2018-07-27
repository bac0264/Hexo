using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScaler : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        float screenHeight = Camera.main.orthographicSize * 2f;
        float bgHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        float screenWidth = screenHeight * Screen.width / Screen.height;
        float bgWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        transform.localScale = new Vector3(screenWidth / bgWidth, screenHeight / bgHeight);
    }
}
