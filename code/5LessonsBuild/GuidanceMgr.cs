//this manages the toggles for guidance such as harmony metronome and rhythm
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //for ToList
using UnityEngine.UI; //added for colors and toggles


public class GuidanceMgr : MonoBehaviour
{
    [SerializeField] GameObject ImprovMgr;

    //toggle declarations here
    public Toggle rhythmtoggle, harmonytoggle, metronometoggle;

    //important variables here
    // there is some sort of computation here that we need to map
    // so we're gonna have another function that returns the guidance value
    // for improv manager
    public int guidancevalue = 9; //default is 9
                                  // lets use flags for easy tracking

    public bool rhythm, harmony, metronome;
    //then have a function that if all three are on return guidance value 3


    // Start is called before the first frame update
    void Start()
    {

        //initialize listener toggles here

        //lesson 01 toggle listener
        rhythmtoggle.GetComponent<Toggle>();
        rhythmtoggle.onValueChanged.AddListener(delegate
        {
            RhythmToggleValueChanged(rhythmtoggle);
        });

    }

    //we dont need this 
    //// Update is called once per frame
    //void Update()
    //{

    //}//end update

    //===== toggle listener functions
    //Play a rhythm accompaniement
    public void RhythmToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            guidancevalue = DetermineAccompaniement();
            rhythm = true; 
            Debug.Log("Including rhythm");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Added rhythm.";

        }//endif 
        else
        {
            //change to default lesson
            // lesson = 9;
            guidancevalue = 9; //dont change lesson, just accompaniement
            rhythm = false; 
            Debug.Log("Removing rhythm");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Removed rhythm.";
            // display_name.text = "select lesson";

        }//endelse 

    }//end rhythrmh toggle 



    //Show Harmony (aka Melody Licks)
    public void HarmonyToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            guidancevalue = DetermineAccompaniement();
            harmony = true; 
            Debug.Log("Including harmony (aka left hand)");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Added harmony.";

        }//endif 
        else
        {
            //change to default lesson
            // lesson = 9;
            guidancevalue = 9; //dont change lesson, just accompaniement
            harmony = false; 
            Debug.Log("Removing harmony ");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Removed harmony.";

        }//endelse 

    }//end harmonytoggle

    //Show Harmony (aka Melody Licks)
    public void MetronomeToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            guidancevalue = DetermineAccompaniement();
            metronome = true; 
            Debug.Log("Including metronome");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Added metronome.";

        }//endif 
        else
        {
            //change to default lesson
            // lesson = 9;
            guidancevalue = 9; //dont change lesson, just accompaniement
            metronome = false; 
            Debug.Log("Removing metronome");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Removed metronome";
        }//endelse 

    }//end harmonytoggle

    public int DetermineAccompaniement()
    {
        // r= 2, h=4, m=8, all is 2+4+8 = 14
        if (rhythm && harmony && metronome)
        {
 
            return 14;  // r + h + m
        }//endif
        else if (rhythm && harmony && !metronome)
        {

            return 6; // r + h
        }
        else if (rhythm && metronome && !harmony)
        {
 
            return 10; // r + m
        }
        else if (harmony && metronome && !rhythm)
        {
   
            return 12; // h + m
        }
        else if (rhythm && !harmony && !metronome)
        {

            return 2; // r
        }
        else if (harmony && !rhythm && !metronome)
        {
            return 4; // h 
        }
        else if (metronome && !harmony && !rhythm)
        {
            return 8; // m
        }
        else
        {
            return 9; 
        }
    }//end determine accompaniement


}//end class
