using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    public GameObject holder;
    GameObject item;
    bool equiped = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!equiped)
        {
            if (transform.childCount == 1)
            {
                if (transform.GetChild(0).GetComponent<Item>().isHelmet || transform.GetChild(0).GetComponent<Item>().isBody ||
                transform.GetChild(0).GetComponent<Item>().isLegs || transform.GetChild(0).GetComponent<Item>().isWeapons)
                {
                    Instantiate(transform.GetChild(0).GetComponent<Item>().equipment, holder.transform,true);
                    item = holder.transform.GetChild(0).gameObject;
                    item.transform.localPosition *= 0.0f;
                    item.transform.localScale = new Vector3(1, 1, 1);
                    if(transform.GetChild(0).GetComponent<Item>().isWeapons || transform.GetChild(0).GetComponent<Item>().isHelmet)
                    {
                        item.transform.localScale = new Vector3(2, 2, 1);
                    }
                    item.transform.localRotation = holder.transform.localRotation;
                    
                    equiped = true;
                }
                
            }

        }
        if(transform.childCount == 0)
        {
            if(holder.transform.childCount == 1)
            {
                if (holder.transform.root.GetComponent<PlayerStats>())
                {
                    if (item.GetComponent<ItemStats>())
                    {
                        ItemStats itemStats = item.GetComponent<ItemStats>();
                        holder.transform.root.GetComponent<PlayerStats>().removeStats(itemStats.damage, itemStats.defense, itemStats.knockback);
                    }
                }
                Destroy(holder.transform.GetChild(0).gameObject);
                item = null;
                equiped = false;
            }
        }
    }
}
