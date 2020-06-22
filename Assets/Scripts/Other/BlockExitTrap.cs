using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockExitTrap : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !trapActive)
        {
            trapActive = true;
            barrier.SetActive(true);
        }
    }

    public GameObject[] enemy;
    GameObject barrier;

    bool trapActive = false;

    // Start is called before the first frame update
    void Start()
    {
        barrier = transform.GetChild(0).gameObject;
        barrier.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(trapActive)
        {
            bool selfDestruct = false;
            foreach(GameObject en in enemy)
            {
                if(en != null)
                {
                    selfDestruct = false;
                    break;
                }
                else if(en == null)
                {
                    selfDestruct = true;
                }
            }
            if(selfDestruct)
            {
                Destroy(gameObject);
            }
        }
    }
}
