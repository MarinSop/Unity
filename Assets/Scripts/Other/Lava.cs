using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{

    public Vector2 knockback;
    public float damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player") && !collision.GetComponent<PlayerHitManager>().isHit)
        {
            int x = 0;
            if(collision.transform.position.x > transform.position.x)
            {
                x = 1;
            }
            else
            {
                x = -1;
            }
            collision.GetComponent<Rigidbody2D>().velocity *= 0.0f;
            collision.GetComponent<PlayerHitManager>().hit(new Vector2(knockback.x * x,knockback.y), damage);
        }
    }

}
