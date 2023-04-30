using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
    public static CanvasButtons Instance;

    public GameObject winScreen;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        //returns to the default level
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (winScreen.activeSelf)
            {
                winScreen.SetActive(false);
                SceneManager.LoadScene(0);
            }
        }
    }

    /// <summary>
    /// Displays the winscreen overlay
    /// </summary>
    public void ShowWinScreen()
    {
        //winScreen.SetActive(true);
    }

    /// <summary>
    /// Resets the map to the last reached checkpoint
    /// </summary>
    public void Reset()
    {
        TruckBehaviour.Instance.ResetPosition();
        TimerBarBehaviour.Instance.Reset();
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
