using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool isEmpty = true;
    public bool isHelmetSlot;
    public bool isBodySlot;
    public bool isLegsSlot;
    public bool isWeaponSlot;
    public bool isInvSlot;


    void Update()
    {
    }

    public bool isItemValid(GameObject item)
    {
        if (isHelmetSlot && item.GetComponent<Item>().isHelmet)
        {
            return true;
        }
        else if (isBodySlot && item.GetComponent<Item>().isBody)
        {
            return true;
        }
        else if (isLegsSlot && item.GetComponent<Item>().isLegs)
        {
            return true;
        }
        else if (isWeaponSlot && item.GetComponent<Item>().isWeapons)
        {
            return true;
        }
        else if(isInvSlot && item.GetComponent<Item>().isItem)
        {
            return true;
        }
        return false;

    }
}
