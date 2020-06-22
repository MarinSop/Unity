using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDrop : MonoBehaviour
{
    public float coinValue;
    public bool pickupable = false;
    public float timeToPickup;

    private void Start()
    {
        coinValue = Random.Range(1, 15);
        LeanTween.moveY(transform.gameObject, transform.position.y + 0.5f, 1.0f).setLoopPingPong();

    }
    private void Update()
    {
        if (!pickupable)
        {
            timeToPickup -= Time.deltaTime;
            if (timeToPickup <= 0.0f)
            {
                pickupable = true;
            }

        }
    }
}
