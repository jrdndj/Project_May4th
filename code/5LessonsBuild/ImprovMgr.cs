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
   // [SerializeField] GameObject AudioManager;

    //public static ImprovMgr ImprovMgrInstance;

    //== some variables that ImprovMgr will manage

    public int modeValue = 9, lessonValue = 9, guidanceValue = 9; //mode, lesson and guidance mgrs need these
    public int SelectedIndex = 0; // 0 by default
    int spawntype = 9; //9 is default, 1 is for harmony, 2 is for licks

    //==== UI related variables

    [SerializeField] public Text display_text; //connect to display_text

    //=========== COLOR RELATED VARIABLES ==========/
    //these are the color related objects
    public ColorBlock theColor;
    public Button theButton;
    Color32 improvpink = new Color32(255, 150, 234, 255);
    Color32 yellow = Color.yellow;
    Color32 belowpink = new Color32(75, 0, 130, 255);  //this is indigo akshully
    Color32 blues = new Color32(65, 105, 225, 255); // this is for the blues blue
    Color32 restblack = Color.black; //for the rests 


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
        Debug.Log("Entered manage improv function");
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
        Debug.Log("mode value we have is " + modeValue);
        if (modeValue == 1)
        {
            Debug.Log("lesson value we have is " + lessonValue);
            //now check which lesson
            if (lessonValue == 1)
            {

                Debug.Log("guidance value we have is " + guidanceValue);
                //nowcheck if guidance is chosen
                if (guidanceValue == 4) // only harmony
                {
                    //=====load improv lick first, then load guidance licks applicable


                    //lick
                    RollManager.GetComponent<RollMgr>().Filename = "L0100.mid"; //improv lick
                   // Debug.Log("filename is transferred " + RollManager.GetComponent<RollMgr>().Filename);

                    //InvokeSongManager to access methods to generate PianoRoll
                    RollManager.GetComponent<RollMgr>().InvokeSongManager();

                    //generate MIDIevents  - here we only get the events of the licks
                    RollManager.GetComponent<RollMgr>().GenerateMIDIEvents("L0100.mid");

                    //generate roll
                    RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, 2);

                    //=== we need the y position of the first improv licks since we will base the harmony there 

                    //=======then load rhythm guidance
                    //harmony
                    RollManager.GetComponent<RollMgr>().Filename = "L01LH.mid"; //harmony

                    //InvokeSongManager to access methods to generate PianoRoll
                    RollManager.GetComponent<RollMgr>().InvokeSongManager();

                    //no need to generate midievents for harmony

                    //generate roll
                    RollManager.GetComponent<RollMgr>().GeneratePianoRoll(yellow, 1);

                    //=== then play roll all spawns and play songs

                    //song to be invoked is only one file which is
                    // L0104.aiff
                    //update index for AudioManager

                    SelectedIndex = 0;
                    //  AudioManager.GetComponent<AudioManager>().ChangeAudioSelection(SelectedIndex);

                    //select swing-allmodes-allconfig.aiff for audio

                    //==== template stuff 
                    ////select swing-allmodes-allconfig.midi for pianoroll generation
                    //RollManager.GetComponent<RollMgr>().Filename = "L0104.mid";

                    ////invoke song manager from rollmgr
                    //RollManager.GetComponent<RollMgr>().InvokeSongManager();

                    ////generate piano roll based on these 
                    //RollManager.GetComponent<RollMgr>().GeneratePianoRoll();


                }//end if check guidance
                else if (guidanceValue == 2) //only r
                {

                }//else guidanceValue rhythm only 
                else if (guidanceValue == 8) //only m
                {

                }
                else if (guidanceValue == 4) // only h 
                {

                }
                else if (guidanceValue == 6) //only r + h
                {

                }
                else if (guidanceValue == 10) //only r + m
                {

                }
                else if (guidanceValue == 12) // only h + m
                {

                }
                else if (guidanceValue == 14) // all modes
                {



                }//end - no guidance value - so just update by default BELOW

                //by default there is no guidance so we go to the standard value

                //select swing-allmodes-allconfig.midi for pianoroll generation
                RollManager.GetComponent<RollMgr>().Filename = "L0100.mid"; //must be 00

                //invoke song manager from rollmgr
                RollManager.GetComponent<RollMgr>().InvokeSongManager();

                //generate MIDIevents  - here we only get the events of the licks
                RollManager.GetComponent<RollMgr>().GenerateMIDIEvents("L0100.mid");
                //generate piano roll based on these
                SelectedIndex = 0;
                RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, 2);

               

                //== anything below will now for other lesson


            }//end if check lesson 1
            else if (lessonValue == 2) //sequence mode 
            {


            }//end listen and learn mode ifs

            else if (lessonValue == 3) // motif mode
            {


            }//end motif mode

            else if (lessonValue == 4) //variations mode
            {




            }//end variations mode
            else if (lessonValue == 5) //ques-Ans mode 
            {


            }// by default it is ques-ans mode

            //=== if there is none then no applicable lesson selected
            display_text.text = " ";

        }//end main if for mode 01
        else
        {
            //assume test yourself mode 

        }//end of test yourself mode
    }//end manage improv method

    //========some other methods would be defined here

}//end class
