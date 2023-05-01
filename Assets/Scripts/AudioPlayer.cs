using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSource2;

    public void PlayAudio(float start)
    {
        audioSource.time = start;
        audioSource.Play();
    }

    public void PlayAudio2(float start)
    {
        audioSource2.time = start;
        audioSource2.Play();
    }
}
