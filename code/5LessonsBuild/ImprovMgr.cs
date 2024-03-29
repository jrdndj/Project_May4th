﻿/* Improv Manager now receives all info from the toggles and
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
    [SerializeField] GameObject RollManager, GuidanceManager, LessonManager, ModeManager, Metronome, MusicSheetMgr;


    public static ImprovMgr Instance;

    //== some variables that ImprovMgr will manage

    public int modeValue = 9, lessonValue = 9, guidanceValue = 9; //mode, lesson and guidance mgrs need these
                                                                  // int SelectedIndex = 0; // 0 by default
    public int spawntype = 9; //9 is default, 1 is for harmony, 2 is for licks

    bool loaded = false;

    public int seqctr = 4;
    public int display_lesson_ctr = 0; //always init to 1 and refresh to 1
    public int display_lesson_max = 8; //set 7 if lessonvalue = 1

    //==== UI related variables

    [SerializeField] public Text display_text; //connect to display_text
    [SerializeField] public Text display_lesson; //for displaying lessons

    //=========== COLOR RELATED VARIABLES ==========/
    //these are the color related objects
    public ColorBlock theColor;
    public Button theButton;
    Color32 improvpink = new Color32(255, 150, 234, 255);
    Color32 yellow = Color.yellow;
    Color32 belowpink = new Color32(75, 0, 130, 255);  //this is indigo akshully
                                                       //  Color32 blues = new Color32(65, 105, 225, 255); // this is for the blues blue
                                                       // Color32 restblack = Color.black; //for the rests 


    //playback related objects and variables
    [SerializeField] GameObject AudioManager;
    public AudioClip[] clips;
    public AudioSource audioSource;
    public bool IsRhythmPlaying = false;

    // Start is called before the first frame update
    void Start()
    {

        //set display text
        display_text.text = "Select mode, lesson and guidance to begin.";

    }//endstart

    // Update is called once per frame
    void Update()
    {
        if (loaded && RollManager.GetComponent<RollMgr>().numOfEvents == RollManager.GetComponent<RollMgr>().ctr)
        {
            display_text.text = "Lesson finished. Select another lesson to continue";
            loaded = false;
            AudioManager.GetComponent<AudioManager>().StopRhythm(); //finish rhythm too

        }  //end if loaded check

    }//end update()

    //=== function definitions

    //introducing ManageSequence so I dont destroy ManageImpprov
    public void ManageSequence()
    {
        
        //metronome is always on but to be SOLID we need to determine accompaniement
        GuidanceManager.GetComponent<GuidanceMgr>().DetermineAccompaniement();

        RollManager.GetComponent<RollMgr>().reload = true;
        display_text.text = "Lesson ongoing...";

        //since its constant, set it here
        RollManager.GetComponent<RollMgr>().swingfrequency = 2; //16-24 works best with swing
        RollManager.GetComponent<RollMgr>().fallSpeed = 16; //formerly 18
        RollManager.GetComponent<RollMgr>().pixelsPerBeat = 24; //formerly 24


        LoadSequence(modeValue, lessonValue, guidanceValue, 0); //should always be zero

    }//end ManageSequence

    //assign sequence for viz filenames on load
    public List<string> AssignSequence(int modeValue, int lessonValue, int guidanceValue, int count)
    {
        //some local variables to streamline data transfer
        List<string> vizfilenames = new List<string>(8);
        int vizindex = count;

        //the filename is in LessonManager.GetComponent<LessonMgr>().lessonlist[lessonValue][lessonValue];
        //dynamically assign the lessons from LessonMgr - this is the viz and the set of 
        if (LessonManager.GetComponent<LessonMgr>().lessonlist.Count > 0)
        {
            List<string> lessonSublessons = LessonManager.GetComponent<LessonMgr>().lessonlist[lessonValue - 1]; // -1 index 
            for (int i = 0; i < 8 && i < lessonSublessons.Count; i++)
            {
                vizfilenames.Add(lessonSublessons[i]); // Add first 8 sublessons to eightElementList
            }
        }//end transfer

        return vizfilenames;

    }//end AssignSequence

    //assign filenames for music sheets on load
    public List<string> AssignSheetFileNames(int modeValue, int lessonValue, int guidanceValue, int count)
    {
        //some local variables to streamline data transfer
        List<string> sheetfilenames = new List<string>(8);
       // int sheetindex = count;

        //the filename is in LessonManager.GetComponent<LessonMgr>().lessonlist[lessonValue][lessonValue];
        //dynamically assign the lessons from LessonMgr - this is the viz and the set of 
        if (LessonManager.GetComponent<LessonMgr>().sheetlist.Count > 0)
        {
            List<string> sheetSublessons = LessonManager.GetComponent<LessonMgr>().sheetlist[lessonValue - 1]; // -1 index 
            for (int i = 0; i < 8 && i < sheetSublessons.Count; i++)
            {
                sheetfilenames.Add(sheetSublessons[i]); // Add first 8 sublessons to eightElementList
            }
        }//end transfer

        return sheetfilenames;
    }//end AssignSequence

    public void LoadSequence(int modeValue, int lessonValue, int guidanceValue, int count)
    {

        //assign filenames that we will pass
        List<string> vizfilenames = AssignSequence(modeValue, lessonValue, guidanceValue, count);
        List<string> sheetfilenames = AssignSheetFileNames(modeValue, lessonValue, guidanceValue, count);
        int vizindex = count;
        string lesson_title; 

        //now show the sheetfilename - by calling MusicSheetManager to update notation handler
        MusicSheetMgr.GetComponent<MusicSheetManager>().SetSheetFilename(sheetfilenames[vizindex]); //no minus 1 here
        Debug.Log("sheet index loaded is " + vizindex);

        //update display lesson regardless of mode comes in two parts
        //part 1 set lesson title
        switch (lessonValue)
        {
            case 1: lesson_title = "Swing"; break;
            case 2: lesson_title = "Sequences"; break;
            case 3: lesson_title = "Motifs"; break;
            case 4: lesson_title = "Variations"; break;
            case 5: lesson_title = "Ques-Ans"; break;
            default: lesson_title = ""; break; 
        }//end switch

        //part 2 set max upper bound 
        if (lessonValue == 1)
        {
            display_lesson_max = 7;
            lesson_title = "Swing";
        }//end
        else
        {
            display_lesson_max = 8; 
        }//end else

        //now update lesson title whenever LoadSequences are called
        display_lesson_ctr = count+1; // should be count now
        display_lesson.text = display_lesson_ctr + "/" + display_lesson_max + "\n " + lesson_title;

        Debug.Log("midi file to load is " + vizindex);
        //now manage everything base on the mode!
        //== dont get confused 1 = w&L, 2 = TEST, 3 = TRY
        if (modeValue == 1) // watch and learn
        {
           
            RollManager.GetComponent<RollMgr>().Filename = vizfilenames[vizindex];
            RollManager.GetComponent<RollMgr>().InvokeSongManager();
            RollManager.GetComponent<RollMgr>().GenerateMIDIEvents(vizfilenames[vizindex]);
            spawntype = 2;
            RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, spawntype, modeValue);
            Metronome.GetComponent<Metronome>().StartMetronome(); 

        }//end modeValue 1
        else if (modeValue == 3) //try yourself
        {
            //code is the same just that in Rollmanager it stops the midi events 
            RollManager.GetComponent<RollMgr>().Filename = vizfilenames[vizindex];
            RollManager.GetComponent<RollMgr>().InvokeSongManager();
            RollManager.GetComponent<RollMgr>().GenerateMIDIEvents(vizfilenames[vizindex]);
            spawntype = 2;
            RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, spawntype, modeValue);
            //if harmony is enabled then no need to play metronome
            if ((guidanceValue == 4 || guidanceValue == 12))
            {
                AudioManager.GetComponent<AudioManager>().HarmonySelection(lessonValue);             
            }//end checkharmony
            else
            {
                Metronome.GetComponent<Metronome>().StartMetronome();

            }//end else

            
        }//end else modevalue 3
        else if (modeValue == 2)//testyourself
        {
            display_text.text = "Your turn, compose on the fly!";
            //if harmony is enabled then no need to play metronome
            if (guidanceValue == 4 || guidanceValue == 12)
            {
                AudioManager.GetComponent<AudioManager>().HarmonySelection(lessonValue);
            }//end checkharmony
            else if (guidanceValue == 8 || guidanceValue == 10)
            {
                Metronome.GetComponent<Metronome>().StartMetronome();
            }
   
            //play rhythm!
            else if ((guidanceValue == 2 || guidanceValue == 10 || guidanceValue == 6 || guidanceValue == 14))
            {
                PlayRhythm();
            }
            else
            {
                Metronome.GetComponent<Metronome>().StartMetronome();

            }//end else
             // RollManager.GetComponent<RollMgr>().audioSource.Play();
        }//end else 
        ////the filename is in LessonManager.GetComponent<LessonMgr>().lessonlist[lessonValue][lessonValue];
        ////dynamically assign the lessons from LessonMgr - this is the viz and the set of 
        //if (LessonManager.GetComponent<LessonMgr>().lessonlist.Count > 0)
        //{
        //    List<string> lessonSublessons = LessonManager.GetComponent<LessonMgr>().lessonlist[lessonValue - 1]; // -1 index 
        //    for (int i = 0; i < 8 && i < lessonSublessons.Count; i++)
        //    {
        //        vizfilenames.Add(lessonSublessons[i]); // Add first 8 sublessons to eightElementList
        //    }
        //}//end transfer

        //have some invokedelay function that sends the filenames per lesson and task
        // call task 1 



        ////set 1
        ////get song information 
        //RollManager.GetComponent<RollMgr>().Filename = vizfilenames[vizindex];
        //RollManager.GetComponent<RollMgr>().InvokeSongManager();
        //RollManager.GetComponent<RollMgr>().GenerateMIDIEvents(vizfilenames[vizindex]);
        //spawntype = 2;
        //RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, spawntype, modeValue);
        //Debug.Log("generated piano roll");
      //  Metronome.GetComponent<Metronome>().StartMetronome();
        //   IsRhythmPlaying = true;

    





        //=== recover start if fck up
        //  //have a loop to load the entire thing
        //  while (vizindex < vizfilenames.Count && RollManager.GetComponent<RollMgr>().reload) {
        ////  while (vizindex < vizfilenames.Count)
        ////  {

        //      //set the pattern from list of midis

        //      //get song information 
        //      RollManager.GetComponent<RollMgr>().Filename = vizfilenames[vizindex]; 
        //      RollManager.GetComponent<RollMgr>().InvokeSongManager();

        //  //note we need this to make sure the key presses get lit. if we wanna change the sound
        //  //its in the co routine in rollmgr
        //      RollManager.GetComponent<RollMgr>().GenerateMIDIEvents(vizfilenames[vizindex]);

        //      //generate viz 
        //      spawntype = 2;
        //      RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, spawntype, modeValue);
        //      Debug.Log("generated piano roll");

        //      //since metronome is always part, call metronome here 
        //      Metronome.GetComponent<Metronome>().StartMetronome();

        //      // seqctr--;

        //      //play metronome
        //      //  Invoke("PlayMetronome", 8.0f);
        //      //   Instance.audioSource.Play();
        //      // RollManager.GetComponent<RollMgr>().audioSource.Play();
        //      IsRhythmPlaying = true;

        //      vizindex++;
        //  }//end foreach

        //==== recover end if fkcup

        //stop metronome here
        //  Metronome.GetComponent<Metronome>().StopMetronome();


        //set the lessons to generate viz

        //get midievents if WaL mode

        //generate PianoRoll

        //generic pattern below




    }//end loadsequence 


    //==== this is the old way we did things without the ff, bw and pause buttons ====/ 

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

        //call determine accompaniement to finalize the value of guidancevalue
        GuidanceManager.GetComponent<GuidanceMgr>().DetermineAccompaniement();
        // guidanceValue = GuidanceManager.GetComponent<GuidanceMgr>().guidancevalue;

        loaded = true;
        display_text.text = "Lesson ongoing...";

        //since its constant, set it here
        RollManager.GetComponent<RollMgr>().swingfrequency = 2; //16-24 works best with swing
        RollManager.GetComponent<RollMgr>().fallSpeed = 16; //formerly 18
        RollManager.GetComponent<RollMgr>().pixelsPerBeat = 24; //formerly 24


        //listen and learn mode
        Debug.Log("mode value we have is " + modeValue);
        if (modeValue == 1 || modeValue == 3) //mode 1 or 3 doesnt matter, only the coroutine changes
        {
            Debug.Log("lesson value we have is " + lessonValue);

            //begin rhythm if true
            if (GuidanceManager.GetComponent<GuidanceMgr>().rhythm && !IsRhythmPlaying)
            {
                //   Invoke("PlayMetronome", 4.0f);
                //   Instance.audioSource.Play();
                // RollManager.GetComponent<RollMgr>().audioSource.Play();
                IsRhythmPlaying = true;
            }

            //now check which lesson
            if (lessonValue == 1)
            {

                //swing specific values for sync
                //RollManager.GetComponent<RollMgr>().swingfrequency = 2;
                //RollManager.GetComponent<RollMgr>().fallSpeed = 17; 
                //RollManager.GetComponent<RollMgr>().pixelsPerBeat = 24;

                //RollManager.GetComponent<RollMgr>().swingfrequency = 2;
                //RollManager.GetComponent<RollMgr>().fallSpeed = 16; //formerly 18
                //RollManager.GetComponent<RollMgr>().pixelsPerBeat = 24;


                Debug.Log("guidance value we have is " + guidanceValue);
                //nowcheck if guidance is chosen
                if (guidanceValue == 4) // only harmony
                {
                    //=====load improv lick first, then load guidance licks applicable

                    //read file to generate lick highlights 
                    RollManager.GetComponent<RollMgr>().Filename = "L0100F.mid"; //improv lick
                                                                                 // Debug.Log("filename is transferred " + RollManager.GetComponent<RollMgr>().Filename);

                    //InvokeSongManager to access methods to generate PianoRoll
                    RollManager.GetComponent<RollMgr>().InvokeSongManager();

                    //generate MIDIevents  - here we only get the events of the licks
                    RollManager.GetComponent<RollMgr>().GenerateMIDIEvents("L0100F.mid");

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
                    //=====load improv lick first, then load guidance licks applicable

                    //read file to generate lick highlights 
                    RollManager.GetComponent<RollMgr>().Filename = "L0100F.mid"; //improv lick

                    //InvokeSongManager to access methods to generate PianoRoll
                    RollManager.GetComponent<RollMgr>().InvokeSongManager();

                    //generate MIDIevents  - here we only get the events of the licks
                    RollManager.GetComponent<RollMgr>().GenerateMIDIEvents("L0100F.mid");

                    //generate piano roll from the file read
                    spawntype = 2;
                    RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, spawntype, modeValue);


                    //=======then load RHYTHM!!! guidance





                    //select swing-allmodes-allconfig.aiff for audio

                    //==== template stuff 
                    ////select swing-allmodes-allconfig.midi for pianoroll generation
                    //RollManager.GetComponent<RollMgr>().Filename = "L0104.mid";

                    ////invoke song manager from rollmgr
                    //RollManager.GetComponent<RollMgr>().InvokeSongManager();

                    ////generate piano roll based on these 
                    //RollManager.GetComponent<RollMgr>().GeneratePianoRoll();


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
                    RollManager.GetComponent<RollMgr>().Filename = "L0100F.mid"; //must be 00

                    //invoke song manager from rollmgr
                    RollManager.GetComponent<RollMgr>().InvokeSongManager();

                    //generate MIDIevents  - here we only get the events of the licks
                    RollManager.GetComponent<RollMgr>().GenerateMIDIEvents("L0100F.mid");
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
                // RollManager.GetComponent<RollMgr>().swingfrequency = 3;
                //RollManager.GetComponent<RollMgr>().fallSpeed = 18;
                //RollManager.GetComponent<RollMgr>().pixelsPerBeat = 24f;

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
                    RollManager.GetComponent<RollMgr>().Filename = "L0200F.mid"; //improv lick

                    //InvokeSongManager to access methods to generate PianoRoll
                    RollManager.GetComponent<RollMgr>().InvokeSongManager();

                    //generate MIDIevents  - here we only get the events of the licks
                    RollManager.GetComponent<RollMgr>().GenerateMIDIEvents("L0200F.mid");

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
                RollManager.GetComponent<RollMgr>().Filename = "L0200F.mid"; //must be 00

                //invoke song manager from rollmgr
                RollManager.GetComponent<RollMgr>().InvokeSongManager();

                //generate MIDIevents  - here we only get the events of the licks
                RollManager.GetComponent<RollMgr>().GenerateMIDIEvents("L0200F.mid");
                //generate piano roll based on these
                spawntype = 2;
                RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, spawntype, modeValue);

            }//end listen and learn mode ifs

            else if (lessonValue == 3) // motif mode
            {

                //RollManager.GetComponent<RollMgr>().swingfrequency = 3;
                //RollManager.GetComponent<RollMgr>().fallSpeed = 18;
                //RollManager.GetComponent<RollMgr>().pixelsPerBeat = 24f;

                //default value for lesson 02
                Debug.Log("guidance value we have is " + guidanceValue);

                //change swing frequency to 2
                RollManager.GetComponent<RollMgr>().swingfrequency = 4;

                //change velocity
                RollManager.GetComponent<RollMgr>().velocity = 60;

                //nowcheck if guidance is chosen
                if (guidanceValue == 4) // only harmony
                {
                    //=====load improv lick first, then load guidance licks applicable

                    //lick
                    RollManager.GetComponent<RollMgr>().Filename = "L0300F.mid"; //improv lick

                    //InvokeSongManager to access methods to generate PianoRoll
                    RollManager.GetComponent<RollMgr>().InvokeSongManager();

                    //generate MIDIevents  - here we only get the events of the licks
                    RollManager.GetComponent<RollMgr>().GenerateMIDIEvents("L0300F.mid");

                    //generate roll
                    spawntype = 2;
                    RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, spawntype, modeValue);

                    //=== we need the y position of the first improv licks since we will base the harmony there 

                    //=======then load rhythm guidance
                    //harmony
                    // RollManager.GetComponent<RollMgr>().Filename = "L03LH.mid"; //harmony

                    //InvokeSongManager to access methods to generate PianoRoll
                    //  RollManager.GetComponent<RollMgr>().InvokeSongManager();

                    //no need to generate midievents for harmony

                    //generate roll
                    //  RollManager.GetComponent<RollMgr>().GeneratePianoRoll(yellow, 1, modeValue);

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

                //select motif-allmodes-allconfig.midi for pianoroll generation
                RollManager.GetComponent<RollMgr>().Filename = "L0300F.mid"; //must be 00

                //invoke song manager from rollmgr
                RollManager.GetComponent<RollMgr>().InvokeSongManager();

                //generate MIDIevents  - here we only get the events of the licks
                RollManager.GetComponent<RollMgr>().GenerateMIDIEvents("L0300F.mid");
                //generate piano roll based on these
                spawntype = 2;
                RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, spawntype, modeValue);


            }//end motif mode

            else if (lessonValue == 4) //variations mode
            {

                //RollManager.GetComponent<RollMgr>().swingfrequency = 3;
                //RollManager.GetComponent<RollMgr>().fallSpeed = 18;
                //RollManager.GetComponent<RollMgr>().pixelsPerBeat = 24f;

                //default value for lesson 02
                Debug.Log("guidance value we have is " + guidanceValue);

                //change swing frequency to 2
                RollManager.GetComponent<RollMgr>().swingfrequency = 4;

                //change velocity
                RollManager.GetComponent<RollMgr>().velocity = 60;

                //nowcheck if guidance is chosen
                if (guidanceValue == 4) // only harmony
                {
                    //=====load improv lick first, then load guidance licks applicable

                    //lick
                    RollManager.GetComponent<RollMgr>().Filename = "L0400F.mid"; //improv lick

                    //InvokeSongManager to access methods to generate PianoRoll
                    RollManager.GetComponent<RollMgr>().InvokeSongManager();

                    //generate MIDIevents  - here we only get the events of the licks
                    RollManager.GetComponent<RollMgr>().GenerateMIDIEvents("L0400F.mid");

                    //generate roll
                    spawntype = 2;
                    RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, spawntype, modeValue);

                    //=== we need the y position of the first improv licks since we will base the harmony there 

                    //=======then load rhythm guidance
                    //harmony
                    // RollManager.GetComponent<RollMgr>().Filename = "L03LH.mid"; //harmony

                    //InvokeSongManager to access methods to generate PianoRoll
                    //  RollManager.GetComponent<RollMgr>().InvokeSongManager();

                    //no need to generate midievents for harmony

                    //generate roll
                    //  RollManager.GetComponent<RollMgr>().GeneratePianoRoll(yellow, 1, modeValue);

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

                //select motif-allmodes-allconfig.midi for pianoroll generation
                RollManager.GetComponent<RollMgr>().Filename = "L0400F.mid"; //must be 00

                //invoke song manager from rollmgr
                RollManager.GetComponent<RollMgr>().InvokeSongManager();

                //generate MIDIevents  - here we only get the events of the licks
                RollManager.GetComponent<RollMgr>().GenerateMIDIEvents("L0400F.mid");
                //generate piano roll based on these
                spawntype = 2;
                RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, spawntype, modeValue);



            }//end variations mode
            else if (lessonValue == 5) //ques-Ans mode 
            {

                //RollManager.GetComponent<RollMgr>().swingfrequency = 3;
                //RollManager.GetComponent<RollMgr>().fallSpeed = 18;
                //RollManager.GetComponent<RollMgr>().pixelsPerBeat = 24f;

                //default value for lesson 02
                Debug.Log("guidance value we have is " + guidanceValue);

                //change swing frequency to 2
                RollManager.GetComponent<RollMgr>().swingfrequency = 4;

                //change velocity
                RollManager.GetComponent<RollMgr>().velocity = 60;

                //nowcheck if guidance is chosen
                if (guidanceValue == 4) // only harmony
                {
                    //=====load improv lick first, then load guidance licks applicable

                    //lick
                    RollManager.GetComponent<RollMgr>().Filename = "L0500F.mid"; //improv lick

                    //InvokeSongManager to access methods to generate PianoRoll
                    RollManager.GetComponent<RollMgr>().InvokeSongManager();

                    //generate MIDIevents  - here we only get the events of the licks
                    RollManager.GetComponent<RollMgr>().GenerateMIDIEvents("L0500F.mid");

                    //generate roll
                    spawntype = 2;
                    RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, spawntype, modeValue);

                    //=== we need the y position of the first improv licks since we will base the harmony there 

                    //=======then load rhythm guidance
                    //harmony
                    // RollManager.GetComponent<RollMgr>().Filename = "L03LH.mid"; //harmony

                    //InvokeSongManager to access methods to generate PianoRoll
                    //  RollManager.GetComponent<RollMgr>().InvokeSongManager();

                    //no need to generate midievents for harmony

                    //generate roll
                    //  RollManager.GetComponent<RollMgr>().GeneratePianoRoll(yellow, 1, modeValue);

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

                //select motif-allmodes-allconfig.midi for pianoroll generation
                RollManager.GetComponent<RollMgr>().Filename = "L0500F.mid"; //must be 00

                //invoke song manager from rollmgr
                RollManager.GetComponent<RollMgr>().InvokeSongManager();

                //generate MIDIevents  - here we only get the events of the licks
                RollManager.GetComponent<RollMgr>().GenerateMIDIEvents("L0500F.mid");
                //generate piano roll based on these
                spawntype = 2;
                RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, spawntype, modeValue);


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


    //rhythm related scripts
    //selecting audio for audiosource
    //playing the audio source with delay
    // 0 - 100bpm, 1-100bpm, 2-idk, 3-120bpm, 4-120bpm
    public void PlayRhythm()
    {
        //decentralising to AudioManager game object 
        AudioManager.GetComponent<AudioManager>().RhythmAudioSelection(0); //change this one 
                                                                           //Instance.audioSource.Play();
                                                                           //Instance.MotifToPlay.Play();
    }//endplaydelayedaudio

    public void PlayMetronome()
    {
        //decentralising to AudioManager game object 
        AudioManager.GetComponent<AudioManager>().MetronomeSelection();
    }//end play metronome

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
        RollManager.GetComponent<RollMgr>().IsRhythmPlaying = false;

        //stop audio
        AudioManager.GetComponent<AudioManager>().StopRhythm();

        //remove sheets
        MusicSheetMgr.GetComponent<MusicSheetManager>().ClearSheets();

        //stop metronome
        Metronome.GetComponent<Metronome>().StopMetronome();

        //clear counter too
        RollManager.GetComponent<RollMgr>().ctr = 0;
        display_lesson_ctr = 0;
        display_lesson_max = 0;
        display_lesson.text = ">"; 


        //clear all MIDIevents un played
        RollManager.GetComponent<RollMgr>().noteInfo.Clear();

        //clear hihglights in the keyboard to be safe
        RollManager.GetComponent<RollMgr>().CleanupKeyboard();

        //clearing of all inner values
        modeValue = 9;
        lessonValue = 9;
        guidanceValue = 9;
        IsRhythmPlaying = false;


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

    //this is some change sequence function for the buttons
    public void ClearSpawns()
    {
        //=== this is kinda like ResetAllValues but without clearing the selection and
        //we are just moving to the next or previous lessons
        //destroy objects
        RemoveObjectsWithParent("RollManager");

        //stop all coroutines
        RollManager.GetComponent<RollMgr>().StopAllCoroutines(); //added this to be sure
        RollManager.GetComponent<RollMgr>().IsMotifPlaying = false;
        RollManager.GetComponent<RollMgr>().IsRhythmPlaying = false;

        //stop audio
        AudioManager.GetComponent<AudioManager>().StopRhythm();

        //remove sheets
        MusicSheetMgr.GetComponent<MusicSheetManager>().ClearSheets();

        //stop metronome
        Metronome.GetComponent<Metronome>().StopMetronome();

        //clear counter too
        RollManager.GetComponent<RollMgr>().ctr = 0;

        //clear all MIDIevents un played
        RollManager.GetComponent<RollMgr>().noteInfo.Clear();

        //clear hihglights in the keyboard to be safe
        RollManager.GetComponent<RollMgr>().CleanupKeyboard();
    }

}//end class
