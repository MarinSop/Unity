using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public GameObject itemDrop;
    public GameObject equipment;
    [HideInInspector]
    public GameObject prevSlot;
    public bool isHelmet;
    public bool isBody;
    public bool isLegs;
    public bool isWeapons;
    public bool isItem;

    public bool isHeld = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isHeld)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    public void returnToPrevSlot()
    {
        transform.SetParent(prevSlot.transform);
        prevSlot.GetComponent<Slot>().isEmpty = false;
        transform.localPosition *= 0.0f;
    }
}
