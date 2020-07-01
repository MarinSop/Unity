using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitManager : MonoBehaviour
{

    PlayerStats stats;
    Rigidbody2D rb;
    public bool isHit = false;
    public ParticleSystem particles;

    


    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void hit(Vector2 knockback,float damage)
    {
        if (!GetComponent<PlayerMovement>().isDodging)
        {
            particles.Play();
            isHit = true;
            stats.takeDamage(damage);
            rb.AddForce(knockback, ForceMode2D.Impulse);
            FindObjectOfType<AudioManager>().Play("Hurt");
        }
    }
}
