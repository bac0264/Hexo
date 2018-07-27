using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour {
    [Range(0, 300)]public int fps;

	void Update () {
        Application.targetFrameRate = fps;
	}
}
