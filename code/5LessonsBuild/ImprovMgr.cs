/* Improv Manager now receives all info from the toggles and
 * maps the right audio clip based on the correct combination
 * this is done and received when the button "Load" is clicked. 
 * 
 * improvmgr calls the ff
 * contentmgr to get the right audio clip based on the improv selected
 * rollmgr to generate the viz based on the midi file selected 
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //for ToList
using UnityEngine.UI; //added for colors

/*
 * The general algorithm
 * ImprovMgr awaits for the Load button to be triggered
 * When trigged, it calls content mgr and roll mgr 
 * *
 */
public class ImprovMgr : MonoBehaviour
{
    //== declare Improv to be static so other classes can access it withouts
    [SerializeField] GameObject RollManager;

    //public static ImprovMgr ImprovMgrInstance;

    //== some variables that ImprovMgr will manage

    public int modeValue = 9, lessonValue=9, guidanceValue=9; //mode, lesson and guidance mgrs need these

    //==== UI related variables

    [SerializeField] public Text display_text; //connect to display_text


    // Start is called before the first frame update
    void Start()
    {
        //set display text
        display_text.text = "Select mode, lesson and guidance to begin.";
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //=== function definitions

    // when button is clicked, PullContent gets the right content
    public void ManageImprov()
    {
        /*
         * algo for pull content
         * get mode value
         * get lesson value
         * get guidance value
         * pick the right content to be picked 
         * call ContentMgr
         * call RolLScript
         * **/
        //listen and learn mode
        if (modeValue==1)
        {
            //now check which lesson
            if (lessonValue==1)
            {
                //nowcheck if guidance is chosen
                if (guidanceValue == 4) // only harmony
                {
                    //select swing-allmodes-allconfig.midi for pianoroll generation
                    RollManager.GetComponent<RollMgr>().Filename = "L0104.mid";
                    //select swing-allmodes-allconfig.aiff for audio

                    //call pianoroll for now
                    RollManager.GetComponent<RollMgr>().GeneratePianoRoll();
                }//end if check guidance
                else if (guidanceValue == 2) //only r
                {

                }
                else if (guidanceValue == 8) //only m
                {

                }
                else if (guidanceValue == 4) // only h 
                {

                }
                else if (guidanceValue == 6) //only r + h
                {

                }
                else if (guidanceValue == 10 ) //only r + m
                {

                }
                else if (guidanceValue == 12) // only h + m
                {

                }
                else if (guidanceValue == 14) // all modes
                {

                }//end 
            }//end if check lesson
           

        }//end listen and learn mode ifs


    }//end PullContent

   

    

}//endclass 
