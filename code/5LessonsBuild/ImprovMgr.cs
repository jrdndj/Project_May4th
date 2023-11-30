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
    [SerializeField] GameObject RollManager, GuidanceManager, LessonManager, ModeManager;

    //== some variables that ImprovMgr will manage

    public int modeValue = 9, lessonValue = 9, guidanceValue = 9; //mode, lesson and guidance mgrs need these
                                                                  // int SelectedIndex = 0; // 0 by default
    public int spawntype = 9; //9 is default, 1 is for harmony, 2 is for licks

    //==== UI related variables

    [SerializeField] public Text display_text; //connect to display_text

    //=========== COLOR RELATED VARIABLES ==========/
    //these are the color related objects
    public ColorBlock theColor;
    public Button theButton;
    Color32 improvpink = new Color32(255, 150, 234, 255);
    Color32 yellow = Color.yellow;
    Color32 belowpink = new Color32(75, 0, 130, 255);  //this is indigo akshully
                                                       //  Color32 blues = new Color32(65, 105, 225, 255); // this is for the blues blue
                                                       // Color32 restblack = Color.black; //for the rests 

    // Start is called before the first frame update
    void Start()
    {
        //set display text
        display_text.text = "Select mode, lesson and guidance to begin.";

    }//endstart

    // Update is called once per frame
    void Update()
    {

    }//end update()

    //=== function definitions

    // when button is clicked, PullContent gets the right content
    public void ManageImprov()
    {
        //  Debug.Log("Entered manage improv function");
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
        if (modeValue == 1 || modeValue == 3) //mode 1 or 3 doesnt matter, only the coroutine changes
        {
            Debug.Log("lesson value we have is " + lessonValue);
            //now check which lesson
            if (lessonValue == 1)
            {

                //swing specific values for sync
                //RollManager.GetComponent<RollMgr>().swingfrequency = 2;
                //RollManager.GetComponent<RollMgr>().fallSpeed = 17; 
                //RollManager.GetComponent<RollMgr>().pixelsPerBeat = 24;

                RollManager.GetComponent<RollMgr>().swingfrequency = 2;
                RollManager.GetComponent<RollMgr>().fallSpeed = 18;
                RollManager.GetComponent<RollMgr>().pixelsPerBeat = 24;


                Debug.Log("guidance value we have is " + guidanceValue);
                //nowcheck if guidance is chosen
                if (guidanceValue == 4) // only harmony
                {
                    //=====load improv lick first, then load guidance licks applicable

                    //read file to generate lick highlights 
                    RollManager.GetComponent<RollMgr>().Filename = "L0100g.mid"; //improv lick
                                                                                 // Debug.Log("filename is transferred " + RollManager.GetComponent<RollMgr>().Filename);

                    //InvokeSongManager to access methods to generate PianoRoll
                    RollManager.GetComponent<RollMgr>().InvokeSongManager();

                    //generate MIDIevents  - here we only get the events of the licks
                    RollManager.GetComponent<RollMgr>().GenerateMIDIEvents("L0100g.mid");

                    //generate piano roll from the file read
                    spawntype = 2;
                    RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, spawntype, modeValue);

                    //=== we need the y position of the first improv licks since we will base the harmony there 

                    //=======then load rhythm guidance

                    //harmony
                    RollManager.GetComponent<RollMgr>().Filename = "L01LH.mid"; //harmony

                    //InvokeSongManager to access methods to generate PianoRoll
                    RollManager.GetComponent<RollMgr>().InvokeSongManager();

                    //no need to generate midievents for harmony

                    //generate roll
                    spawntype = 1;
                    RollManager.GetComponent<RollMgr>().GeneratePianoRoll(yellow, spawntype, modeValue);



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
                else         //by default there is no guidance so we go to the standard value
                {
                    //select swing-allmodes-allconfig.midi for pianoroll generation
                    RollManager.GetComponent<RollMgr>().Filename = "L0100g.mid"; //must be 00

                    //invoke song manager from rollmgr
                    RollManager.GetComponent<RollMgr>().InvokeSongManager();

                    //generate MIDIevents  - here we only get the events of the licks
                    RollManager.GetComponent<RollMgr>().GenerateMIDIEvents("L0100g.mid");
                    //generate piano roll based on these

                    spawntype = 2;
                    RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, spawntype, modeValue);

                }//end else default mode

            }//end if check lesson 1
            else if (lessonValue == 2) //sequence mode 
            {

                //sequence specific values for sync
                //RollManager.GetComponent<RollMgr>().swingfrequency = 3;
                //RollManager.GetComponent<RollMgr>().fallSpeed = 16;
                //RollManager.GetComponent<RollMgr>().pixelsPerBeat = 22.5f;
                RollManager.GetComponent<RollMgr>().swingfrequency = 3;
                RollManager.GetComponent<RollMgr>().fallSpeed = 18;
                RollManager.GetComponent<RollMgr>().pixelsPerBeat = 24f;

                //default value for lesson 02
                Debug.Log("guidance value we have is " + guidanceValue);

                //change swing frequency to 2
                RollManager.GetComponent<RollMgr>().swingfrequency = 3;

                //change velocity
                RollManager.GetComponent<RollMgr>().velocity = 60;

                //nowcheck if guidance is chosen
                if (guidanceValue == 4) // only harmony
                {
                    //=====load improv lick first, then load guidance licks applicable

                    //lick
                    RollManager.GetComponent<RollMgr>().Filename = "L0200.mid"; //improv lick

                    //InvokeSongManager to access methods to generate PianoRoll
                    RollManager.GetComponent<RollMgr>().InvokeSongManager();

                    //generate MIDIevents  - here we only get the events of the licks
                    RollManager.GetComponent<RollMgr>().GenerateMIDIEvents("L0200.mid");

                    //generate roll
                    spawntype = 2;
                    RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, spawntype, modeValue);

                    //=== we need the y position of the first improv licks since we will base the harmony there 

                    //=======then load rhythm guidance
                    //harmony
                    RollManager.GetComponent<RollMgr>().Filename = "L02LH.mid"; //harmony

                    //InvokeSongManager to access methods to generate PianoRoll
                    RollManager.GetComponent<RollMgr>().InvokeSongManager();

                    //no need to generate midievents for harmony

                    //generate roll
                    RollManager.GetComponent<RollMgr>().GeneratePianoRoll(yellow, 1, modeValue);

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
                RollManager.GetComponent<RollMgr>().Filename = "L0200.mid"; //must be 00

                //invoke song manager from rollmgr
                RollManager.GetComponent<RollMgr>().InvokeSongManager();

                //generate MIDIevents  - here we only get the events of the licks
                RollManager.GetComponent<RollMgr>().GenerateMIDIEvents("L0200.mid");
                //generate piano roll based on these
                spawntype = 2;
                RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, spawntype, modeValue);

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

        }//end main if for mode 01 or mode 03        
        else if (modeValue == 2) // test yourself mode
        {
            //do something here 

        }//end of test yourself mode
    }//end manage improv method

    //========some other methods would be defined here

    //this method destroys all objects whose parent is RollManager (thereby clears all falling piano notes)
    //thanks gpt for this
    void RemoveObjectsWithParent(string parentName)
    {
        // Find all objects of type GameObject in the scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        // Iterate through each object
        foreach (GameObject obj in allObjects)
        {
            // Check if the parent of the object is named "RollManager"
            if (obj.transform.parent != null && obj.transform.parent.name == parentName)
            {
                // Destroy the object
                Destroy(obj);
            }//end if check parent name
        }//end foreach
        Debug.Log("Piano roll objects have been cleared");
    }//end removeobjects 

    //one catch all function to reset lessons, modes, guidances and destroys all objects and stops all coroutines
    public void ResetAllValues()
    {
        //== algorithm
        // 01 destroy all objects - ok 
        // 02 end all coroutines - ok -within meta 
        // 03 clear all modes, guidances, modes, toggles - ok 
        // 04 start from the scratch - ok!
        // 05 clear the keyboard of any highlights just to be safe - ok
        // 06 reset isMotifplaying so it can be resumed again
        // 07 clear the contents of the midi co routine so it is fresh

        //destroy objects
        RemoveObjectsWithParent("RollManager");

        //stop all coroutines
        RollManager.GetComponent<RollMgr>().StopAllCoroutines(); //added this to be sure
        RollManager.GetComponent<RollMgr>().IsMotifPlaying = false;

        //clear counter too
        RollManager.GetComponent<RollMgr>().ctr = 0; 

        //clear all MIDIevents un played
        RollManager.GetComponent<RollMgr>().noteInfo.Clear(); 

        //clear hihglights in the keyboard to be safe
        RollManager.GetComponent<RollMgr>().CleanupKeyboard();

        //clearing of all inner values
        modeValue = 9;
        lessonValue = 9;
        guidanceValue = 9;
          

        //clear guidance toggles 
        GuidanceManager.GetComponent<GuidanceMgr>().rhythmtoggle.isOn = false;
        GuidanceManager.GetComponent<GuidanceMgr>().harmonytoggle.isOn = false;
        GuidanceManager.GetComponent<GuidanceMgr>().metronometoggle.isOn = false;

        //clear lesson toggles 
        LessonManager.GetComponent<LessonMgr>().lesson01_modeswing.isOn = false;
        LessonManager.GetComponent<LessonMgr>().lesson02_sequencing.isOn = false;
        LessonManager.GetComponent<LessonMgr>().lesson03_motifs.isOn = false;
        LessonManager.GetComponent<LessonMgr>().lesson04_variations.isOn = false;
        LessonManager.GetComponent<LessonMgr>().lesson05_quesans.isOn = false;

        //clear mode toggles
        ModeManager.GetComponent<ModeMgr>().listentoggle.isOn = false;
        ModeManager.GetComponent<ModeMgr>().trytoggle.isOn = false;
        ModeManager.GetComponent<ModeMgr>().testtoggle.isOn = false;
    }//end reset all values 

}//end class
