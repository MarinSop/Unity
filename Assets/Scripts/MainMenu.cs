using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<AudioManager>().PlayMusic("MainMenu");
    }

    public void ExitButton()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Level1");
    }


    public void OptionsMenu()
    {
        SceneManager.LoadScene("Options");
    }


}
