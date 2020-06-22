using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Potion : MonoBehaviour
{

    public string potionType;
    public int amount;
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if(potionType == "refill")
        {
            amount = 2;
        }
        text.text = amount.ToString();
    }

    public void use()
    {
        if (amount > 0)
        {
            amount -= 1;
            transform.root.GetComponent<PlayerStats>().currentHealth += 40.0f;
        }
        text.text = amount.ToString();
    }
}
