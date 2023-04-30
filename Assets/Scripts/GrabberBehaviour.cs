using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberBehaviour : MonoBehaviour
{
    public static GrabberBehaviour Instance;
    public bool grabberIsActive = true;
    
    [SerializeField] private Animator startGrabber;
    [SerializeField] private Animator endGrabber;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartGrabberAction();
    }

    private void StartGrabberAction()
    {
        //startGrabber.Play();
    }

    private void EndGrabberAction()
    {
        //endGrabber.Play();
    }
}
