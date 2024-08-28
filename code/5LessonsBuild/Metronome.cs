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
    public bool metronomestarted = false; 

    void Start()
    {
        nextBeatTime = Time.time;
    }//end start 

    // Method to start the metronome
    public void StartMetronome()
    {
        nextBeatTime = Time.time;
        float interval = 60f / bpm; // formerly 60, 56f was working
        InvokeRepeating(nameof(PlayClickSound), 0f, interval); // Start generating click sounds with the calculated interval
    }//end startmetronome

    // Method to stop the metronome
    public void StopMetronome()
    {
        CancelInvoke(nameof(PlayClickSound)); // Stop generating click sounds
    }//end stop metronome

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound, 0.05f); // Play the assigned click sound
    }//end void playclicksound

    public void FourBeatStart()
    {
         float delayBetweenTicks = 0.6f; //0.56f
        // Schedule four ticks with a delay between each one
        //Invoke(nameof(PlayClickSound), 0f);
        Invoke(nameof(PlayClickSound), delayBetweenTicks);
        Invoke(nameof(PlayClickSound), delayBetweenTicks * 2);
        Invoke(nameof(PlayClickSound), delayBetweenTicks * 3);
        Invoke(nameof(PlayClickSound), delayBetweenTicks * 4); // formerly 0 
        Invoke(nameof(PlayClickSound), delayBetweenTicks * 5);
        Invoke(nameof(PlayClickSound), delayBetweenTicks * 6);
        Invoke(nameof(PlayClickSound), delayBetweenTicks * 7);
        Invoke(nameof(PlayClickSound), delayBetweenTicks * 8);
    }
}//end metronome class