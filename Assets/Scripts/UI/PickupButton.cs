using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupButton : MonoBehaviour
{
    public GameObject pickupButton;
    public float pickupRange;
    Inventory inventory;
    public LayerMask pickupLayer;

    // Start is called before the first frame update
    private void Start()
    {
        inventory = GetComponent<Inventory>();
    }
    // Update is called once per frame
    void Update()
    {
        Collider2D pickup = Physics2D.OverlapCircle(transform.position, pickupRange, pickupLayer);

        if (pickup)
        {
            //pickupButton.SetActive(true);
            pickupButton.transform.localScale = new Vector2(1, 1);
            if (SimpleInput.GetButtonDown("Pickup"))
            {
                if (pickup.GetComponent<Pickup>())
                {
                    for (int i = 0; i < inventory.slotAmount; ++i)
                    {
                        if (inventory.slots[i].GetComponent<Slot>().isEmpty == true)
                        {
                            inventory.addItem(pickup.GetComponent<Pickup>().item, i);
                            Destroy(pickup.gameObject);
                            break;
                        }
                    }

                }
                else if(pickup.GetComponent<Chest>())
                {
                    transform.root.GetComponent<PlayerSaver>().openedChests.Add(pickup.gameObject);
                    pickup.GetComponent<Chest>().open();
                }
                else if(pickup.GetComponent<Door>())
                {
                    pickup.GetComponent<Door>().loadScene();
                }
            }
        }
        else
        {
            pickupButton.transform.localScale = new Vector2(0, 0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}


