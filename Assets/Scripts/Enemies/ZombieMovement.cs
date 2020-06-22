using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    EnemyStats stats;

    Animator anim;
    public float maxSpeed;
    public float aSpeed;
    public float dSpeed;
    public float followRadius;
    Transform player;
    public LayerMask playerMask;
    float dampedVelocity;
    public LayerMask raycastingLayers;
    Vector2 velocity;
    Rigidbody2D rb;
    float x = 0;
    bool deaccelerate = true;


    public Transform attackPos;
    public float attackRange;
    public float attackLenght;
    bool isAttacking = false;
    bool hitSucessful = false;
    public float attackAnimationTime;
    float currentAttackAnimationTime;
    bool attackCooldown = false;
    public float attackCooldownTime;
    float currentAttackCooldownTime;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentAttackAnimationTime = attackAnimationTime;
        currentAttackCooldownTime = attackCooldownTime;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        stats = GetComponent<EnemyStats>();
        Vector2 heading = player.position - transform.position;
        float distance = heading.magnitude;
        Vector2 direction = heading / distance;
        if (GetComponent<EnemyHitManager>().isHit)
        {
            isAttacking = false;
            currentAttackAnimationTime = attackAnimationTime;
            hitSucessful = false;
            attackCooldown = false;
        }
        if (distance <= followRadius && distance > attackRange && !GetComponent<EnemyHitManager>().isHit && !isAttacking)
        {
            movement(distance, direction);
        }
        else
        {
            deaccelerate = true;
        }
        if (distance <= attackRange && !isAttacking && !attackCooldown)
        {
            isAttacking = true;
        }
        if (deaccelerate)
        {
            rb.velocity = new Vector2(Mathf.SmoothDamp(velocity.x, 0, ref dampedVelocity, dSpeed),rb.velocity.y);
        }
        attack(distance);

        anim.SetBool("attacking", isAttacking);
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        anim.SetBool("stunned", GetComponent<EnemyHitManager>().isHit);

    }

    void attack(float distance)
    {
        if(isAttacking && !attackCooldown)
        {
            velocity.x = 0;
            currentAttackAnimationTime -= Time.deltaTime;
            if(currentAttackAnimationTime <= 0.40f && !hitSucessful)
            {
                Collider2D playerHit = Physics2D.OverlapCircle(attackPos.position, attackLenght, playerMask);
                if(playerHit)
                {
                    if (transform.localRotation.y > 0)
                    {
                        playerHit.GetComponent<PlayerHitManager>().hit(stats.knockback, stats.damage);
                    }
                    else
                    {
                        playerHit.GetComponent<PlayerHitManager>().hit(stats.knockback * -1, stats.damage);
                    }
                    hitSucessful = true;
                }

            }
            if(currentAttackAnimationTime <= 0)
            {
                currentAttackAnimationTime = attackAnimationTime;
                isAttacking = false;
                hitSucessful = false;
                attackCooldown = true;
            }
        }
        if(attackCooldown)
        {
            currentAttackCooldownTime -= Time.deltaTime;
            if(currentAttackCooldownTime <= 0.0f)
            {
                attackCooldown = false;
                currentAttackCooldownTime = attackCooldownTime;
            }
        }
    }

    void movement(float distance,Vector2 direction)
    {
            if (direction.x > 0)
            {
                x = 1;
                transform.localRotation = Quaternion.Euler(0, -180, 0);
            }
            else if (direction.x < 0)
            {
                x = -1;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance + 5.0f, raycastingLayers);
            if (hit && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                deaccelerate = false;
                rb.velocity = new Vector2(Mathf.SmoothDamp(velocity.x, x * maxSpeed, ref dampedVelocity, aSpeed),rb.velocity.y);
            }
            else
            {
                deaccelerate = true;
            }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackLenght);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
