using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    public int ID;
    public ParticleSystem particles;
    public GameObject[] allLoot;
    bool startedPlaying = false;

    private void Start()
    {
        if(PlayerPrefs.GetInt("Chest" + ID,0) > 0)
        {
            gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if(startedPlaying)
        {
            if(!particles.isPlaying)
            {
                gameObject.SetActive(false);
            }
        }
    }
    public void open()
    {
        foreach (GameObject loot in allLoot)
        {
            Instantiate(loot, new Vector2(transform.position.x + Random.Range(-2.0f,2.0f),transform.position.y), Quaternion.Euler(0, 0, 0));
        }
        particles.Play();
        startedPlaying = true;
    }


}
