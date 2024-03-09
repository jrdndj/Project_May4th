using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //playback related variables
    public AudioClip[] clips;
   // public AudioSource MotifToPlay;
    public AudioSource RhythmToPlay;
   // public AudioSource MetronomeToPlay; 

    public int selectedIndex = 0; //default is the 0th clip

    // Start is called before the first frame update
    void Start()
    {

       // MotifToPlay.clip = clips[0]; //this automatically plays the song
         
    }//end start

    /**
     * index 0 - L0100.aiff
     * index 1 - L0104.aiff
     * index 2 - some rhythm
     * 
     * 
     * 
     * **/
    public void ChangeAudioSelection(int selectedIndex)
    {
        ////== 0 is default
        //if (selectedIndex >= 0 && selectedIndex < clips.Length)
        //{
        //    MotifToPlay.clip = clips[selectedIndex];
        //    MotifToPlay.Play();
        //}//end selecting of chosen index
        //else
        //{
        //    Debug.Log("Invalid clip selection");
        //}//end else

    }//end changeAudio Selection

    /**
    * index 0 - L01R0.aiff
    * index 1 - L01R1.aiff
    * index 2 - some rhythm
    * 
    * 
    * 
    * **/
    public void RhythmAudioSelection(int selectedIndex)
    {
        RhythmToPlay.clip = clips[selectedIndex];
        RhythmToPlay.Play();
        //if (selectedIndex==0)
        //{
        //    RhythmToPlay.clip = clips[0];
        //    RhythmToPlay.Play();
        //}//end selecting of chosen index
        //else if(selectedIndex==1)
        //{
        //    RhythmToPlay.clip = clips[1];
        //    RhythmToPlay.Play();
           
        //}//end else if
        //else if (selectedIndex == 2)
        //{
        //    RhythmToPlay.clip = clips[2];
        //    RhythmToPlay.Play();

        //}//end else if
        //else
        //{
        //    Debug.Log("Invalid clip selection");
        //}//end else
    }//end RhythmAudioSelection

    public void MetronomeSelection()
    {
        RhythmToPlay.clip = clips[7];
        RhythmToPlay.Play();
    }

    public void HarmonySelection(int selectedIndex)
    {
        RhythmToPlay.clip = clips[selectedIndex];
        RhythmToPlay.Play();
    }

    public void StopRhythm()
    {
        RhythmToPlay.Stop();
    }//end stoprhythm

    public void StopMetronome()
    {
        RhythmToPlay.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }//end update


}//end audio manager 
