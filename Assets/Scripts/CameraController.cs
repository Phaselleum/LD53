using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    
    [SerializeField] private Transform target;
    [SerializeField] private Camera camera;
    /// <summary> Initial offset of the camera to the target </summary>
    private Vector3 offset = Vector3.zero;

    private Vector3 initialPos;

    private float startTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        startTime = 0;
        initialPos = transform.position;
    }

    private void Update()
    {
        //Don't follow the target in the first 5 seconds (to account for grabber animation)
        startTime += Time.deltaTime;
        if (startTime < 5) return;
        if(offset == Vector3.zero)
        {
            offset = transform.position - target.position;
        }

        //Don't follow the target after the goal has been reached
        if (GrabberBehaviour.Instance.goalReached) return;
        //follow the target object
        transform.position = target.position + offset;
    }

    /// <summary>
    /// Sets the initial camera position in case a checkpoint has been reached
    /// </summary>
    /// <param name="newOffset">Offset from starting position</param>
    public void SetInitialCameraPosition(Vector3 newOffset)
    {
        initialPos += newOffset;
        transform.position = initialPos;
    }

    /// <summary>
    /// Reset camera position to last checkpoint
    /// </summary>
    public void ResetCamera()
    {
        startTime = 0;
        transform.position = initialPos;
    }
}
