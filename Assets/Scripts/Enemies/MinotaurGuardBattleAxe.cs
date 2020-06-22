using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurGuardBattleAxe : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    public Vector2 followRange;
    public LayerMask findingLayers;
    public float speed;
    public float acc;
    public float deacc;
    EnemyStats stats;
    float x = 0;


    public float attackArea;
    bool attacking = false;
    bool attackCooldown = false;
    float attackLenght = 0.0f;
    public float attackCooldownTime;
    float currentAttackCooldownTime;
    public float attackRange;
    bool hitSucessful = false;
    public Transform attackPos;

    bool headAttacking = false;
    public float headAttackDamage;
    public float headAttackingSpeed;
    public float headAttackingTime;
    float currentHeadAttackingTime;
    public Transform hitDetector;
    public LayerMask playerLayer;
    public LayerMask hittableLayers;


    int dodgeAfterHit = 0;
    public bool dodging = false;
    public float dodgeSpeed;
    public float dodgeTime;
    float currentDodgeTime;


    float dampVel;
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<EnemyStats>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentAttackCooldownTime = attackCooldownTime;
        currentDodgeTime = dodgeTime;
        currentHeadAttackingTime = headAttackingTime;
        dodgeAfterHit = Random.Range(1, 5);

    }

    // Update is called once per frame
    void Update()
    {
        stats = GetComponent<EnemyStats>();
        Collider2D player = Physics2D.OverlapBox(transform.position, followRange, 0, playerLayer);

        if(player && !GetComponent<EnemyHitManager>().isHit && !dodging && !headAttacking)
        {
            Vector2 heading = player.transform.position - transform.position;
            float distance = heading.magnitude;
            Vector2 direction = heading / distance;
            if (!attacking)
            {
                RaycastHit2D playerInSight = Physics2D.Raycast(transform.position, direction, distance + 5.0f, findingLayers);
                if (playerInSight && playerInSight.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    if (transform.position.x > playerInSight.point.x)
                    {
                        x = -1;
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                        rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, speed * x, ref dampVel, acc), rb.velocity.y);
                    }
                    else
                    {
                        x = 1;
                        transform.localRotation = Quaternion.Euler(0, -180, 0);
                        rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, speed * x, ref dampVel, acc), rb.velocity.y);
                    }
                }
                else if (rb.velocity.x != 0.0f && !playerInSight)
                {
                    rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, 0.0f, ref dampVel, deacc), rb.velocity.y);
                }
            }
            attack(distance, direction);
        }
        else if(GetComponent<EnemyHitManager>().isHit || dodging)
        {
            attacking = false;
            attackLenght = 0.0f;
            headAttacking = false;
            currentHeadAttackingTime = headAttackingTime;
        }
        dodge();
        headAttack();
        if(!attacking && !headAttacking && !dodging && !GetComponent<EnemyHitManager>().isHit)
        {
            anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        }
        else
        {
            anim.SetFloat("speed", 0);
        }
        anim.SetBool("stunned", GetComponent<EnemyHitManager>().isHit);
        anim.SetBool("dodging", dodging);
        anim.SetBool("attacking",attacking);
        anim.SetBool("headattacking", headAttacking);
    }


    void attack(float distance, Vector2 direction)
    {
        if (!attackCooldown)
        {
            if (distance < attackArea && !attacking)
            {
                attacking = true;
            }
            if (attacking)
            {
                attackLenght += Time.deltaTime;
                if(!hitSucessful && attackLenght >= 0.5f && attackLenght <= 0.75f)
                {
                    Collider2D hit = Physics2D.OverlapCircle(attackPos.position, attackRange, playerLayer);
                    if(hit)
                    {
                        hitSucessful = true;
                        hit.GetComponent<PlayerHitManager>().hit(new Vector2(stats.knockback.x * x,stats.knockback.y), stats.damage);
                    }
                }
                if (attackLenght >= 1.33f)
                {
                    hitSucessful = false;
                    attacking = false;
                    attackLenght = 0.0f;
                    attackCooldown = true;
                }
            }

        }
        else
        {
            currentAttackCooldownTime -= Time.deltaTime;
            if(currentAttackCooldownTime <= 0.0f)
            {
                attackCooldown = false;
                currentAttackCooldownTime = attackCooldownTime;
            }
        }
    }

    void dodge()
    {
        if (GetComponent<EnemyHitManager>().hitAmount >= dodgeAfterHit && !dodging)
        {
            GetComponent<EnemyHitManager>().hitAmount = 0;
            dodgeAfterHit = Random.Range(1, 5);
            dodging = true;
            attacking = false;
            attackLenght = 0.0f;
            GetComponent<EnemyHitManager>().resetStun();
        }
        if (dodging)
        {
            currentDodgeTime -= Time.deltaTime;
            //rb.velocity = new Vector2((dodgeSpeed * Time.deltaTime)*(x*-1), rb.velocity.y);
            rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, dodgeSpeed * x * -1, ref dampVel, acc),rb.velocity.y);
            if (currentDodgeTime <= 0.0f)
            {
                dodging = false;
                currentDodgeTime = dodgeTime;
                headAttacking = true;
                hitSucessful = false;
            }
        }

    }

    void headAttack()
    {
        if(headAttacking)
        {
            if (currentHeadAttackingTime <= 0.0f)
            {
                headAttacking = false;
                currentHeadAttackingTime = headAttackingTime;
                hitSucessful = false;
                attackCooldown = true;
                return;
            }
            rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, headAttackingSpeed * x, ref dampVel, acc), rb.velocity.y);
            currentHeadAttackingTime -= Time.deltaTime;
            Collider2D hit = Physics2D.OverlapBox(hitDetector.position, hitDetector.localScale, 0, hittableLayers);
            if(hit && !hitSucessful)
            {
                hitSucessful = true;
                if(hit.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    hit.GetComponent<PlayerHitManager>().hit(stats.knockback,stats.damage + headAttackDamage);
                    headAttacking = false;
                    currentHeadAttackingTime = headAttackingTime;
                    attackCooldown = true;
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, followRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackArea);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
        Gizmos.DrawWireCube(hitDetector.position, hitDetector.localScale);
    }
}
