using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitManager : MonoBehaviour
{
    EnemyStats enemyStats;
    public bool isHit = false;
    Rigidbody2D rb;
    public float stunTime;
    float currentStunTime;
    [HideInInspector]
    public int hitAmount = 0;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyStats = GetComponent<EnemyStats>();
        currentStunTime = stunTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(isHit)
        {
            currentStunTime -= Time.deltaTime;
            if(currentStunTime <= 0.0f)
            {
                currentStunTime = stunTime;
                isHit = false;
            }
        }
    }


    public void hit(Vector2 knockback,float damage)
    {
        isHit = true;
        currentStunTime = stunTime;
        rb.AddForce(knockback, ForceMode2D.Impulse);
        enemyStats.takeDamage(damage);
        hitAmount += 1;
    }


    public void resetStun()
    {
        isHit = false;
        currentStunTime = stunTime;
    }
}
