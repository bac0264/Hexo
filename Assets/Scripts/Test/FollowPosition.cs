using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FollowPosition : MonoBehaviour
{
    public float parralax = 2f;
    private void Update()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        Material mat = mr.material;
        Vector2 offset = mat.mainTextureOffset;
        offset.y = transform.position.y/transform.localScale.y/parralax;
        offset.x = transform.position.x / transform.localScale.x / parralax;
        mat.mainTextureOffset = offset;
    }
}
