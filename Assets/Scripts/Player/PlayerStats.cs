using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Slider healthSlider;
    public Slider staminaSlider;
    public float maxHealth;
    public float maxStamina;
    public float damage;
    public float defense;
    public Vector2 knockback;

    public float currentHealth;
    public float currentStamina;

    [HideInInspector]
    public bool staminaCooldown = false;
    [HideInInspector]
    public bool canRefill = true;
    public float staminaCooldownTime;
    float currentStaminaCooldownTime;
    public float staminaRefillSpeed;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentStaminaCooldownTime = staminaCooldownTime;
    }

    // Update is called once per frame
    void Update()
    {
        staminaRefill();
        updateBars();
    }

    void updateBars()
    {
        healthSlider.value = currentHealth / maxHealth;
        staminaSlider.value = currentStamina / maxStamina;
    }

    public void useStamina(float amount)
    {
        currentStamina -= amount;
        currentStaminaCooldownTime = staminaCooldownTime;
        staminaCooldown = true;
    }

    public float getCurrentStamina()
    {
        return currentStamina;
    }

    void staminaRefill()
    {
        if(currentStamina < maxStamina && !staminaCooldown && canRefill)
        {
            currentStamina += staminaRefillSpeed * Time.deltaTime;
        }
        if(currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
        if(staminaCooldown && canRefill)
        {
            currentStaminaCooldownTime -= Time.deltaTime;
            if(currentStaminaCooldownTime <= 0.0f)
            {
                staminaCooldown = false;
                currentStaminaCooldownTime = staminaCooldownTime;
            }
        }
    }

    public void addStats(float itemDamage, float itemDefense, float itemKnockback)
    {
        knockback.x += itemKnockback;
        damage += itemDamage;
        defense += itemDefense;
    }

    public void removeStats(float itemDamage, float itemDefense, float itemKnockback)
    {
        knockback.x -= itemKnockback;
        damage -= itemDamage;
        defense -= itemDefense;
    }


    public void takeDamage(float damageGiven)
    {
        float damageTaken = damageGiven * (100 / (100 + defense));
        currentHealth -= damageTaken;
    }
}
