using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSwap : MonoBehaviour
{

    GameObject active;
    GameObject secondary;
    int id;
    public float swappingSpeed;
    bool swapping = false;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).localPosition.x == 0.0f)
            {
                active = transform.GetChild(i).gameObject;
            }
            else
            {
                secondary = transform.GetChild(i).gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(SimpleInput.GetMouseButtonDown(5) && !swapping)
        {
            for(int i = 0; i < transform.childCount;++i)
            {
                if(transform.GetChild(i).localPosition.x == 0.0f)
                {
                    secondary = transform.GetChild(i).gameObject;
                }
                else
                {
                    active = transform.GetChild(i).gameObject;
                }
            }
            active.transform.localPosition = new Vector2(-100.0f, 0.0f);
            LeanTween.moveLocalX(secondary, 100, swappingSpeed);
            id = LeanTween.moveLocalX(active, 0.0f, swappingSpeed).id;
            swapping = true;
        }
        else if(SimpleInput.GetMouseButtonDown(4) && !swapping)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                if (transform.GetChild(i).localPosition.x == 0.0f)
                {
                    secondary = transform.GetChild(i).gameObject;
                }
                else
                {
                    active = transform.GetChild(i).gameObject;
                }
            }
            active.transform.localPosition = new Vector2(100.0f, 0.0f);
            LeanTween.moveLocalX(secondary, -100, swappingSpeed);
            id = LeanTween.moveLocalX(active, 0.0f, swappingSpeed).id;
            swapping = true;
        }
        else if(SimpleInput.GetMouseButtonUp(6) && !swapping)
        {
            Debug.Log("stisak");
            active.GetComponent<Potion>().use();
        }

        if(swapping)
        {
            if(!LeanTween.isTweening(id))
            {
                swapping = false;
            }
        }
    }
}
