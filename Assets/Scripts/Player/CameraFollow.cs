using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    Transform offset;
    public float smooth;
    void Start()
    {
        offset = GameObject.FindGameObjectWithTag("CameraOffset").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 target = new Vector3(offset.position.x,offset.position.y, transform.position.z);
        //transform.position = Vector3.SmoothDamp(transform.position, target, ref dampedVelocity, smooth);
        transform.position = Vector3.Lerp(transform.position, target, smooth);
    }
}
