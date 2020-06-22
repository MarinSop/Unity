using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    // Start is called before the first frame update


    public bool dead = false;
    public float timeTillRespawn;
    float currentTimeTillRespawn;
    void Start()
    {
        currentTimeTillRespawn = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!dead && GetComponent<PlayerStats>().currentHealth <= 0.0f)
        {
            dead = true;
            
        }
        if(dead)
        {
            //death Animation
            currentTimeTillRespawn += Time.deltaTime;
            if(currentTimeTillRespawn >= timeTillRespawn)
            {
                dead = false;
                //GetComponent<PlayerStats>().currentHealth = GetComponent<PlayerStats>().maxHealth;
                //transform.position = new Vector2(PlayerPrefs.GetFloat("pPosX"), PlayerPrefs.GetFloat("pPosY"));
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
