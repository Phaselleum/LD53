using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Camera camera;
    /// <summary> Initial offset of the camera to the target </summary>
    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    private void Update()
    {
        //follow the target object
        transform.position = target.position + offset;
    }

}
