using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this code was generated with GPT 
public class Metronome : MonoBehaviour
{
    float bpm = 100f; // Beats per minute
    private float nextBeatTime;
    public AudioSource audioSource; // Assign your AudioSource component in the Unity Editor
    public AudioClip clickSound; // Assign your .aiff click sound in the Unity Editor

    void Start()
    {
        nextBeatTime = Time.time;
    }

    // Method to start the metronome
    public void StartMetronome()
    {
        nextBeatTime = Time.time;
        float interval = 60f / bpm; // Calculate the time interval between each beat
        InvokeRepeating(nameof(PlayClickSound), 0f, interval); // Start generating click sounds with the calculated interval
    }

    // Method to stop the metronome
    public void StopMetronome()
    {
        CancelInvoke(nameof(PlayClickSound)); // Stop generating click sounds
    }

    void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound); // Play the assigned click sound
    }
}