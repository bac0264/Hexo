using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Planet : MonoBehaviour {
    public int PlanetID;
    public void _Move()
    {
        // Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Spaceship.instance.gameObject.transform.position.y + 5, Camera.main.transform.position.z);
        Camera.main.transform.DOMove(Spaceship.instance.transform.position, 1.0f).SetEase(Ease.InOutQuad);
        StartCoroutine(TimeToDelay());
    }
    IEnumerator TimeToDelay()
    {
        yield return new WaitForSeconds(1.2f);
        Spaceship.instance.transform.DOMove(transform.position, 1f).SetEase(Ease.InOutQuad);
    }
}
