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

        MotifToPlay.clip = clips[0];
         
    }//end start

    public void ChangeAudioSelection(int selectedIndex)
    {
        //=== 0 is swing
        //=== 1 for lick 01 and so on.. 
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
