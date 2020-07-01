using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    Animator playerAnimator;
    Animator weaponAnimator;
    Animator helmetAnimator;
    Animator bodyAnimator;
    Animator legsAnimator;

    public Transform helmetHolder;
    public Transform bodyHolder;
    public Transform legsHolder;
    public Transform weaponHolder;

    [HideInInspector]
    public bool firstAttack = false;
    [HideInInspector]
    public bool secondAttack = false;
    [HideInInspector]
    public bool jumping = false;
    [HideInInspector]
    public bool stunned = false;
    [HideInInspector]
    public float runningVelocity;
    [HideInInspector]
    public bool isDodging = false;

    public string weaponType;



    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        weaponAnimator = null;
        helmetAnimator = null;
        bodyAnimator = null;
        legsAnimator = null;

    }

    // Update is called once per frame
    void Update()
    {
        loadAnimatorIfAvailable();
        updateAnimations();

    }


    void updateAnimations()
    {
        playerAnimator.SetFloat("speed", runningVelocity);
        if (weaponType == "longsword")
        {
            playerAnimator.SetBool("attacking", firstAttack);
            playerAnimator.SetBool("attacking2", secondAttack);
        }
        else if (weaponType == "greatsword")
        {
            playerAnimator.SetBool("attackinggreatsword", firstAttack);
            playerAnimator.SetBool("attackinggreatsword2", secondAttack);
        }
        //else if(weaponType == "piercingsword")
        //{
        //    playerAnimator.SetBool("attackingpiercingsword", firstAttack);
        //    playerAnimator.SetBool("attackingpiercingsword2", secondAttack);
        //}
        playerAnimator.SetBool("jumping", jumping);
        playerAnimator.SetBool("stunned", stunned);
        playerAnimator.SetBool("dodging", isDodging);
        if(weaponAnimator)
        {
            weaponAnimator.SetBool("attacking", firstAttack);
            weaponAnimator.SetBool("attacking2", secondAttack);
            if (weaponType == "longsword" || weaponType == "piercingsword")
            {
                 weaponAnimator.SetBool("stunned", stunned);
                weaponAnimator.SetBool("jumping", jumping);
                weaponAnimator.SetFloat("speed", runningVelocity);
                weaponAnimator.SetBool("dodging", isDodging);
            }
        }
        if (helmetAnimator)
        {
            if (weaponType == "longsword")
            {
                helmetAnimator.SetBool("greatswordattack", false);
                helmetAnimator.SetBool("longswordattack", firstAttack);
                if (!firstAttack)
                {
                    helmetAnimator.SetBool("longswordattack2", secondAttack);
                }
            }
            else if (weaponType == "greatsword")
            {
                helmetAnimator.SetBool("longswordattack", false);
                helmetAnimator.SetBool("greatswordattack", firstAttack);
                if (!firstAttack)
                {
                    helmetAnimator.SetBool("greatswordattack2", secondAttack);
                }
            }
            helmetAnimator.SetBool("dodging", isDodging);
        }
        if (bodyAnimator)
        {
            if (weaponType == "greatsword")
            {
                bodyAnimator.SetBool("attackinggreatsword", firstAttack);
                bodyAnimator.SetBool("attackinggreatsword2", secondAttack);
            }
            else if (weaponType == "longsword")
            {
                bodyAnimator.SetBool("attacking", firstAttack);
                bodyAnimator.SetBool("attacking2", secondAttack);
            }
                bodyAnimator.SetFloat("speed", runningVelocity);
                bodyAnimator.SetBool("jumping", jumping);
                bodyAnimator.SetBool("dodging", isDodging);
        }
        if (legsAnimator)
        {
            legsAnimator.SetFloat("speed", runningVelocity);
            if(weaponType == "longsword")
            legsAnimator.SetBool("attacking", firstAttack);
            else if(weaponType == "greatsword")
                legsAnimator.SetBool("greatswordattacking", firstAttack);
            legsAnimator.SetBool("attacking2", secondAttack);

            legsAnimator.SetBool("jumping", jumping);
            legsAnimator.SetBool("dodging", isDodging);
        }
    }
    void loadAnimatorIfAvailable()
    {
        if (weaponHolder.childCount == 1)
        {
            weaponAnimator = weaponHolder.GetChild(0).GetComponent<Animator>();
            if (weaponHolder.GetChild(0).GetComponent<ItemStats>().weaponType == "longsword")
            {
                if(bodyAnimator)
                {
                    bodyAnimator.SetBool("idle2", true);
                }
                weaponType = weaponHolder.GetChild(0).GetComponent<ItemStats>().weaponType;
                playerAnimator.SetBool("idle2", true);

            }
            else if (weaponHolder.GetChild(0).GetComponent<ItemStats>().weaponType == "greatsword")
            {
                if (bodyAnimator)
                {
                    bodyAnimator.SetBool("idle3", true);
                }
                weaponType = weaponHolder.GetChild(0).GetComponent<ItemStats>().weaponType;
                playerAnimator.SetBool("idle3", true);
            }
            else if (weaponHolder.GetChild(0).GetComponent<ItemStats>().weaponType == "piercingsword")
            {
                weaponType = weaponHolder.GetChild(0).GetComponent<ItemStats>().weaponType;
            }
        }
        else
        {
            if (bodyAnimator)
            {
                bodyAnimator.SetBool("idle2", false);
                bodyAnimator.SetBool("idle3", false);
            }
            weaponAnimator = null;
            playerAnimator.SetBool("idle2", false);
            playerAnimator.SetBool("idle3", false);
            weaponType = null;
        }
        if (bodyHolder.childCount == 1)
        {
            bodyAnimator = bodyHolder.GetChild(0).GetComponent<Animator>();
        }
        else
        {
            bodyAnimator = null;
        }

        if (helmetHolder.childCount == 1)
        {
            helmetAnimator = helmetHolder.GetChild(0).GetComponent<Animator>();
        }
        else
        {
            helmetAnimator = null;
        }
        if (legsHolder.childCount == 1)
        {
            legsAnimator = legsHolder.GetChild(0).GetComponent<Animator>();
        }
        else
        {
            legsAnimator = null;
        }

    }

}
