using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberBehaviour : MonoBehaviour
{
    public static GrabberBehaviour Instance;
    public bool goalReached = false;
    
    [SerializeField] private Animator animator;

    private Vector3 initalPos;
    private Vector3 tempPos;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        initalPos = transform.position;
    }

    /// <summary>
    /// Sets the grabber position to a given offset to account for having reached a checkpoint
    /// </summary>
    /// <param name="offset">Offset from starting position</param>
    public void SetGrabberPositionOffset(Vector3 offset)
    {
        initalPos += offset;
        transform.position = initalPos;
    }

    /// <summary>
    /// Reset grabber position to the last checkpoint and play the placement animation
    /// </summary>
    public void ResetGrabber()
    {
        animator.Play("GrabberStart", 0, 0);
        transform.position = initalPos;
        goalReached = false;
    }

    /// <summary>
    /// Attempt to grab the vehicle at the end position
    /// </summary>
    /// <param name="tempOffset"></param>
    public void GrabVehicle(Vector3 tempOffset)
    {
        transform.position = initalPos + tempOffset;
        animator.Play("GrabberEnd", 0, 0);
    }
}
