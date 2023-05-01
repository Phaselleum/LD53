using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberBehaviour : MonoBehaviour
{
    public static GrabberBehaviour Instance;
    public bool grabberIsActive = true;
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

    public void SetGrabberPositionOffset(Vector3 offset)
    {
        initalPos += offset;
        transform.position = initalPos;
    }

    public void ResetGrabber()
    {
        animator.Play("GrabberStart", 0, 0);
        transform.position = initalPos;
        goalReached = false;
    }

    public void GrabVehicle(Vector3 tempOffset)
    {
        transform.position = initalPos + tempOffset;
        animator.Play("GrabberEnd", 0, 0);
    }
}
