using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    public float maxHealth;
    public float damage;
    public float defense;
    public Vector2 knockback;
    public Slider slider;
    public GameObject canvas;

    public float currentHealth;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        dead();
        healthBar();
    }

    public void takeDamage(float damageGiven)
    {
        float damageTaken = damageGiven * (100 / (100 + defense));
        currentHealth -= damageTaken;
    }

    void healthBar()
    {
        slider.value = currentHealth / maxHealth;
        if(currentHealth < maxHealth)
        {
            canvas.SetActive(true);
        }
    }


    void dead()
    {
        if(currentHealth <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
