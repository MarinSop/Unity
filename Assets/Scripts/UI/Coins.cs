using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coins : MonoBehaviour
{


    TextMeshProUGUI coinsDisplay;
    public float coinAmount = 0;
    public LayerMask coinLayer;
    public float pickupRange;
    public Transform pickupPos;

    // Start is called before the first frame update
    void Start()
    {
        coinsDisplay = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        coinsDisplay.text = coinAmount.ToString();
        Collider2D coin = Physics2D.OverlapCircle(pickupPos.position, pickupRange, coinLayer);
        if (coin)
        {
            if(coin.GetComponent<CoinDrop>().pickupable)
            {
                FindObjectOfType<AudioManager>().Play("Pickup");
                coinAmount += coin.GetComponent<CoinDrop>().coinValue;
                Destroy(coin.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(pickupPos.position, pickupRange);
    }
}
