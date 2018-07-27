using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpaceship : MonoBehaviour {
    float speed = 1f;
    private void Update()
    {
        Vector3 temp = Camera.main.transform.position;
        if (Input.GetKey("w"))
        {
            temp.y += speed;
            Camera.main.transform.position = temp;
        }
        if (Input.GetKey("s")) {
            temp.y -= speed;
            Camera.main.transform.position = temp;
        }
    }
}
