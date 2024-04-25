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
    [SerializeField] GameObject RollManager, GuidanceManager, LessonManager, ModeManager, Metronome, MusicSheetMgr;


    public static ImprovMgr Instance;

    //== some variables that ImprovMgr will manage

    public int modeValue = 9, lessonValue = 9, guidanceValue = 9; //mode, lesson and guidance mgrs need these
                                                                  // int SelectedIndex = 0; // 0 by default
    public int spawntype = 9; //9 is default, 1 is for harmony, 2 is for licks

    public bool loaded = false;
    public bool CanReload = false;

    public int seqctr = 0;
    public int display_lesson_ctr = 0; //always init to 1 and refresh to 1
    public int display_lesson_max = 8; //set 7 if lessonvalue = 1

    public int vizindex, harmonyindex = 0;

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
        //note this function decides the second, third and fourth in the protocol
        //W&L 4B 4B 4P 4P
        // Test none
        // Compose 4B 4P 4B 4P

        //== if mode 1
        if (modeValue == 1)
        {
            if (loaded && RollManager.GetComponent<RollMgr>().numOfEvents == RollManager.GetComponent<RollMgr>().ctr)
            {
      
                // display_text.text = "Lesson finished. Select another lesson to continue";
                loaded = false;
               // AudioManager.GetComponent<AudioManager>().StopRhythm(); //finish rhythm too
                seqctr++;
                CanReload = true; //this is for the 4B 4B 4P 4P sequence

                //fresh restart
                //ClearSpawns();
               Invoke("ClearSpawns", 0.56f);
                //RollManager.GetComponent<RollMgr>().ctr = 0;
                //LoadSequence(modeValue, lessonValue, guidanceValue, 0);
                //release presses here 
                //RollManager.GetComponent<RollMgr>().ReleaseAllPresses();


                //then immediately reload
                if (seqctr < 4 && CanReload)
                {
                    //restart some numbers here to ensure repeat 
                    RollManager.GetComponent<RollMgr>().ctr = 0;
                    CanReload = false;
                    if (seqctr >= 2)
                    {
                        Invoke("TryYourself", 1f);
                        //LoadSequence(3, lessonValue, guidanceValue, vizindex); //should always be zero
                    }
                    else
                    {
                        Invoke("WatchAndLearn", 1f);
                        //LoadSequence(modeValue, lessonValue, guidanceValue, vizindex); //should always be zero
                    }
                }//end can reload
            }  //end if loaded check

        }//end watch and learn mode 1

        //if (modeValue == 2) //test mode
        //{

        //}//end test mode

        else if (modeValue == 4) //compose mode 
        {

            //check the lesson
            //if it is variations then we just repeat once
            if (lessonValue == 4) //variations is lesson 4
            {
                //we just load it once and thats it 
                if (loaded && RollManager.GetComponent<RollMgr>().numOfEvents == RollManager.GetComponent<RollMgr>().ctr && seqctr < 1)
                {
                    // display_text.text = "Lesson finished. Select another lesson to continue";
                    loaded = false;
                    //AudioManager.GetComponent<AudioManager>().StopRhythm(); //finish rhythm too
                    seqctr++;
                    CanReload = true; //this is for the 4B 4B 4P 4P sequence

                    //fresh restart
                    // ClearSpawns();
                    Invoke("ClearExceptMetronome", 0.56f);
                    // ClearExceptMetronome();
                    //RollManager.GetComponent<RollMgr>().ctr = 0;
                    // LoadSequence(3, lessonValue, guidanceValue, vizindex);

                    //    //then immediately reload
                    //    if (CanReload)
                    //    {
                    //        //restart some numbers here to ensure repeat 
                    //        RollManager.GetComponent<RollMgr>().ctr = 0;
                    //        CanReload = false;

                    //        LoadSequence(3, lessonValue, guidanceValue, vizindex);

                    //        //if (seqctr >= 2)
                    //        //{
                    //        //    LoadSequence(3, lessonValue, guidanceValue, vizindex); //should always be zero
                    //        //}
                    //        //else
                    //        //{
                    //        //    //loading 01 cos we wanna force the W&L mode without changing actual mode
                    //        //    LoadSequence(1, lessonValue, guidanceValue, vizindex); //should always be zero
                    //        //}//endelse

                    //    }//end can reload
                    //}  //end if loaded check

                    //then immediately reload
                    if (seqctr < 2 && CanReload)
                {
                    //restart some numbers here to ensure repeat 
                    RollManager.GetComponent<RollMgr>().ctr = 0;
                    CanReload = false;
                    if (seqctr >= 2)
                    {
                            Invoke("TryYourself", 1f);
                         //   LoadSequence(3, lessonValue, guidanceValue, vizindex); //should always be zero
                    }
                 //   else Metronome.GetComponent<Metronome>().FourBeatStart();
                        //LoadSequence(1, lessonValue, guidanceValue, vizindex); //should always be zero

                    }//end can reload
            }  //end if loaded check

        }//end check if lesson is variations
            else if (lessonValue == 5) //QA is lesson 5
            {
                //if it is QA we do the 2V 2P 2V 2P
                //practically the same as modeValue 1 but instead of 4b4b4p4p its 4b4p4b4p
                if (loaded && RollManager.GetComponent<RollMgr>().numOfEvents == RollManager.GetComponent<RollMgr>().ctr)
                {
                    // display_text.text = "Lesson finished. Select another lesson to continue";
                    loaded = false;
                  //  AudioManager.GetComponent<AudioManager>().StopRhythm(); //finish rhythm too
                    seqctr++;
                    CanReload = true; //this is for the 4B 4B 4P 4P sequence

                    //fresh restart
                    // ClearSpawns();
                    Invoke("ClearExceptMetronome", 0.56f);
                  //  ClearExceptMetronome();
                    //RollManager.GetComponent<RollMgr>().ctr = 0;
                    //LoadSequence(modeValue, lessonValue, guidanceValue, 0);

                    //    //then immediately reload
                    //    if (seqctr % 2 == 1 && CanReload)
                    //    {
                    //        //restart some numbers here to ensure repeat 
                    //        RollManager.GetComponent<RollMgr>().ctr = 0;
                    //        CanReload = false;
                    //        if (seqctr >= 2)
                    //        {
                    //            LoadSequence(3, lessonValue, guidanceValue, vizindex); //should always be zero
                    //        }
                    //        else
                    //        {
                    //            //loading 01 cos we wanna force the W&L mode without changing actual mode
                    //            LoadSequence(1, lessonValue, guidanceValue, vizindex); //should always be zero
                    //        }//endelse

                    //    }//end can reload
                    //}  //end if loaded check

                    //then immediately reload
                    if (seqctr < 4 && CanReload)
                {
                    //restart some numbers here to ensure repeat 
                    RollManager.GetComponent<RollMgr>().ctr = 0;
                    CanReload = false;
                        if (seqctr >= 2)
                        {
                            Invoke("TryYourself", 1f);
                            //LoadSequence(3, lessonValue, guidanceValue, vizindex); //should always be zero
                        }
                 //       else Metronome.GetComponent<Metronome>().FourBeatStart();
                        //LoadSequence(1, lessonValue, guidanceValue, vizindex); //should always be zero

                    }//end can reload
            }  //end if loaded check

        }//end check if lessonValue == 5 

        }//end check if modevalue == 4


    }//end update()

    //=== function definitions

    //introducing ManageSequence so I dont destroy ManageImpprov
    public void ManageSequence()
    {
        // loaded = true;
        //we do some last changes here based on selection of choices 

        //metronome is always on but to be SOLID we need to determine accompaniement
        GuidanceManager.GetComponent<GuidanceMgr>().DetermineAccompaniement();

        RollManager.GetComponent<RollMgr>().reload = true;
        display_text.text = "Lesson ongoing...";

        //since its constant, set it here - dont change these values ever 
        RollManager.GetComponent<RollMgr>().swingfrequency = 2; //16-24 works best with swing
        RollManager.GetComponent<RollMgr>().fallSpeed = 14.8f; //formerly 16 18 //lower fallspeed means slower -  12 -> ~103bpm, 10 -> 80bpm 11.6 is 92  11.8 is 101
        RollManager.GetComponent<RollMgr>().pixelsPerBeat = 24; //formerly 24

        //initialize harmony index here and then play around 
        harmonyindex = (lessonValue - 1) * 8;
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
        //Debug.Log("lesson Value here is " + lessonValue );

        //we do the file manip here! before we load the files

        //have something that tells us that if its compose mode, we still load modeValue 1
        // only in terms of behaviour
        //then the repeat will take place in Update and will change per lesson
        if (modeValue == 4)
        {
            //increment all harmonuy indeces by 8
            harmonyindex += 8; 
            //still load it as if it is 1 and just manage the repetitions 
            modeValue = 1;
            display_text.text = "Improvise during the gaps!";
            //if compose mode AND variations then load lessonvalue 7 then 
            if (lessonValue == 4)
            {
                lessonValue = 7; //load var compose files instead
            }//end check lesson variations
            else if (lessonValue == 5)
            {
                lessonValue = 8; //load QA files instead
            }//end check lesson qa
        }//check modeValue

        //load compose-specific lessons first before changing the operation thru the modevalue 
        //assign filenames that we will pass
        List<string> vizfilenames = AssignSequence(modeValue, lessonValue, guidanceValue, count);
        List<string> sheetfilenames = AssignSheetFileNames(modeValue, lessonValue, guidanceValue, count);
        vizindex = count;
        string lesson_title;

        ////=== so at this point we know now we gonna load the lesson if 6 or 7 

        //////have something that tells us that if its compose mode, we still load modeValue 1
        ////// only in terms of behaviour
        //////then the repeat will take place in Update and will change per lesson
        //if (modeValue == 4)
        //{
        //    modeValue = 1;
        //    //if compose mode AND variations then load lessonvalue 7 then 
        //    //if (lessonValue == 4)
        //    //{
        //    //    lessonValue = 7; //load var compose files instead
        //    //}//end check lesson variations
        //    //else if (lessonValue == 5)
        //    //{
        //    //    lessonValue = 8; //load QA files instead
        //    //}//end check lesson qa
        //}//check modeValue

        ////the lessons will remain assigned to compose lessons - no!
        ////lesson 4 and 5 will be the same for other modes but
        //// will be different if compose mode!
        ////but this should happen after the string manip values 





        loaded = true;



        //now show the sheetfilename - by calling MusicSheetManager to update notation handler
        MusicSheetMgr.GetComponent<MusicSheetManager>().SetSheetFilename(sheetfilenames[vizindex]); //no minus 1 here
                                                                                                    // Debug.Log("sheet index loaded is " + vizindex);

        //update display lesson regardless of mode comes in two parts
        //part 1 set lesson title
        switch (lessonValue)
        {
            case 1: lesson_title = "Swing"; break;
            case 2: lesson_title = "Sequences"; break;
            case 3: lesson_title = "Motifs"; break;
            case 4: lesson_title = "Variations"; break;
            case 5: lesson_title = "Ques-Ans"; break;
            case 7: lesson_title = "Variations"; break;
            case 8: lesson_title = "Ques-Ans"; break;
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
        display_lesson_ctr = count + 1; // should be count now
        display_lesson.text = display_lesson_ctr + "/" + display_lesson_max + "\n " + lesson_title;

       // Debug.Log("midi file to load is " + vizindex);
      //  Debug.Log("midi file loaded is " + vizfilenames[vizindex]);
     //   harmonyindex = (lessonValue - 1) * 8;
      //  Debug.Log("harmony to assign is " + harmonyindex);
        //now manage everything base on the mode!
        //== dont get confused 1 = w&L, 2 = TEST, 3 = TRY, 4 = COMPOSE 
        if (modeValue == 1) // watch and learn
        {

            RollManager.GetComponent<RollMgr>().Filename = vizfilenames[vizindex];
            RollManager.GetComponent<RollMgr>().InvokeSongManager();
            RollManager.GetComponent<RollMgr>().GenerateMIDIEvents(vizfilenames[vizindex]);
            spawntype = 2;
            RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, spawntype, modeValue);

            if (GuidanceManager.GetComponent<GuidanceMgr>().rhythm == true)
            {

                Invoke("PlayRhythm", 2.6f);
               // PlayRhythm();
                Metronome.GetComponent<Metronome>().metronomestarted = true; 
            }

            if (GuidanceManager.GetComponent<GuidanceMgr>().metronome == true)
            {

                Metronome.GetComponent<Metronome>().FourBeatStart();
            }

            //else if (GuidanceManager.GetComponent<GuidanceMgr>().harmony == true)
            //{
              
            //    //      Metronome.GetComponent<Metronome>().StartMetronome();
            //    Invoke("PlayHarmony", 2.0f);
                
            //}//end checkharmony
            //else
            //{
            //    Metronome.GetComponent<Metronome>().StartMetronome();

            //}//end else

        }//end modeValue 1
        else if (modeValue == 3) //try yourself
        {
            //code is the same just that in Rollmanager it stops the midi events 
            RollManager.GetComponent<RollMgr>().Filename = vizfilenames[vizindex];
            RollManager.GetComponent<RollMgr>().InvokeSongManager();
            RollManager.GetComponent<RollMgr>().GenerateMIDIEvents(vizfilenames[vizindex]);
            spawntype = 2;
            RollManager.GetComponent<RollMgr>().GeneratePianoRoll(improvpink, spawntype, modeValue);
           // Metronome.GetComponent<Metronome>().FourBeatStart();
            //if harmony is enabled then no need to play metronome
            if ((guidanceValue == 4 || guidanceValue == 12))
            {
                AudioManager.GetComponent<AudioManager>().HarmonySelection(lessonValue);
            }//end checkharmony
            else
            {
              //  Metronome.GetComponent<Metronome>().StartMetronome();

            }//end else


        }//end else modevalue 3
        else if (modeValue == 2)//testyourself
        {
            display_text.text = "Your turn, compose on the fly!";
            //if harmony is enabled then no need to play metronome
            //  if (guidanceValue == 4 || guidanceValue == 12)
          // Debug.Log("harmony toggle is " + GuidanceManager.GetComponent<GuidanceMgr>().harmony);
          // Debug.Log("rhythm toggle is " + GuidanceManager.GetComponent<GuidanceMgr>().rhythm);
          // Debug.Log("metronome toggle is " + GuidanceManager.GetComponent<GuidanceMgr>().metronome);
            if (GuidanceManager.GetComponent<GuidanceMgr>().rhythm == true )
            {
                //Metronome.GetComponent<Metronome>().FourBeatStart();
                PlayRhythm();
            }
            else if (GuidanceManager.GetComponent<GuidanceMgr>().harmony == true ) {
                // Metronome.GetComponent<Metronome>().FourBeatStart();
                //Invoke(nameof(AudioManager.GetComponent<AudioManager>().HarmonySelection(lessonValue)), 0f);
              //  Metronome.GetComponent<Metronome>().FourBeatStart();
                Metronome.GetComponent<Metronome>().StartMetronome();
                //PlayHarmony();
                Invoke("PlayHarmony", 2.0f);
                //AudioManager.GetComponent<AudioManager>().HarmonySelection(lessonValue);
            }//end checkharmony

            else if (GuidanceManager.GetComponent<GuidanceMgr>().metronome == true )
            {
                Metronome.GetComponent<Metronome>().StartMetronome();
            }//end if metronome selected
            //else if (guidanceValue == 8 || guidanceValue == 10)
            //{
            //    Metronome.GetComponent<Metronome>().StartMetronome();
            //}

            //play rhythm!
            //else if (GuidanceManager.GetComponent<GuidanceMgr>().rhythm)
            //{
            //    PlayRhythm();
            //}
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
       // Debug.Log("mode value we have is " + modeValue);
        if (modeValue == 1 || modeValue == 3) //mode 1 or 3 doesnt matter, only the coroutine changes
        {
          //  Debug.Log("lesson value we have is " + lessonValue);

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


             //   Debug.Log("guidance value we have is " + guidanceValue);
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
              //  Debug.Log("guidance value we have is " + guidanceValue);

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
              //  Debug.Log("guidance value we have is " + guidanceValue);

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
            //    Debug.Log("guidance value we have is " + guidanceValue);

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
              //  Debug.Log("guidance value we have is " + guidanceValue);

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
        AudioManager.GetComponent<AudioManager>().RhythmAudioSelection(7); //change this one 
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
     //   Debug.Log("Piano roll objects have been cleared");
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

        //stop audio
        AudioManager.GetComponent<AudioManager>().StopHarmony();

        //remove sheets
        MusicSheetMgr.GetComponent<MusicSheetManager>().ClearSheets();

        //stop metronome
        Metronome.GetComponent<Metronome>().StopMetronome();

        //refresh metronome variables
        Metronome.GetComponent<Metronome>().metronomestarted = false;

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

        seqctr = 0;
        harmonyindex = 0; 

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
        ModeManager.GetComponent<ModeMgr>().composetoggle.isOn = false;
    }//end reset all values

    //this is some change sequence function for the buttons
    public void ClearSpawns()
    {
        //=== this is kinda like ResetAllValues but without clearing the selection and
        //we are just moving to the next or previous lessons
        //destroy objects
        RemoveObjectsWithParent("RollManager");

        // RollManager.GetComponent<RollMgr>().ReleaseAllPresses();
        //release all midipresses
        //Invoke("RollManager.GetComponent<RollMgr>().ReleaseAllPresses", 0.56f);

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

        //refresh metronome variables
        Metronome.GetComponent<Metronome>().metronomestarted = false; 

        //clear counter too
        RollManager.GetComponent<RollMgr>().ctr = 0;

        //clear all MIDIevents un played
        RollManager.GetComponent<RollMgr>().noteInfo.Clear();

        //clear hihglights in the keyboard to be safe
        RollManager.GetComponent<RollMgr>().CleanupKeyboard();

        RollManager.GetComponent<RollMgr>().ReleaseAllPresses();

        //release all midipresses
        // Invoke("RollManager.GetComponent<RollMgr>().ReleaseAllPresses", 2.24f);
    }// clear spawns

    //this is some change sequence function for the buttons
    public void ClearExceptMetronome()
    {
        //=== this is kinda like ResetAllValues but without clearing the selection and
        //we are just moving to the next or previous lessons
        //destroy objects
        RemoveObjectsWithParent("RollManager");

        // RollManager.GetComponent<RollMgr>().ReleaseAllPresses();
        //release all midipresses
        //Invoke("RollManager.GetComponent<RollMgr>().ReleaseAllPresses", 0.56f);

        //stop all coroutines
        RollManager.GetComponent<RollMgr>().StopAllCoroutines(); //added this to be sure
        RollManager.GetComponent<RollMgr>().IsMotifPlaying = false;
        RollManager.GetComponent<RollMgr>().IsRhythmPlaying = false;

        //stop audio
        //AudioManager.GetComponent<AudioManager>().StopRhythm();

        //remove sheets
        MusicSheetMgr.GetComponent<MusicSheetManager>().ClearSheets();

        //stop metronome
       // Metronome.GetComponent<Metronome>().StopMetronome();

        //refresh metronome variables
        Metronome.GetComponent<Metronome>().metronomestarted = false;

        //clear counter too
        RollManager.GetComponent<RollMgr>().ctr = 0;

        //clear all MIDIevents un played
        RollManager.GetComponent<RollMgr>().noteInfo.Clear();

        //clear hihglights in the keyboard to be safe
        RollManager.GetComponent<RollMgr>().CleanupKeyboard();

        RollManager.GetComponent<RollMgr>().ReleaseAllPresses();

        //release all midipresses
        // Invoke("RollManager.GetComponent<RollMgr>().ReleaseAllPresses", 2.24f);
    }//end clear except metronome

    //some method to delay calling of harmony
    public void PlayHarmony()
    {
        Debug.Log("Harmony index to be played is " + harmonyindex);
        AudioManager.GetComponent<AudioManager>().HarmonySelection(harmonyindex);
    }

    public void TryYourself()
    {
        LoadSequence(3, lessonValue, guidanceValue, vizindex); //should always be zero
    }//end

    public void WatchAndLearn()
    {
        LoadSequence(modeValue, lessonValue, guidanceValue, vizindex); //should always be zero
    }


}//end class
