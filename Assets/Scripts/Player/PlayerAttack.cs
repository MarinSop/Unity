using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerHitManager hit;
    public Transform longSwordAttackPos;
    public Transform greatSwordAttackPos;
    public float attackRange;
    public LayerMask enemyLayer;
    PlayerStats stats;
    public bool attacking = false;
    bool hitSuccesful = false;

    public float longswordAttackCost;
    public float greatswordAttackCost;
    //public float piercingswordAttackCost;

    float firstAttackTime = 0.0f;
    float secondAttackTime = 0.0f;

    float gFirstAttackTime = 0.0f;
    float gSecondAttackTime= 0.0f;

    //float pFirstAttackTime = 0.0f;
    //float pSecondAttackTime = 0.0f;


    bool firstAttack = false;
    bool secondAttack = false;
    bool resetAttack = false;
    bool useFirstStamina = false;
    bool useSecondStamina = false;

    PlayerAnimationManager anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<PlayerAnimationManager>();
        hit = GetComponent<PlayerHitManager>();
    }

    // Update is called once per frame
    void Update()
    {
        stats = GetComponent<PlayerStats>();
        if (!hit.isHit)
        {
            if (checkStamina())
            {
                if (SimpleInput.GetButtonDown("Attack"))
                {
                    if (firstAttack && !secondAttack)
                    {
                        secondAttack = true;
                        useSecondStamina = true;
                        attacking = true;

                    }
                    if (secondAttack && !resetAttack && !firstAttack)
                    {
                        resetAttack = true;
                        useFirstStamina = true;
                        attacking = true;
                    }
                    if (!attacking)
                    {
                        attacking = true;
                        useFirstStamina = true;
                        firstAttack = true;
                    }
                }
            }

            if (anim.weaponType == "longsword")
            {
                longSwordAttack(stats);
            }
            else if (anim.weaponType == "greatsword")
            {
                greatSwordAttack(stats);
            }
            //else if(anim.weaponType == "piercingsword")
            //{
            //    piercingSwordAttack(stats);
            //}
        }
        else
        {
            attacking = false;
            hitSuccesful = false;
            firstAttack = false;
            secondAttack = false;
            resetAttack = false;
            firstAttackTime = 0.0f;
            gFirstAttackTime = 0.0f;
            secondAttackTime = 0.0f;
            gSecondAttackTime = 0.0f;
            //pFirstAttackTime = 0.0f;
            //pSecondAttackTime = 0.0f;
            stats.canRefill = true;
        }
        if(!firstAttack)
        {
            anim.secondAttack = secondAttack;
        }
        anim.firstAttack = firstAttack;
    }

    void longSwordAttack(PlayerStats stats)
    {
        if (attacking)
        {
            if (firstAttack)
            {
                firstAttackTime += Time.deltaTime;
                if (firstAttackTime >= 0.33f && firstAttackTime <= 0.5f && !hitSuccesful)
                {
                    if (useFirstStamina)
                    {
                        stats.useStamina(longswordAttackCost);
                        stats.canRefill = false;
                        useFirstStamina = false;
                    }
                    Collider2D hit = Physics2D.OverlapCircle(longSwordAttackPos.position, attackRange, enemyLayer);
                    if (hit)
                    {
                        hitSuccesful = true;
                        if (!hit.GetComponent<EnemyHitManager>())
                        {
                            return;
                        }
                        if (transform.localRotation.y < 0)
                        {
                            hit.GetComponent<EnemyHitManager>().hit(stats.knockback * -1, stats.damage);
                        }
                        else
                        {
                            hit.GetComponent<EnemyHitManager>().hit(stats.knockback, stats.damage);
                        }
                    }
                }
                if (firstAttackTime >= 0.66f)
                {
                    if (!secondAttack)
                    {
                        attacking = false;
                        stats.canRefill = true;
                    }
                    firstAttack = false;
                    hitSuccesful = false;
                    firstAttackTime = 0.0f;
                }

            }
            if (secondAttack && !firstAttack)
            {
                    if (useSecondStamina)
                    {
                        stats.useStamina(longswordAttackCost);
                        stats.canRefill = false;
                        useSecondStamina = false;
                    }
                secondAttackTime += Time.deltaTime;
                if (secondAttackTime >= 0.16f && secondAttackTime <= 0.5f && !hitSuccesful)
                {
                    Debug.Log("mid");
                    Collider2D hit = Physics2D.OverlapCircle(longSwordAttackPos.position, attackRange, enemyLayer);
                    if (hit)
                    {
                        Debug.Log("attack");
                        hitSuccesful = true;
                        if (!hit.GetComponent<EnemyHitManager>())
                        {
                            return;
                        }
                        if (transform.localRotation.y < 0)
                        {
                            hit.GetComponent<EnemyHitManager>().hit(stats.knockback * -1, stats.damage);
                        }
                        else
                        {
                            hit.GetComponent<EnemyHitManager>().hit(stats.knockback, stats.damage);
                        }
                    }
                }
                if (secondAttackTime >= 0.66f)
                {
                    Debug.Log("");
                    secondAttackTime = 0.0f;
                    attacking = false;
                    hitSuccesful = false;
                    secondAttack = false;
                    stats.canRefill = true;
                    if (resetAttack)
                    {
                        attacking = true;
                        resetAttack = false;
                        firstAttack = true;
                        anim.secondAttack = secondAttack;
                    }
                }
            }
        }


    }

    void greatSwordAttack(PlayerStats stats)
    {
        if (attacking)
        {
            if (firstAttack)
            {
                    if (useFirstStamina)
                    {
                        stats.useStamina(greatswordAttackCost);
                    stats.canRefill = false;
                        useFirstStamina = false;
                    }
                gFirstAttackTime += Time.deltaTime;
                if (gFirstAttackTime >= 0.66f && gFirstAttackTime <= 1.16f && !hitSuccesful)
                {
                    Collider2D hit = Physics2D.OverlapCircle(greatSwordAttackPos.position, attackRange, enemyLayer);
                    if (hit)
                    {
                        hitSuccesful = true;
                        if (!hit.GetComponent<EnemyHitManager>())
                        {
                            return;
                        }
                        if (transform.localRotation.y < 0)
                        {
                            hit.GetComponent<EnemyHitManager>().hit(stats.knockback * -1, stats.damage);
                        }
                        else
                        {
                            hit.GetComponent<EnemyHitManager>().hit(stats.knockback, stats.damage);
                        }
                    }
                }
                if (gFirstAttackTime >= 1.5f)
                {
                    if (!secondAttack)
                    {
                        stats.canRefill = true;
                        attacking = false;
                    }
                    firstAttack = false;
                    hitSuccesful = false;
                    gFirstAttackTime = 0.0f;
                }

            }
            if (secondAttack && !firstAttack)
            {
                    if (useSecondStamina)
                    {
                        stats.useStamina(greatswordAttackCost);
                    stats.canRefill = false;
                        useSecondStamina = false;
                    }
                gSecondAttackTime += Time.deltaTime;
                if (gSecondAttackTime >= 0.66f && gSecondAttackTime <= 1.16f && !hitSuccesful)
                {
                    Collider2D hit = Physics2D.OverlapCircle(greatSwordAttackPos.position, attackRange, enemyLayer);
                    if (hit)
                    {
                        hitSuccesful = true;
                        if (!hit.GetComponent<EnemyHitManager>())
                        {
                            return;
                        }
                        if (transform.localRotation.y < 0)
                        {
                            hit.GetComponent<EnemyHitManager>().hit(stats.knockback * -1, stats.damage);
                        }
                        else
                        {
                            hit.GetComponent<EnemyHitManager>().hit(stats.knockback, stats.damage);
                        }
                    }
                }
                if (gSecondAttackTime >= 1.5f)
                {
                    gSecondAttackTime = 0.0f;
                    attacking = false;
                    secondAttack = false;
                    hitSuccesful = false;
                    stats.canRefill = true;
                    if (resetAttack)
                    {
                        attacking = true;
                        resetAttack = false;
                        firstAttack = true;
                        anim.secondAttack = secondAttack;
                    }
                }
            }
        }
        
    }

    //void piercingSwordAttack(PlayerStats stats)
    //{
    //    if (attacking)
    //    {
    //        if (firstAttack)
    //        {
    //                if (useFirstStamina)
    //                {
    //                    stats.useStamina(piercingswordAttackCost);
    //                    useFirstStamina = false;
    //                }
    //            pFirstAttackTime += Time.deltaTime;
    //            if (pFirstAttackTime >= 0.16f && pFirstAttackTime <= 0.416f && !hitSuccesful)
    //            {
    //                Collider2D hit = Physics2D.OverlapCircle(longSwordAttackPos.position, attackRange, enemyLayer);
    //                if (hit)
    //                {
    //                    hitSuccesful = true;
    //                    if (!hit.GetComponent<EnemyHitManager>())
    //                    {
    //                        return;
    //                    }
    //                    if (transform.localRotation.y < 0)
    //                    {
    //                        hit.GetComponent<EnemyHitManager>().hit(stats.knockback * -1, stats.damage);
    //                    }
    //                    else
    //                    {
    //                        hit.GetComponent<EnemyHitManager>().hit(stats.knockback, stats.damage);
    //                    }
    //                }
    //            }
    //            if (pFirstAttackTime >= 0.5f)
    //            {
    //                if (!secondAttack)
    //                {
    //                    attacking = false;
    //                }
    //                firstAttack = false;
    //                hitSuccesful = false;
    //                pFirstAttackTime = 0.0f;
    //            }

    //        }
    //        if (secondAttack && !firstAttack)
    //        {
    //                if (useSecondStamina)
    //                {
    //                    stats.useStamina(piercingswordAttackCost);
    //                    useSecondStamina = false;
    //                }
    //            pSecondAttackTime += Time.deltaTime;
    //            if (pSecondAttackTime >= 0.16f && pSecondAttackTime <= 0.416f && !hitSuccesful)
    //            {
    //                Collider2D hit = Physics2D.OverlapCircle(longSwordAttackPos.position, attackRange, enemyLayer);
    //                if (hit)
    //                {
    //                    hitSuccesful = true;
    //                    if (!hit.GetComponent<EnemyHitManager>())
    //                    {
    //                        return;
    //                    }
    //                    if (transform.localRotation.y < 0)
    //                    {
    //                        hit.GetComponent<EnemyHitManager>().hit(stats.knockback * -1, stats.damage);
    //                    }
    //                    else
    //                    {
    //                        hit.GetComponent<EnemyHitManager>().hit(stats.knockback, stats.damage);
    //                    }
    //                }
    //            }
    //            if (pSecondAttackTime >= 0.5f)
    //            {
    //                pSecondAttackTime = 0.0f;
    //                attacking = false;
    //                hitSuccesful = false;
    //                secondAttack = false;
    //                if (resetAttack)
    //                {
    //                    attacking = true;
    //                    resetAttack = false;
    //                    firstAttack = true;
    //                    anim.secondAttack = secondAttack;
    //                }
    //            }
    //        }
    //    }
    //}

    bool checkStamina()
    {
        if(transform.GetComponent<PlayerStats>().getCurrentStamina() > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
        //if (anim.weaponType == "longsword" && transform.GetComponent<PlayerStats>().getCurrentStamina() >= longswordAttackCost)
        //{
        //    return true;
        //}
        //else if (anim.weaponType == "greatsword" && transform.GetComponent<PlayerStats>().getCurrentStamina() >= greatswordAttackCost)
        //{
        //    return true;
        //}
        //else if (anim.weaponType == "piercingsword" && transform.GetComponent<PlayerStats>().getCurrentStamina() >= piercingswordAttackCost)
        //{
        //    return true;
        //}
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(greatSwordAttackPos.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(longSwordAttackPos.position, attackRange);

    }
}
