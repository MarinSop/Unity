using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public int ID;

    PlayerStats pStats;
    SpriteRenderer currentSprite;
    public Sprite unmarked;
    public Sprite marked;

    bool isMarked = false;
    public float radius;
    public LayerMask playerLayer;


    bool saved = false;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player") && !saved)
        {
            saved = true;
            pStats = collision.GetComponent<PlayerStats>();
            if (!isMarked)
            {
                isMarked = true;
                PlayerPrefs.SetInt("checkpoint" + ID, 1);
                currentSprite.sprite = marked;
            }
            PlayerPrefs.SetFloat("pPosX", transform.position.x);
            PlayerPrefs.SetFloat("pPosY", transform.position.y);
            PlayerPrefs.SetFloat("pHealth", pStats.maxHealth);
            PlayerPrefs.SetFloat("Coins", collision.GetComponent<PlayerSaver>().coins.GetComponent<Coins>().coinAmount);
            saveInventory(collision.gameObject);
            for(int i = 0; i < collision.GetComponent<PlayerSaver>().openedChests.Count;++i)
            {
                PlayerPrefs.SetInt("Chest" + collision.GetComponent<PlayerSaver>().openedChests[i].GetComponent<Chest>().ID, 1);
            }
            collision.GetComponent<PlayerSaver>().openedChests.Clear();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && saved)
        {
            saved = false;
        }
    }


    void Start()
    {
        currentSprite = GetComponent<SpriteRenderer>();
        if(PlayerPrefs.GetInt("checkpoint" + ID,0) > 0)
        {
            isMarked = true;
            currentSprite.sprite = marked;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    void saveInventory(GameObject player)
    {
        Inventory pInventory = player.GetComponent<Inventory>();
        for(int i = 0; i < pInventory.slots.Length-1; ++i)
        {
            if(!pInventory.slots[i].GetComponent<Slot>().isEmpty && pInventory.slots[i].GetComponent<Slot>())
            { 
                PlayerPrefs.SetInt("Slot" + i, pInventory.slots[i].transform.GetChild(0).GetComponent<Item>().ID);
            }
            else if(pInventory.slots[i].GetComponent<Slot>().isEmpty && pInventory.slots[i].GetComponent<Slot>())
            {
                PlayerPrefs.SetInt("Slot" + i, 0);
            }
        }
        for(int i = 0; i < pInventory.armorSlotAmount; ++i)
        {
            if (!pInventory.armorSlots[i].GetComponent<Slot>().isEmpty && pInventory.armorSlots[i].GetComponent<Slot>())
            {
                PlayerPrefs.SetInt("aSlot" + i, pInventory.armorSlots[i].transform.GetChild(0).GetComponent<Item>().ID);
            }
            else if (pInventory.armorSlots[i].GetComponent<Slot>().isEmpty && pInventory.armorSlots[i].GetComponent<Slot>())
            {
                PlayerPrefs.SetInt("aSlot" + i, 0);
            }
        }
    }


}
