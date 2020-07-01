using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaver : MonoBehaviour
{
    PlayerStats playerStats;
    Inventory inventory;
    public GameObject[] allItems;
    public List<GameObject> openedChests;
    public GameObject coins;
    // Start is called before the first frame update

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        inventory = GetComponent<Inventory>();
        if (GameObject.FindGameObjectWithTag("Checkpoint"))
        {
            transform.position = new Vector2(PlayerPrefs.GetFloat("pPosX", transform.position.x), PlayerPrefs.GetFloat("pPosY", transform.position.y));
        }
        playerStats.currentHealth = PlayerPrefs.GetFloat("pHealth", playerStats.maxHealth);
        coins.GetComponent<Coins>().coinAmount = PlayerPrefs.GetFloat("Coins", 0);
        for(int i = 0; i < inventory.slotAmount; ++i)
        {
            for (int j = 0; j < allItems.Length; ++j)
            {
                if(allItems[j].GetComponent<Item>().ID == PlayerPrefs.GetInt("Slot" + i,0))
                {
                    Instantiate(allItems[j], inventory.slots[i].transform);
                    inventory.slots[i].GetComponent<Slot>().isEmpty = false;
                }
            }
        }
        for(int i = 0; i < inventory.armorSlotAmount; ++i)
        {
            for (int j = 0; j < allItems.Length; ++j)
            {
                if(allItems[j].GetComponent<Item>().ID == PlayerPrefs.GetInt("aSlot" + i,0))
                {
                    Instantiate(allItems[j], inventory.armorSlots[i].transform);
                    inventory.armorSlots[i].GetComponent<Slot>().isEmpty = false;
                    inventory.armorSlots[i].GetComponent<Equip>().equipArmor();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
