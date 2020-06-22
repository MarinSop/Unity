using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurGuardBow : MonoBehaviour
{
    Rigidbody2D rb;
    public Transform legs;
    public Transform body;
    Animator legsAnim;
    Animator bodyAnim;
    public float shootingRange;
    Transform player;
    public LayerMask hittableLayers;
    float x = 1;

    bool shooting = false;
    bool arrowShot = false;
    float shootingTimer = 0.0f;
    bool shootingCooldown = false;
    public float shootingCooldownTime;
    float currentShootingCooldown;

    public GameObject arrow;
    public Transform arrowSpawnPos;
    public float angle;
    float y = 0;

    bool dodging = false;
    bool readyToDodge = false;
    public float dodgeTriggerRadius;
    public float dodgeSpeed;
    float dampedVel;
    public float acclSpeed;
    public float dodgeTime;
    float currentDodgeTime;
    bool dodgeCooldown = false;
    public float dodgeCooldownTime;
    float currentDodgeCooldownTime;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        legsAnim = legs.GetComponent<Animator>();
        bodyAnim = body.GetComponent<Animator>();
        currentShootingCooldown = shootingCooldownTime;
        currentDodgeCooldownTime = 0.0f;
        currentDodgeTime = 0.0f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 heading = player.position - transform.position;
        float distance = heading.magnitude;
        Vector2 direction = heading / distance;
        if(distance <= shootingRange && !GetComponent<EnemyHitManager>().isHit)
        {
                angle = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
            if (direction.x > 0)
            {
                y = -180;
                angle *= -1;
                transform.localRotation = Quaternion.Euler(0, y, 0);
                body.localRotation = Quaternion.Euler(0.0f, body.localRotation.y, angle);
                x = 1;
            }
            else if (direction.x < 0)
            {
                y = 0;
                angle += 180;
                transform.localRotation = Quaternion.Euler(0, y, 0);
                x = -1;
                body.localRotation = Quaternion.Euler(0.0f, body.localRotation.y, angle);
            }
            RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, distance + 5.0f, hittableLayers);
            if(ray.collider.gameObject.layer == LayerMask.NameToLayer("Player") && !shooting && !shootingCooldown)
            {
                shooting = true;
                readyToDodge = true;
            }
            else
            {
                readyToDodge = false;
            }
            if(distance <= dodgeTriggerRadius && !dodging && !dodgeCooldown && readyToDodge)
            {
                dodging = true;
            }

        }
        else if (GetComponent<EnemyHitManager>().isHit)
        {
            dodging = false;
            shooting = false;
            currentDodgeCooldownTime = 0.0f;
            currentShootingCooldown = shootingCooldownTime;
            shootingTimer = 0.0f;
            currentDodgeTime = 0.0f;
            shootingCooldown = false;
            dodgeCooldown = false;
        }
        dodge();
        shoot(direction);
        legsAnim.SetBool("stunned", GetComponent<EnemyHitManager>().isHit);
        legsAnim.SetBool("dodging", dodging);
        bodyAnim.SetBool("stunned", GetComponent<EnemyHitManager>().isHit);
        bodyAnim.SetBool("shooting", shooting);

    }

    void shoot(Vector2 direction)
    {
        if(shooting && !shootingCooldown)
        {
            shootingTimer += Time.deltaTime;
            if (shootingTimer >= 0.666f && !arrowShot)
            {
                GameObject tempArrow = arrow;
                tempArrow.GetComponent<Arrow>().damage = GetComponent<EnemyStats>().damage;
                tempArrow.GetComponent<Arrow>().knockback = GetComponent<EnemyStats>().knockback.x;
                arrowShot = true;
                //shoot arrow
                Instantiate(arrow, arrowSpawnPos.position, Quaternion.Euler(0, y, angle));
            }
            else if (shootingTimer >= 1.5f)
            {
                arrowShot = false;
                shooting = false;
                shootingCooldown = true;
                shootingTimer = 0;

            }
        }
        if(shootingCooldown)
        {
            currentShootingCooldown -= Time.deltaTime;
            if(currentShootingCooldown <= 0.0f)
            {
                currentShootingCooldown = shootingCooldownTime;
                shootingCooldown = false;
            }
        }
    }


    void dodge()
    {
        if(dodging)
        {
            rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, dodgeSpeed * x * -1, ref dampedVel, acclSpeed), rb.velocity.y);
            currentDodgeTime += Time.deltaTime;
            if(currentDodgeTime >= dodgeTime)
            {
                dodging = false;
                currentDodgeTime = 0.0f;
                dodgeCooldown = true;
            }
        }
        if(dodgeCooldown)
        {
            currentDodgeCooldownTime += Time.deltaTime;
            if(currentDodgeCooldownTime >= dodgeCooldownTime)
            {
                dodgeCooldown = false;
                currentDodgeCooldownTime = 0.0f;
                readyToDodge = false;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, dodgeTriggerRadius);
    }
}
