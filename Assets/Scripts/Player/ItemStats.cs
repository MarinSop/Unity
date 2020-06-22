using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats : MonoBehaviour
{
    public float damage;
    public float defense;
    public float knockback;
    public string weaponType;

    // Start is called before the first frame update
    void Start()
    {
        givePlayerStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void givePlayerStats()
    {
        Transform player = transform.root;
        if (!player.GetComponent<PlayerStats>())
        {
            return;
        }
            player.GetComponent<PlayerStats>().addStats(damage,defense,knockback);
        
    }
}
