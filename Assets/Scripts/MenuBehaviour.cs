using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Scene01");
    }
    
    public void Instructions()
    {
        SceneManager.LoadScene("Instructions");
    }
    
    public void Quit()
    {
        Application.Quit();
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
