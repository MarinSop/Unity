using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantMinotaurBoss : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform trigger;
    public GameObject stompAttackObj;
    public Transform stompAttackPos;
    Animator anim;
    Rigidbody2D rb;
    GameObject player;
    int x = 0;
    float dampedVel;

    bool following = false;
    bool playerInArea = false;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public LayerMask checkpointLayer;
    public float speed;
    public float acl;
    public float hornAttackSpeed;
    public float dodgeSpeed;
    public bool hornAttacking = false;

    int randomAttack = 0;

    bool attacking = false;
    public Transform attackPos;
    float attackTime = 0.0f;
    bool hitSuccesful = false;

    public Transform leftCheckpoint;
    public Transform rightCheckpoint;

    bool hornHitSuccesful = false;
    bool reposition = false;
    bool readyToAttack = false;
    Transform closerCheckpoint;
    Transform otherCheckpoint;

    bool attackCooldown = false;
    public float attackCooldownTime;
    float currentAttackCooldownTime;

    public bool stomping = false;
    float stompAttackTime = 0.0f;
    int stompNum = 1;




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentAttackCooldownTime = attackCooldownTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapBox(trigger.position, trigger.localScale, 0, playerLayer) && !playerInArea)
        {
            playerInArea = true;
            following = true;
        }
        if (following && !attacking && !hornAttacking && !stomping)
        {
            Vector2 heading = player.transform.position - transform.position;
            float distance = heading.magnitude;
            Vector2 direction = heading / distance;
            if (player.transform.position.x <= transform.position.x)
            {
                x = -1;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                x = 1;
                transform.localRotation = Quaternion.Euler(0, -180, 0);
            }
            if (attackCooldown) { x *= -1; }
            rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, speed * x, ref dampedVel, acl), rb.velocity.y);
            if (Physics2D.OverlapBox(attackPos.position, attackPos.localScale, 0, playerLayer) && !attacking && !attackCooldown)
            {
                attacking = true;
            }

        }
        attack();
        hornAttack();
        stompAttack();
        attackCooldownFun();
        if (attacking || hornAttacking ||stomping)
        {
            anim.SetFloat("speed", 0);
        }
        else
        {
            anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        }
        if (!reposition)
        {
            anim.SetBool("hornattacking", hornAttacking);
            anim.SetBool("stomping", stomping);
        }
        else
        {
            anim.SetBool("hornattacking", false);
            anim.SetBool("stomping", false);
        }
        anim.SetBool("attacking", attacking);
        anim.SetBool("dodging", reposition);
    }


    void attack()
    {
        if (attacking && !hornAttacking && !attackCooldown)
        {
            attackTime += Time.deltaTime;
            if (attackTime >= 0.333f && attackTime <= 0.666f && !hitSuccesful)
            {
                Collider2D player = Physics2D.OverlapBox(attackPos.position, attackPos.localScale, 0, playerLayer);
                if (player)
                {
                    hitSuccesful = true;
                    player.GetComponent<PlayerHitManager>().hit(GetComponent<EnemyStats>().knockback * x, GetComponent<EnemyStats>().damage);
                }
            }
            if (attackTime >= 1.5f && attackTime < 2.0f && !hornHitSuccesful)
            {
                rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, hornAttackSpeed * x, ref dampedVel, 0.001f), rb.velocity.y);
                Collider2D player = Physics2D.OverlapBox(attackPos.position, attackPos.localScale, 0, playerLayer);
                if (player && hitSuccesful)
                {
                    hornHitSuccesful = true;
                    player.GetComponent<PlayerHitManager>().hit(GetComponent<EnemyStats>().knockback * x, GetComponent<EnemyStats>().damage);
                }
            }
            if (attackTime >= 2.0f || hornHitSuccesful)
            {
                attackCooldown = true;
                attackCooldownTime = 1.0f;
                currentAttackCooldownTime = attackCooldownTime;
                randomAttack = Random.Range(0, 30);
                attackTime = 0.0f;
                attacking = false;
                hornHitSuccesful = false;
            }
        }
    }


    void attackCooldownFun()
    {
        if (attackCooldown)
        {
            currentAttackCooldownTime -= Time.deltaTime;
            if (currentAttackCooldownTime <= 0.0f || Physics2D.OverlapBox(transform.position, transform.localScale, 0.0f, checkpointLayer))
            {
                attackCooldown = false;
                currentAttackCooldownTime = attackCooldownTime;
                if(randomAttack > 10 && randomAttack < 20)
                {
                    hornAttacking = true;
                }
                else if( randomAttack >= 20)
                {
                    stomping = true;
                }
            }
        }
    }

    void hornAttack()
    {
        if (hornAttacking && !attackCooldown)
        {
            if (!readyToAttack && !reposition)
            {
                if (closerCheckpoint == null || !Physics2D.OverlapBox(closerCheckpoint.position, closerCheckpoint.localScale, 0, enemyLayer))
                {
                    reposition = true;
                }
            }
            repositionFun();
            if (!reposition && readyToAttack)
            {
                rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, hornAttackSpeed * x, ref dampedVel, acl), rb.velocity.y);
                if (Physics2D.OverlapBox(otherCheckpoint.position, otherCheckpoint.localScale, 0, enemyLayer))
                {
                    hornAttacking = false;
                    readyToAttack = false;
                    attackCooldown = true;
                    attackCooldownTime = 3.0f;
                    randomAttack = Random.Range(0, 25);
                }
                Collider2D playerCol = Physics2D.OverlapBox(attackPos.position, attackPos.localScale, 0, playerLayer);
                if (Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy")) == false)
                {
                    if (playerCol && !hornHitSuccesful)
                    {
                        hornAttacking = false;
                        readyToAttack = false;
                        attackCooldown = true;
                        attackCooldownTime = 3.0f;
                        currentAttackCooldownTime = attackCooldownTime;
                        randomAttack = Random.Range(0, 25);
                        player.GetComponent<PlayerHitManager>().hit(GetComponent<EnemyStats>().knockback * x, GetComponent<EnemyStats>().damage);
                    }
                }
            }
        }
    }


    void stompAttack()
    {
        if(stomping && !attackCooldown)
        {
            if (!readyToAttack && !reposition)
            {
                if (closerCheckpoint == null || !Physics2D.OverlapBox(closerCheckpoint.position, closerCheckpoint.localScale, 0, enemyLayer))
                {
                    reposition = true;
                }
            }
            repositionFun();
            if(!reposition && readyToAttack)
            {
                GameObject tempAtt = stompAttackObj;
                tempAtt.GetComponent<Stomp>().knockback = GetComponent<EnemyStats>().knockback.x;
                tempAtt.GetComponent<Stomp>().damage = GetComponent<EnemyStats>().damage;
                tempAtt.GetComponent<Stomp>().x = x;
                stompAttackTime += Time.deltaTime;
                if(stompAttackTime >= 1.0f && stompAttackTime < 2.0f && stompNum == 1)
                {
                    Instantiate(tempAtt, stompAttackPos.position,Quaternion.Euler(0,0,0));
                    ++stompNum;
                }
                else if(stompAttackTime >= 2.0f && stompAttackTime < 3.0f && stompNum == 2)
                {
                    Instantiate(tempAtt, stompAttackPos.position, Quaternion.Euler(0, 0, 0)); ++stompNum;
                }
                else if (stompAttackTime >= 3.0f && stompAttackTime < 3.5f && stompNum == 3)
                {
                    Instantiate(tempAtt, stompAttackPos.position, Quaternion.Euler(0, 0, 0)); ++stompNum;
                }
                else if(stompAttackTime >= 3.5f)
                {
                    stomping = false;
                    stompNum = 1;
                    readyToAttack = false;
                    stompAttackTime = 0.0f;
                }
            }
        }
    }


    void rotateBoss()
    {
        if (player.transform.position.x <= transform.position.x)
        {
            x = -1;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            x = 1;
            transform.localRotation = Quaternion.Euler(0, -180, 0);
        }
    }


    void repositionFun()
    {
        if (reposition)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
            if (Mathf.Abs(transform.position.x - leftCheckpoint.position.x) < Mathf.Abs(transform.position.x - rightCheckpoint.position.x))
            {
                transform.localRotation = Quaternion.Euler(0, -180, 0);
                closerCheckpoint = leftCheckpoint;
                otherCheckpoint = rightCheckpoint;
                rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, dodgeSpeed * -1, ref dampedVel, acl), rb.velocity.y);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                closerCheckpoint = rightCheckpoint;
                otherCheckpoint = leftCheckpoint;
                rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, dodgeSpeed, ref dampedVel, acl), rb.velocity.y);
            }
            if (Physics2D.OverlapBox(closerCheckpoint.position, closerCheckpoint.localScale, 0, enemyLayer))
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
                rb.velocity = new Vector2(0.0f, rb.velocity.y);
                reposition = false;
                readyToAttack = true;
                rotateBoss();
            }
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(trigger.position, trigger.localScale);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, attackPos.localScale);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(leftCheckpoint.position, leftCheckpoint.localScale);
        Gizmos.DrawWireCube(rightCheckpoint.position, rightCheckpoint.localScale);
    }
}
