using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{

    public Slider musicSlider;
    public Slider effectsSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void musicValue()
    {
        FindObjectOfType<AudioManager>().musicVolumeChange(musicSlider.value);
    }

    public void effectsValue()
    {
        FindObjectOfType<AudioManager>().effectsVolumeChange(effectsSlider.value);
    }

    public void back()
    {
        SceneManager.LoadScene("MainMenu");
    }



}
