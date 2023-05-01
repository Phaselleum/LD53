using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
    public static CanvasButtons Instance;

    public GameObject winScreen;
    [SerializeField] private AudioSource winSound;

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
                SceneManager.LoadSceneAsync(0);
            }
        }
    }

    /// <summary>
    /// Displays the winscreen overlay
    /// </summary>
    public void ShowWinScreen()
    {
        winScreen.SetActive(true);
        winSound.Play();
    }

    /// <summary>
    /// Resets the map to the last reached checkpoint
    /// </summary>
    public void ResetToCheckpoint()
    {
        TruckBehaviour.Instance.ResetPosition();
        TimerBarBehaviour.Instance.ResetTimerBar();
        CameraController.Instance.ResetCamera();
        GrabberBehaviour.Instance.ResetGrabber();
    }

    public void Menu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
