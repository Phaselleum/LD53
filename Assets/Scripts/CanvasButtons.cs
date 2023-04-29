using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
    public void Reset()
    {
        TruckBehaviour.Instance.ResetPosition();
        TimerBarBehaviour.Instance.Reset();
    }
}
