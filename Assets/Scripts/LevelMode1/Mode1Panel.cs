using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode1Panel : MonoBehaviour
{
    public Mode1Slot[] slotList;

    private void OnValidate()
    {
        if(slotList.Length == 0)
        {
            slotList = GetComponentsInChildren<Mode1Slot>();
        }
    }
    private void Start()
    {
        for (int i = 0; i < slotList.Length; i++)
        {
            slotList[i].ID = i;
            slotList[i].Setup();
        }
    }
}
