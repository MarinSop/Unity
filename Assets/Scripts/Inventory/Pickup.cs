using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public GameObject item;
    public LayerMask ground;

    private void Start()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, transform.up * -1,10.0f, ground);
        if(hit)
        {
            transform.position = new Vector2(hit.point.x, hit.point.y + (transform.localScale.y / 2.0f) );
        }
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(0, 0,scale.z);
        LeanTween.scaleX(gameObject, scale.x, 0.5f);
        LeanTween.scaleY(gameObject, scale.y, 0.5f);
        LeanTween.moveY(gameObject,transform.position.y + 1.0f, 1.0f).setLoopPingPong();
        
    }

    private void Update()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * -1);
    }
}

