﻿//this manages the toggles for guidance such as harmony metronome and rhythm
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

    public bool rhythm=false , harmony=false, metronome = false;
    //then have a function that if all three are on return guidance value 3


    // Start is called before the first frame update
    void Start()
    {

        //initialize listener toggles here

        //rhythm toggle listener
        rhythmtoggle.GetComponent<Toggle>();
        rhythmtoggle.onValueChanged.AddListener(delegate
        {
            RhythmToggleValueChanged(rhythmtoggle);
        });

        //harmony toggle listener
        harmonytoggle.GetComponent<Toggle>();
        harmonytoggle.onValueChanged.AddListener(delegate
        {
            HarmonyToggleValueChanged(harmonytoggle);
        });

        //metronome toggle listener
        metronometoggle.GetComponent<Toggle>();
        metronometoggle.onValueChanged.AddListener(delegate
        {
            MetronomeToggleValueChanged(metronometoggle);
        });
    }//end Start

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
            rhythm = true;
           // guidancevalue = DetermineAccompaniement();
            
            Debug.Log("Including rhythm");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Added rhythm.";

            //now send the value to ImprovMgr
         //   ImprovMgr.GetComponent<ImprovMgr>().guidanceValue = guidancevalue;
         //s   guidancevalue = DetermineAccompaniement();

        }//endif 
        else
        {
            //change to default lesson
            // lesson = 9;
            rhythm = false;
            guidancevalue = 9; //dont change lesson, just accompaniement

            Debug.Log("Removing rhythm");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Removed rhythm.";
            // display_name.text = "select lesson";

            //now send the value to ImprovMgr
         //   ImprovMgr.GetComponent<ImprovMgr>().guidanceValue = 9;

        }//endelse 

    }//end rhythrmh toggle 



    //Show Harmony (aka Melody Licks)
    public void HarmonyToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            harmony = true;
          //  guidancevalue = DetermineAccompaniement();
          
            Debug.Log("Including harmony (aka left hand)");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Added harmony.";

           // Debug.Log("guidance value sent to improvmgr is " + guidancevalue );

            //now send the value to ImprovMgr
         //   ImprovMgr.GetComponent<ImprovMgr>().guidanceValue = guidancevalue;
        //    guidancevalue = DetermineAccompaniement();

        }//endif 
        else
        {
            //change to default lesson
            // lesson = 9;
            harmony = false;
         //   guidancevalue = 9; //dont change lesson, just accompaniement

            Debug.Log("Removing harmony ");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Removed harmony.";

            //now send the value to ImprovMgr
       //     ImprovMgr.GetComponent<ImprovMgr>().guidanceValue = 9;

        }//endelse 

    }//end harmonytoggle

    //Show Harmony (aka Melody Licks)
    public void MetronomeToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            metronome = true;
           // guidancevalue = DetermineAccompaniement();
            
            Debug.Log("Including metronome");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Added metronome.";

            //now send the value to ImprovMgr
        //    ImprovMgr.GetComponent<ImprovMgr>().guidanceValue = guidancevalue;
          //  guidancevalue = DetermineAccompaniement();
        }//endif 
        else
        {
            //change to default lesson
            // lesson = 9;
            metronome = false;
         //   guidancevalue = 9; //dont change lesson, just accompaniement

            Debug.Log("Removing metronome");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Removed metronome";

            //now send the value to ImprovMgr
         //   ImprovMgr.GetComponent<ImprovMgr>().guidanceValue = 9;
        }//endelse 

    }//end harmonytoggle

    public void DetermineAccompaniement()
    {
        // r= 2, h=4, m=8, all is 2+4+8 = 14
        if (rhythm && harmony && metronome)
        {
        ///    Debug.Log("RHM" + rhythm + harmony + metronome);
            ImprovMgr.GetComponent<ImprovMgr>().guidanceValue = 14;
            // return 14;  // r + h + m
        }//endif
        else if (rhythm && harmony && !metronome)
        {
         //   Debug.Log("RHM" + rhythm + harmony + metronome);
            ImprovMgr.GetComponent<ImprovMgr>().guidanceValue = 6 ;
            // return 6; // r + h
        }
        else if(rhythm && metronome && !harmony)
        {
        //    Debug.Log("RHM" + rhythm + harmony + metronome);
            ImprovMgr.GetComponent<ImprovMgr>().guidanceValue = 10; 
            //return 10; // r + m
        }
        else if(harmony && metronome && !rhythm)
        {
        //    Debug.Log("RHM" + rhythm + harmony + metronome);
            ImprovMgr.GetComponent<ImprovMgr>().guidanceValue = 12; 
            //return 12; // h + m
        }
        else if(rhythm && !harmony && !metronome)
        {
         //   Debug.Log("RHM" + rhythm + harmony + metronome);
            ImprovMgr.GetComponent<ImprovMgr>().guidanceValue = 2; 
            //return 2; // r
        }
        else if(harmony && !rhythm && !metronome)
        {
          //  Debug.Log("RHM" + rhythm + harmony + metronome);
            ImprovMgr.GetComponent<ImprovMgr>().guidanceValue = 4;
            // return 4; // h 
        }
        else if (metronome && !harmony && !rhythm)
        {
          //  Debug.Log("RHM" + rhythm + harmony + metronome);
            ImprovMgr.GetComponent<ImprovMgr>().guidanceValue = 8;
            //return 8; // m
        }
        else
        {
           // Debug.Log("RHM" + rhythm + harmony + metronome);
            ImprovMgr.GetComponent<ImprovMgr>().guidanceValue = 9; 
            //return 9; //default 
        }
    }//end determine accompaniement


}//end class
