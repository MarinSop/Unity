using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    bool isInventoryEnabled;

    public GameObject slotHolder;
    public GameObject armorHolder;
    public GameObject[] slots;
    public GameObject[] armorSlots;
    public int slotAmount;
    public int armorSlotAmount;
    public GameObject inventory;
    public GameObject statsPanel;
    public GameObject backToMenuButton;

    //Inventory drag and drop system
    EventSystem eventSystem;
    public Canvas canvas;
    GraphicRaycaster graphicsRaycaster;
    PointerEventData pointerEventData;
    public GameObject ItemHolder;
    GameObject heldItem;
    GameObject itemSlot;
    bool isHoldingItem = false;
    bool isHolding = false;

    public float holdWaitTime;
    float currentHoldWaitTime;
    bool holdTimerStart = false;

    public TextMeshProUGUI damageTxt;
    public TextMeshProUGUI defenseTxt;
    public TextMeshProUGUI knockbackTxt;

    // Start is called before the first frame update
    void Start()
    {
        isInventoryEnabled = inventory.activeSelf;
        slotAmount = slotHolder.transform.childCount;
        armorSlotAmount = armorHolder.transform.childCount;
        slots = new GameObject[slotAmount];
        armorSlots = new GameObject[armorSlotAmount];
        for (int i = 0; i < slotAmount; ++i)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < armorSlotAmount; ++i)
        {
            armorSlots[i] = armorHolder.transform.GetChild(i).gameObject;
        }

        graphicsRaycaster = canvas.GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
        currentHoldWaitTime = holdWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (SimpleInput.GetButtonDown("Inventory"))
        {
            if (inventory.activeInHierarchy == false)
            {
                inventory.SetActive(true);
                isInventoryEnabled = true;
                backToMenuButton.SetActive(true);
            }
            else
            {
                inventory.SetActive(false);
                isInventoryEnabled = false;
                backToMenuButton.SetActive(false);
            }
        }
        if (isInventoryEnabled)
        {
            itemDragging();
        }
    }


    public void addItem(GameObject item, int slotNum)
    {
        item.GetComponent<Item>().prevSlot = slots[slotNum];
        Instantiate(item, slots[slotNum].transform, false);
        slots[slotNum].GetComponent<Slot>().isEmpty = false;
    }

    void itemDragging()
    {
        if (holdTimerStart)
        {
            currentHoldWaitTime -= Time.deltaTime;
            if (currentHoldWaitTime <= 0.0f)
            {
                Debug.Log("holding");
                isHolding = true;
                holdTimerStart = false;
            }
        }
        if (isHolding)
        {
            if (!isHoldingItem)
            {
                if (heldItem)
                {
                    pointerEventData = new PointerEventData(eventSystem);
                    pointerEventData.position = Input.mousePosition;
                    List<RaycastResult> results = new List<RaycastResult>();
                    graphicsRaycaster.Raycast(pointerEventData, results);
                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject.GetComponent<Slot>())
                        {
                            if (result.gameObject == itemSlot && result.gameObject.transform.GetChild(0).gameObject == heldItem)
                            {

                                heldItem.transform.SetParent(ItemHolder.transform);
                                heldItem.GetComponent<Item>().isHeld = true;
                                heldItem.GetComponent<Item>().prevSlot = itemSlot;
                                itemSlot.GetComponent<Slot>().isEmpty = true;
                                isHoldingItem = true;
                                break;
                            }
                        }
                        else
                        {
                            isHolding = false;
                        }
                    }
                }
            }
        }


        if (Input.GetButtonDown("Fire1"))
        {
            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            graphicsRaycaster.Raycast(pointerEventData, results);
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.GetComponent<Slot>())
                {
                    if (!result.gameObject.GetComponent<Slot>().isEmpty)
                    {
                        holdTimerStart = true;
                        itemSlot = result.gameObject;
                        heldItem = itemSlot.transform.GetChild(0).gameObject;
                    }
                }
            }

        }

        if (Input.GetButtonUp("Fire1"))
        {

            if (isHoldingItem)
            {
                heldItem.GetComponent<Item>().isHeld = false;
                isHoldingItem = false;
                pointerEventData = new PointerEventData(eventSystem);
                pointerEventData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                graphicsRaycaster.Raycast(pointerEventData, results);
                bool drop = false;
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.CompareTag("Slot"))
                    {
                        if (result.gameObject.GetComponent<Slot>().isEmpty == true)
                        {
                            if (result.gameObject.GetComponent<Slot>().isItemValid(heldItem))
                            {
                                heldItem.transform.SetParent(result.gameObject.transform);
                                heldItem.transform.localPosition *= 0.0F;
                                result.gameObject.GetComponent<Slot>().isEmpty = false;
                                heldItem.GetComponent<Item>().prevSlot = result.gameObject;
                                drop = false;
                            }
                            else
                            {
                                heldItem.GetComponent<Item>().returnToPrevSlot();
                                drop = false;
                            }
                            break;
                        }
                        if (result.gameObject.GetComponent<Slot>().isEmpty == false)
                        {
                            heldItem.GetComponent<Item>().returnToPrevSlot();
                            drop = false;
                            break;
                        }
                    }
                    else if (result.gameObject.CompareTag("Inventory"))
                    {
                        heldItem.GetComponent<Item>().returnToPrevSlot();
                        drop = false;
                        break;
                    }
                    else if (!result.gameObject.CompareTag("Inventory") && !result.gameObject.CompareTag("Slot"))
                    {
                        drop = true;
                    }
                }
                if (drop == true)
                {
                    Instantiate(heldItem.GetComponent<Item>().itemDrop, transform.position, Quaternion.Euler(0, 0, 0));
                    Destroy(heldItem);
                    drop = false;
                }

            }
            if (holdTimerStart && !isHolding)
            {
                statsPanel.SetActive(true);
                Vector3 offset = new Vector3(-100.0f, 0.0f, 0.0f);
                statsPanel.transform.position = Input.mousePosition + offset;
                float damage = heldItem.GetComponent<Item>().equipment.GetComponent<ItemStats>().damage;
                float defense = heldItem.GetComponent<Item>().equipment.GetComponent<ItemStats>().defense;
                float knockback = heldItem.GetComponent<Item>().equipment.GetComponent<ItemStats>().knockback;
                damageTxt.text = damage.ToString();
                defenseTxt.text = defense.ToString();
                knockbackTxt.text = knockback.ToString();
            }
            else
            {
                statsPanel.SetActive(false);
            }


            isHolding = false;
            holdTimerStart = false;
            currentHoldWaitTime = holdWaitTime;


        }
    }

    public void backToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}

