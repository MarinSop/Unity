using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    PlayerAnimationManager anim;
    PlayerHitManager hit;

    bool jump = false;
    Rigidbody2D rb;
    float x = 0;
    public float speed = 100.0f;
    public float jumpHeight = 10.0f;
    public float maxSpeed = 20.0f;
    Vector2 velocity;
   public float deacceleration = 10.0f;
    float dampedDeaccelerationVelocity;
    float dampedAccelerationVelocity;

    public Transform groundCheck;
    public Transform wallCheck;
    bool grounded = false;
    public LayerMask groundLayer;
    public Vector2 groundCheckSize;
    public Vector2 wallCheckSize;

    public float stunTime;
    float currentStunTime;

    public float dodgeSpeed;
    public float dodgingTime;
    public float dodgeCost;
    float currenDodgingTime;
    [HideInInspector]
    public bool isDodging = false;

    bool soundPlayed = false;
    // Start is called before the first frame update


    void Start()
    {
        Invoke("playTheme", 0.1f);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("BossBarrier"),true);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<PlayerAnimationManager>();
        hit = GetComponent<PlayerHitManager>();
        currentStunTime = stunTime;
        currenDodgingTime = dodgingTime;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GetComponent<PlayerDeath>().dead)
        {
            if (!transform.GetComponent<PlayerAttack>().attacking && !hit.isHit && !isDodging)
            {
                x = SimpleInput.GetAxis("Horizontal");
                if (x < 0)
                {
                    transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                }
                else if (x > 0)
                {
                    transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                }

                if (SimpleInput.GetButtonDown("Jump") && grounded)
                {
                    jump = true;
                    FindObjectOfType<AudioManager>().Play("Jump");
                }


                movement();
            }
            else if (hit.isHit || transform.GetComponent<PlayerAttack>().attacking)
            {
                if (hit.isHit)
                {
                    currentStunTime -= Time.deltaTime;
                    if (currentStunTime <= 0)
                    {
                        hit.isHit = false;
                        currentStunTime = stunTime;
                    }
                }
                velocity.x = Mathf.SmoothDamp(velocity.x, 0.0F, ref dampedDeaccelerationVelocity, deacceleration);
            }
            dodge();
        }
        else
        {
            hit.isHit = false;
            currentStunTime = stunTime;
            isDodging = false;
            currenDodgingTime = dodgingTime;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        }
        if (isDodging || GetComponent<PlayerAttack>().attacking || hit.isHit)
        {
            anim.jumping = true;
            anim.runningVelocity = 0;
        }
        else
        {
            anim.runningVelocity = Mathf.Abs(rb.velocity.x);
            anim.jumping = grounded;
        }
        anim.isDodging = isDodging;
        anim.stunned = hit.isHit;
        if(Mathf.Abs(rb.velocity.x) >= 7.0f && !soundPlayed && Mathf.Abs(rb.velocity.x) < 11.0f && grounded)
        {
            soundPlayed = true;
            FindObjectOfType<AudioManager>().Play("Run");
        }
        else if(Mathf.Abs(rb.velocity.x) < 7.0f && soundPlayed || Mathf.Abs(rb.velocity.x) > 11.0f && soundPlayed || !grounded && soundPlayed)
        {
            soundPlayed = false;
            FindObjectOfType<AudioManager>().Stop("Run");
        }
    }
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0,groundLayer);

        if (jump)
        {
            jump = false;
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            rb.AddForce(new Vector2(0.0f,jumpHeight), ForceMode2D.Impulse);
        }
        if(!hit.isHit && !isDodging && !GetComponent<PlayerAttack>().attacking)
        {
            rb.velocity = new Vector2(velocity.x, rb.velocity.y);
        }
        else if(isDodging)
        {
            rb.velocity = new Vector2(x * dodgeSpeed * Time.deltaTime, 0.0f);
        }
    }

    void movement()
    {
        bool wallHit = Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0, groundLayer);

        if (!wallHit && x != 0)
        {
            velocity.x = Mathf.SmoothDamp(velocity.x, x * maxSpeed, ref dampedAccelerationVelocity, speed);
        }
        else
        {
            velocity.x = Mathf.SmoothDamp(velocity.x, 0.0F, ref dampedDeaccelerationVelocity, deacceleration);
        }
    }

    void dodge()
    {
        if (!hit.isHit)
        {
            if (transform.GetComponent<PlayerStats>().getCurrentStamina() > 0)
            {
                if (SimpleInput.GetButtonDown("Dodge") && !hit.isHit && !transform.GetComponent<PlayerAttack>().attacking && !isDodging)
                {
                    isDodging = true;
                    transform.GetComponent<PlayerStats>().useStamina(dodgeCost);
                    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
                    FindObjectOfType<AudioManager>().Play("Dodge");

                }
            }
            if (isDodging)
            {
                currenDodgingTime -= Time.deltaTime;
                if (currenDodgingTime <= 0)
                {
                    isDodging = false;
                    currenDodgingTime = dodgingTime;
                    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);

                }

            }
        }
        else
        {
            isDodging = false;
            currenDodgingTime = dodgingTime;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        }
    }
    void playTheme()
    {
        if(SceneManager.GetActiveScene().name == "Level1")
        {
            FindObjectOfType<AudioManager>().PlayMusic("Level1Theme");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
