using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Spaceship : MonoBehaviour {
    public static Spaceship instance;
    private void Awake()
    {

        if (instance == null) instance = this;
    }

}
