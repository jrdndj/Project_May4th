//some portions of this code was inspired from https://github.com/KateSawada/midi_visualizer_tutorial_01/blob/vol01/Assets/Scripts/BarScript.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //added for colors

public class RollScript : MonoBehaviour
{
    //============ ENVIRONMENT RELATED VARIABLES =============/

    //this helps the mapping of keys similar to that midi hardware
    [SerializeField] List<GameObject> pianoKeys = new List<GameObject>();

    //an important element to manage all children of spawns
    [SerializeField] GameObject rollManager;
    //[SerializeField] GameObject piano; //should be the piano lets sees

    //this is to manage spawns
    GameObject[] spawnedBars = new GameObject[keysCount]; //this was original
    //GameObject[] spawnedBars = new GameObject[1000];

    //for the lower position limit
    public GameObject green_line;

    //for the spawn point
    public GameObject spawn_top;

    //for the destory point
    public GameObject destroy_point;

    //for the chord name to display
    //public GameObject chord_name; 
    [SerializeField] private Text display_name;

    //help us check for errors and control the number of spawns
    const int keysCount = 61; //or is it 68?
    public float lowerpositionlimit; //changed to float                              
    public int ctr;  //an internal counter
    public float spawnpoint; //y coord of the spawnpoint
    bool[] isKeyPressed = new bool[keysCount]; //for spawning
    bool[] isKeyHighLighted = new bool[keysCount]; //for error checking
    bool spawnNew = true; //flag to trigger next spawn or not
    bool highlightNow = false; //flag to trigger higlighting
    //some crucial variables
    public int spawnCount = 0;

    //to seperate melody and improv pressing
    //s bool[] melodyToPress = new bool[keysCount];
    bool[] melodyToHighlight = new bool[keysCount];
    //bool[] improvToPress = new bool[keysCount];
    bool[] improvToHighlight = new bool[keysCount];
    //int checkHighlights = 0;
    //bool isHit; 
    // bool isRolling;
    //bool isNext;
    //bool greenIsHit;
    //bool nameChanged; 

    float barSpeed = (float)0.47; //from 0.05 0.65 was ok //0.15 is still too fast

    //for the co routines
    private IEnumerator spawn;
    private IEnumerator roll;

    //=========== COLOR RELATED VARIABLES ==========/
    //these are the color related objects
    public ColorBlock theColor;
    public Button theButton;
    Color32 improvpink = new Color32(255, 150, 234, 255);
    Color32 yellow = Color.yellow;
    Color32 belowpink = new Color32(75, 0, 130, 255);  //this is indigo akshully
    Color32 blues = new Color32(65, 105, 225, 255); // this is for the blues blue

    //=========== CHORD RELATED VARIABLES ==========/

    //still need my enum for black key spawning
    List<int> blacklist = new List<int>() { 1, 3, 6, 8, 10, 13, 15, 18, 20, 22, 25, 27, 30, 32, 34, 37, 39, 42, 44, 46, 49, 51, 54, 56, 58 };

    //systematically first octave white is 0th to 6th, second octave is 7th to 13th etc
    /**
     * octave of 2 index is 0 to 11
     * octave of 3 index is 12 to 23
     * octave of 4 index is 24 to 35
     * octave of 5 index is 36 to 47
     * octave of 6 index is 48 to 59
     * octave of 7 index is only 60
     */
    //list of some white only chords and their chord licks
    //D3 F3 A3 C4 --- D4 F4 A4 C5 - ok mapped! - higher D5 F5 A5 C6
    static List<int> Dmin7Chord = new List<int>() { 14, 17, 21, 24 };
    static List<int> Dmin7ChordTone = new List<int>() { 26, 29, 33, 36, 38, 41, 45, 48 };
    static List<int> Dmin7HalfStep = new List<int>() { 25, 28, 32, 35 };
    static List<int> Dmin7Above = new List<int>() { 28, 31, 35, 38 };

    //C3 E3 G3 B3 --- C4 E4 G4 B4 - ok mapped! - C5 E5 G5 B5
    static List<int> Cmaj7Chord = new List<int>() { 12, 16, 19, 23 };
    static List<int> Cmaj7ChordTone = new List<int>() { 24, 28, 31, 35, 36, 40, 43, 47 };
    static List<int> Cmaj7HalfStep = new List<int>() { 23, 27, 30, 34 };
    static List<int> CMaj7Above = new List<int>() { 26, 29, 33, 36 };

    //G3 B3 D4 F4 --- G4 B4 D5 F5 - ok mapped! 
    static List<int> G7Chord = new List<int>() { 19, 23, 26, 29 };
    static List<int> G7ChordTone = new List<int>() { 31, 35, 38, 41 };

    //D3 F3 G3 B3 --- D4 Fs4 G4 B4 - ok mapped! - D5 Fs5 G5 B5
    static List<int> G43Chord = new List<int>() { 14, 17, 19, 23 };
    static List<int> G43ChordTone = new List<int>() { 26, 29, 31, 35, 38, 41, 43, 47 };
    static List<int> G43HalfStep = new List<int>() { 25, 30, 34 }; //removed 29 here cos of overlap
    static List<int> G43Above = new List<int>() { 28, 33, 36 }; //removed 31 here cos of overlap

    //Amin7 A3 C4 E4 G4 --- A4 C5 E5 G5 - ok mapped!
    static List<int> Amin7Chord = new List<int>() { 21, 24, 28, 31 };
    static List<int> Amin7ChordTone = new List<int>() { 33, 36, 40, 43 };

    //Emin7 E3 G3 B3 D4 --- E4 G4 B4 D5 - ok mapped!
    static List<int> Emin7Chord = new List<int>() { 16, 19, 23, 26 };
    static List<int> Emin7ChordTone = new List<int>() { 28, 31, 35, 38 };

    //Fmaj7Chord F3 A3 C4 E4 --- F4 A4 C5 E 5 - ok mapped!
    static List<int> Fmaj7Chord = new List<int>() { 17, 21, 24, 28 };
    static List<int> Fmaj7ChordTone = new List<int>() { 29, 33, 36, 40 };
    //extended harmonies, simply get last value then +2

    //F7Chord F3 A3 C4 Ds4 --- 
    static List<int> F7Chord = new List<int>() { 17, 21, 24, 27 };

    // Blues Semitone on EScale E4 G4 A4 A#4 B4 D5 E5 A#5
    static List<int> ESemiTone = new List<int>() { 28, 31, 33, 34, 35, 38, 40, 43, 45, 46, 47, 50, 52 };
    static List<int> CSemiTone = new List<int>() { 24, 27, 29, 30, 31, 34, 36, 39, 41, 42, 43, 46, 48 };
    // F4 Gs4 As4 B4 C5 Ds5 F5
    static List<int> FSemiTone = new List<int>() { 29, 32, 34, 35, 36, 39, 41, 44, 46, 47, 48, 51, 53 };
    static List<int> GSemiTone = new List<int>() { 31, 34, 36, 37, 38, 41, 43, 47, 49, 50, 51, 54, 56 };

    //list of mixed chords and their chord licks
    //aka minor chords
    //Cmin7 C3 D#3 G3 As3 -- C4 D#4 G4 As4 - ok mapped!
    static List<int> Cmin7Chord = new List<int>() { 12, 15, 19, 22 };
    static List<int> Cmin7ChordTone = new List<int>() { 24, 27, 31, 34 };

    //C7 C3 E3 G3 A#3 - C4 E4 G4 A#4 - ok mapped! 
    static List<int> C7Chord = new List<int>() { 12, 16, 19, 22 };
    static List<int> C7ChordTone = new List<int>() { 24, 28, 31, 34 };

    //A2 Cs3 E3 G3 - A3 Cs4 E4 G4 - ok mapped! - A4 Cs5 E5 G5
    static List<int> A7Chord = new List<int>() { 9, 13, 16, 19 };
    static List<int> A7ChordTone = new List<int>() { 21, 25, 28, 31, 33, 37, 40, 43 };
    //combined with the chord tone, should only show the halfsteps
    static List<int> A7HalfStep = new List<int>() { 20, 24, 27, 30 };
    static List<int> A7Above = new List<int>() { 23, 26, 29, 33 };

    //====== SOME BLUES IMPROV variables =====
    List<string> BluesChordNames = new List<string>() { "C7", "C7", "C7", "C7",
                                                           "F7", "F7", "C7", "C7",
                                                              "G7", "F7", "C7", "C7",};
    List<List<int>> EBluesScale = new List<List<int>>() { C7Chord, C7Chord, C7Chord, C7Chord,
                                                           F7Chord, F7Chord, C7Chord, C7Chord,
                                                            G7Chord, F7Chord, C7Chord, C7Chord };
    List<List<int>> EBluesImprov = new List<List<int>>() { CSemiTone, CSemiTone, CSemiTone, CSemiTone,
                                                            FSemiTone, FSemiTone, CSemiTone, CSemiTone,
                                                             GSemiTone, FSemiTone, CSemiTone, CSemiTone};

    //sequence 1 
    //sequence Dmin7, G43, Cmaj7,
    //G43 = D F G B
    //chord tones are

    //==== SET 02 chords
    //List<List<int>> ChordList = new List<List<int>>() { Dmin7Chord, G43Chord, Cmaj7Chord };
    //List<List<int>> LickList = new List<List<int>>() { Dmin7ChordTone, G43ChordTone, Cmaj7ChordTone};

    //=== SET 01 chords
    //List<List<int>> ChordList = new List<List<int>>() {Dmin7Chord, G7Chord, Cmaj7Chord };
    //List<List<int>> LickList = new List<List<int>>() {Dmin7ChordTone, G7ChordTone, Cmaj7ChordTone };

    //=== Set 03 chords
    //List<List<int>> ChordList = new List<List<int>>() {Dmin7Chord, G7Chord, Cmaj7Chord, Cmin7Chord, C7Chord };
    //List<List<int>> LickList = new List<List<int>>() {Dmin7ChordTone, G7ChordTone, Cmaj7ChordTone, Cmin7ChordTone, C7ChordTone };

    //==== Set 04 chords c/o Danilo's suggestion
    //List<List<int>> ChordList = new List<List<int>>() { Dmin7Chord, G43Chord, Cmaj7Chord, Fmaj7Chord, Amin7Chord, Dmin7Chord, G7Chord, Cmin7Chord, Amin7Chord, Emin7Chord, Amin7Chord };
    // List<List<int>> LickList = new List<List<int>>() { Dmin7ChordTone, G43ChordTone, Cmaj7ChordTone, Fmaj7ChordTone, Amin7ChordTone, Dmin7ChordTone, G7ChordTone, Cmin7ChordTone, Amin7ChordTone, Emin7ChordTone, Amin7ChordTone };

    //==== SET 05 Chords
    List<string> ChordNames = new List<string>() { "Dmin7", "G7", "Cmaj7", "A7", "Dmin7", "G7", "Cmaj7", "Cmaj7" };
    List<List<int>> ChordList = new List<List<int>>() { Dmin7Chord, G43Chord, Cmaj7Chord, A7Chord, Dmin7Chord, G43Chord, Cmaj7Chord, Cmaj7Chord };
    List<List<int>> LickList = new List<List<int>>() { Dmin7ChordTone, G43ChordTone, Cmaj7ChordTone, A7ChordTone, Dmin7ChordTone, G43ChordTone, Cmaj7ChordTone, Cmaj7ChordTone };
    List<List<int>> HalfStepList = new List<List<int>>() { Dmin7HalfStep, G43HalfStep, Cmaj7HalfStep, A7HalfStep, Dmin7HalfStep, G43HalfStep, Cmaj7HalfStep, Cmaj7HalfStep };
    List<List<int>> StepAboveList = new List<List<int>>() { Dmin7Above, G43Above, CMaj7Above, A7Above, Dmin7Above, G43Above, CMaj7Above, Cmaj7HalfStep };

    //THIS IS PART OF STEP 01
    //this method is to initialize important stuff for the piano roll
    public void SpawnRoll(List<int> indexList, Color32 spawncolor, int spawntype)
    {
        //for debugging purposes 
        int success = 0;
        bool isBlackPrefab = false;

        //set prefabs
        GameObject whitePrefab, blackPrefab;
        whitePrefab = (GameObject)Resources.Load("Prefab/whitekeyprefab");
        blackPrefab = (GameObject)Resources.Load("Prefab/blackkeyprefab");

        //scan through the list of keys to spawn based on type
        for (int i = 0; i < indexList.Count; i++)
        {
            ////immediate add them to their correct highlight list 
            //if (spawntype == 1)
            //{
            //    spawnHighLight.Add(indexList[i]);
            //}
            //else
            //{
            //    improvHighlight.Add(indexList[i]);
            //}

            //get the transform position of the elements
            //it shouldnt matter anyway
            Vector3 keypos = pianoKeys[indexList[i]].transform.localPosition;

            //get the position of the element in indexList
            //if index is in blacklist, then spawn blackPrefab, else whitePrefab
            if (blacklist.Contains(indexList[i])) //change this later on 
            {
                //spawn a blackprefab
                spawnedBars[spawnCount] = Instantiate(blackPrefab);
                //we need this for the colors later
                isBlackPrefab = true;
                //debugging purposes only
                success++;
            }//endif
            else
            {
                //spawn a whiteprefab
                spawnedBars[spawnCount] = Instantiate(whitePrefab);
                //debugging purposes only
                isBlackPrefab = false;
                success++;
            }//end whitePrefabs

            //set parent for proper positioning
            spawnedBars[spawnCount].transform.SetParent(rollManager.transform, true);

            //store information and spawn on location regardless of prefab
            // change size of spawned key
            spawnedBars[spawnCount].transform.localScale = new Vector3(pianoKeys[spawnCount].transform.localScale.x, 1, 1);

            //set color to yellow or pink based on type
            spawnedBars[spawnCount].GetComponent<Image>().color = spawncolor;

            //make colors darker if dark prefab
            if (isBlackPrefab)
            {
                Color darkerColor = new Color();
                darkerColor = (Color)spawncolor * 0.75f;
                spawnedBars[spawnCount].GetComponent<Image>().color = darkerColor;
            }//endisBlackprefabcheck

            //puts spawn in it proper position
            spawnedBars[spawnCount].transform.localPosition = new Vector3(keypos.x, spawnpoint + 50, keypos.z);

            //add colliders
            // spawnedBars[spawnCount].AddComponent<BoxCollider2D>();

            //increase count of spawn cos of serializedfield
            spawnCount++;
            //if (spawntype == 1)
            //{
            //    //remember the melodybars that should be pressed
            //   // melodyToHighLight[indexList[i]] = true;
            //}
        }//endfor iterating loop list
        if (success == 4)
        {
            //Debug.Log("Chord spawned");
            spawnNew = false;
        }

        // yield return spawnNew = false;
        //pass it
        //yield return indexList;

    }//end spawnRoll

    //STEP 02
    //rolls the spawned keys to the greenline
    public void RollKeys()
    {

        //roll the objects spawns downward
        for (int i = 0; i < spawnedBars.Length; i++) //based on the current #
        {
            //if there are bars spawned keep rolling )
            if (spawnedBars[i] != null)
            {
                Vector3 pos = spawnedBars[i].transform.position;
                //var collider = spawnedBars[i].GetComponent<BoxCollider2D>();
                //changed to -= since we need them to go down
                pos.y -= barSpeed;
                spawnedBars[i].transform.position = pos;

                //STEP 03
                //some destroy instructions here

                if ((spawnedBars[i].GetComponent<RectTransform>().localPosition.y - 60) <= green_line.GetComponent<RectTransform>().localPosition.y)
                {
                    //highlight here when they reach the green line
                    highlightNow = true;

                }//endif
                 //but we destroy only when they reach destroy point

                //since we are 2D, we use RectTransform and get the localPosition since we are in real-time
                if ((spawnedBars[i].GetComponent<RectTransform>().localPosition.y - 60) <= destroy_point.GetComponent<RectTransform>().localPosition.y)
                {
                    //destroy then highlight 
                    Destroy(spawnedBars[i]);
                    //highlightNow = true;

                    //but we can spawn something new now
                    spawnNew = true;

                    //add some timing elements here instead

                    spawnCount--;
                    // isHit = true;
                    //then add stuff on improvtoHighlight
                }//endif check contact green point

                //if it reaches the destroy point then destory and spawn 
                //if ((spawnedBars[i].GetComponent<RectTransform>().localPosition.y - 60) <= destroy_point.GetComponent<RectTransform>().localPosition.y)
                //{
                //    //Destroy(spawnedBars[i]);
                //    //reduce spawn count instead of resetting it

                //    //but we can spawn something new now
                //   // spawnNew = true;
                //    //add some timing elements here instead

                //   // spawnCount--;
                //}//destroy point
            }//endif movement
        }//end loop for to generate all spawns
    }//end roll keys 

    //general algorithm has now changed to
    // step 01: spawn chords
    // step 02: start rolling them down until they reach the green light
    // step 03: destroy object upon hitting green light
    // step 04: highlight chords and improvs based on color
    // step 04b: clear spawn and highlight lists 
    // step 05: proceed to next spawn, repeat

    //some auxilliary functions here that we need to call
    public void onNoteOn(int noteNumber, float velocity)
    {
        //do we even need this?? 
        //isKeyPressed[noteNumber] = true;

        //that's it - red if not highlighted 
        //highlights red if key pressed is not a lick or not in the roll
        //if ((!isKeyHighLighted[noteNumber] && !improvToPress[noteNumber]) || !melodyToPress[noteNumber])
        //i just flipped the logic just in case 
        if (isKeyHighLighted[noteNumber] && (melodyToHighlight[noteNumber] || improvToHighlight[noteNumber]))
        {
            pianoKeys[noteNumber].GetComponent<Image>().color = Color.white;
        }//endif
        else
        {
            pianoKeys[noteNumber].GetComponent<Image>().color = Color.red;
        }//endif
         //you need to forget the melodies until a new one comes

        //totally something else
        //if (melodyToHighlight[noteNumber]==true)
        //{
        //    melodyToHighlight[noteNumber] = false;
        //}//end forget melody
        //else it's wrong that simple

    }//endonNoteOn;

    public void onNoteOff(int noteNumber)
    {
        //FIRST! - the key is no longer pressed so set it to false duh
        isKeyPressed[noteNumber] = false;

        ////THEN return to the appropriate color

        ////if key was in lick and was pressed revert back to pink
        ////if (improvToPress[noteNumber] && improvToHighlight[noteNumber])
        ////if key released is in liscklist? how to say this? 
        ////if(LickList[ctr].Contains(noteNumber) && !spawnNew)
        ////if (improvToHighlight[noteNumber] && !melodyToPress[noteNumber] && !spawnNew)
        // if (!isKeyHighLighted[noteNumber])
        // {
        //pianoKeys[noteNumber].GetComponent<Image>().color = Color.black;
        // improvToHighlight[noteNumber] = false; //change to false 
        //}
        if (improvToHighlight[noteNumber] == true)
        {
            pianoKeys[noteNumber].GetComponent<Image>().color = improvpink;
            //==========for blues
            //pianoKeys[noteNumber].GetComponent<Image>().color = blues;
        }
        else if (melodyToHighlight[noteNumber] == true)
        {
            pianoKeys[noteNumber].GetComponent<Image>().color = Color.white;
        }
        else
        {
            pianoKeys[noteNumber].GetComponent<Image>().color = Color.black;
        }

        //just call the highlights again
        //if (highlightNow)
        //{
        //    HighlightLicks(LickList[ctr], improvpink, 2);
        //}
        //if (isKeyHighLighted[noteNumber] && improvToHighlight[noteNumber])
        //{
        //    pianoKeys[noteNumber].GetComponent<Image>().color = improvpink;
        //}

    }//end bars pressed on note off 

    //lights up a group of keys based on the licks 
    public void HighlightLicks(List<int> lickset, Color32 highlightcolor, int spawntype)
    {
        //show all 4 as a for loops
        for (int i = 0; i < lickset.Count; i++)
        {
            //HIGHLIGHT PINK WHAT SHOULD BE PINK NOTHING MORE

            //highlight piano key based on color and spawntype
            pianoKeys[lickset[i]].GetComponent<Image>().color = highlightcolor;

            //store information for onNoteOff
            if (spawntype == 2) //2 if improv
            {
                improvToHighlight[lickset[i]] = true;
            }
            else //1 if melody
            {
                melodyToHighlight[lickset[i]] = true;
            }

            //we update flag for error checking
            isKeyHighLighted[lickset[i]] = true; //this works never remove this

        }//endfor
        //checkHighlights++;
        //return lickset;
    }//endHighlightLicks

    ////lights up a group of keys based on the licks 
    //public List<int> HighlightChords(List<int> chordset)//removed Color second param
    //{
    //    //show all 4 as a for loop
    //    for (int i = 0; i < chordset.Count; i++)
    //    {
    //        pianoKeys[chordset[i]].GetComponent<Image>().color = yellow;
    //    }//endfors
    //    checkHighlights++;
    //    return chordset;
    //}//endHighlightMelodyChords


    //this will be reused to also unhighlight all existing higlights 
    //when objects get destroyed, unhighlight the most recent lickset
    //public void RemoveHighLights(List<int> highlightset)
    //{
    //    //show all 4 as a for loops
    //    for (int i = 0; i < highlightset.Count; i++)
    //    {
    //        //HIGHLIGHT PINK WHAT SHOULD BE PINK NOTHING MORE
    //        //if pressed, show white else show pink
    //        pianoKeys[highlightset[i]].GetComponent<Image>().color = Color.black;
    //        //flag the appropriate flags

    //        //therefore these flags should change also
    //        improvToPress[highlightset[i]] = false; //for error checking of improv
    //        improvToHighlight[highlightset[i]] = false;

    //    }//endfor
    //    //return lickset;
    //}//endremovelicks

    //we also need to remove the checking for correct melody pressed
    //public void RemoveMelodyCheck(List<int> chordset)
    //{
    //    for (int i = 0; i < chordset.Count; i++)
    //    {
    //        melodyToPress[chordset[i]] = false;
    //    }//endfor

    //}//endRemoveMelodyCheck

    //need a cleanup function
    public void CleanupKeyboard()
    {
        //show all 4 as a for loops
        for (int i = 0; i < keysCount; i++)
        {
            pianoKeys[i].GetComponent<Image>().color = Color.black;
        }//endfor
        //return lickset;
    }//endremovelicks

    ////something for collision with green point
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.name == "green")
    //    {
    //        Debug.Log("hit!");
    //        //greenIsHit = true;
    //        //hihglight licks
    //        CleanupKeyboard();
    //        HighlightLicks(ChordList[ctr], yellow);
    //        HighlightLicks(LickList[ctr], improvpink);

    //        //then change to false
    //        greenIsHit = false; 

    //    }
    //}//end OnCollisionEnter

    //start is for initialization 
    void Start()
    {
        //set greenline pos
        //these values are true never change them to transform.Position
        lowerpositionlimit = green_line.transform.localPosition.y;
        spawnpoint = spawn_top.transform.localPosition.y;
        ctr = 0;
        //belowpink = (Color)improvpink * 0.75f;
        //lets try purple

        //start with a clean slate
        ClearMelodies();
        ClearImprovs();
        CleanupKeyboard();

        //some crucial initialisation
        highlightNow = false;
        // isNext = false;

        //STEP 01
        //spawn the first in the sequence
        display_name.text = ChordNames[ctr];
        SpawnRoll(ChordList[ctr], yellow, 1);
        SpawnRoll(LickList[ctr], improvpink, 2);
        //======== if we wanna spawn blues we use
        //display_name.text = BluesChordNames[ctr];
        //SpawnRoll(EBluesScale[ctr], yellow, 1);


        //this cleans up everything at start
        for (int i = 0; i < 61; i++)
        {
            isKeyPressed[i] = false;
            //dont put anything here anymore it fucks up the configurations
        }

    }//end start function

    // Update is called once per frame
    void Update()
    {
        RollKeys();
        if (highlightNow)
        {
            CleanupKeyboard();
            //we need to clear previous improvs so they dont stain the keyboard
            ClearMelodies();
            ClearImprovs();

            //then highlight the next batch
            //==== JAZZ IMPROV Variables ====
            HighlightLicks(ChordList[ctr], yellow, 1);
            HighlightLicks(LickList[ctr], improvpink, 2);

            //===== Jazz improv with Halfstep
            //HighlightLicks(HalfStepList[ctr], belowpink, 2);

            //==== Jazz improv with Scale above
            //HighlightLicks(StepAboveList[ctr], belowpink, 3);

            //===== BLUES IMPROV VARIABLES ====
            //HighlightLicks(EBluesScale[ctr], yellow, 1);
            //HighlightLicks(EBluesImprov[ctr], blues, 2);

            //highlightNow 
            highlightNow = false;
        }//end if check hihglight now

        //when its time to trigger spawn and chordlist isnt empty
        if (spawnNew)
        {
            //Some things to control the spawning
            //then increment
            //=====jazz 
            if (ctr < ChordList.Count - 1)
            //if (ctr < EBluesScale.Count - 1) //==== improv
            {
                ctr++;
            }
            //revert back to 0 when over this ensures a loop 
            else
            {
                ctr = 0;
            }

            //use co routine perhaps?
            //coroutine = SpawnRoll(ChordList[ctr], yellow, 1);
            //StartCoroutine(coroutine);

            //======jazz variables
            //spawn new based on recent counter
            display_name.text = ChordNames[ctr];
            SpawnRoll(ChordList[ctr], yellow, 1);
            SpawnRoll(LickList[ctr], improvpink, 2);

            //=======blues variables
            //spawn new based on recent counter
            //display_name.text = BluesChordNames[ctr];
            //SpawnRoll(EBluesScale[ctr], yellow, 1);


            spawnNew = false;

        }//endidSpawnNew

    }//end update function

    public void ClearImprovs()
    {
        for (int i = 0; i < 61; i++)
        {
            improvToHighlight[i] = false;
        }
    }//endclearImprovs

    public void ClearMelodies()
    {
        for (int i = 0; i < 61; i++)
        {
            melodyToHighlight[i] = false;
        }
    }//endclearImprovs

}//endclass
