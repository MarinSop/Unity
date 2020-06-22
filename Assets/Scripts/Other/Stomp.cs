using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomp : MonoBehaviour
{

    ParticleSystem particles;
    Rigidbody2D rb;
    public float timeTillDestroy;
    public float speed;
    float dampedVel;

    [HideInInspector]
    public float damage;
    [HideInInspector]
    public float knockback;
    [HideInInspector]
    public float x;
    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
        if(x < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, -180, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.GetComponent<PlayerHitManager>().hit(new Vector2(knockback, 0.0f), damage);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, speed * x, ref dampedVel, 0.1f),rb.velocity.y);
        Destroy(gameObject, timeTillDestroy);
    }
}
