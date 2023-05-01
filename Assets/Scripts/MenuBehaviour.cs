using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadSceneAsync("Scene01");
    }
    
    public void Instructions()
    {
        SceneManager.LoadSceneAsync("Instructions");
    }
    
    public void Levels()
    {
        SceneManager.LoadSceneAsync("Levels");
    }
    
    public void Level1()
    {
        SceneManager.LoadSceneAsync("Scene01");
    }
    
    public void Level2()
    {
        SceneManager.LoadSceneAsync("Scene02");
    }
    
    public void Level3()
    {
        SceneManager.LoadSceneAsync("Scene03");
    }
    
    public void Level4()
    {
        SceneManager.LoadSceneAsync("Scene04");
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
