using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void OnCollisionEnter(Collision other)
    { 
        audioSource.Play();
    }
}
