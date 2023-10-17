using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //playback related variables
    public AudioClip[] clips;
    public AudioSource MotifToPlay;

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
        //== 0 is default
        if (selectedIndex >= 0 && selectedIndex < clips.Length)
        {
            MotifToPlay.clip = clips[selectedIndex];
            MotifToPlay.Play();
        }//end selecting of chosen index
        else
        {
            Debug.Log("Invalid clip selection");
        }//end else

    }//end changeAudio Selection

    // Update is called once per frame
    void Update()
    {
        
    }//end update


}//end audio manager 
