using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
    public static CanvasButtons Instance;

    [SerializeField] private GameObject winScreen;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (winScreen.activeSelf)
            {
                winScreen.SetActive(false);
                SceneManager.LoadScene(0);
            }
        }
    }

    public void ShowWinScreen()
    {
        winScreen.SetActive(true);
    }

    public void Reset()
    {
        TruckBehaviour.Instance.ResetPosition();
        TimerBarBehaviour.Instance.Reset();
    }
}
