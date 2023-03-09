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

    //help us check for errors and control the number of spawns
    const int keysCount = 61; //or is it 68?
    public float lowerpositionlimit; //changed to float                              
    public int ctr;  //an internal counter
    public float spawnpoint; //y coord of the spawnpoint
    bool[] isKeyPressed = new bool[keysCount]; //for spawning
    bool[] isKeyHighLighted = new bool[keysCount]; //for error checking
    bool spawnNew; //flag to trigger next spawn or not

    //to seperate melody and improv pressing
    bool[] melodyToPress = new bool[keysCount];
    bool[] improvToPress = new bool[keysCount];
    bool[] improvToHighlight = new bool[keysCount];

    float barSpeed = (float)0.45; //from 0.05 0.65 was ok //0.15 is still too fast

    //=========== COLOR RELATED VARIABLES ==========/
    //these are the color related objects
    public ColorBlock theColor;
    public Button theButton;
    Color32 improvpink = new Color32(255, 150, 234, 255);
    //Color32 green = Color.green;
    Color32 yellow = Color.yellow;

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
    //D3 F3 A3 C4 --- D4 F4 A4 C5 - ok mapped!
    static List<int> Dmin7Chord = new List<int>() { 14, 17, 21, 24 };
    static List<int> Dmin7ChordTone = new List<int>() { 26, 29, 33, 36 };

    //C3 E3 G3 B3 --- C4 E4 G4 B4 - ok mapped!
    static List<int> Cmaj7Chord = new List<int>() { 12, 16, 19, 23 };
    static List<int> Cmaj7ChordTone = new List<int>() { 24, 28, 31, 35 };

    //G3 B3 D4 F4 --- G4 B4 D5 F5 - ok mapped!
    static List<int> G7Chord = new List<int>() { 19, 23, 26, 29 };
    static List<int> G7ChordTone = new List<int>() { 31, 35, 38, 41 };

    //D3 F3 G3 B3 --- G4 B4 D5 F5 - ok mapped!
    static List<int> G43Chord = new List<int>() { 14, 17, 19, 23 };
    static List<int> G43ChordTone = new List<int>() { 26, 29, 31, 35 };

    //static List<int> Amin7Chord = new List<int>() { 5, 7, 9, 11 };
    //static List<int> Amin7ChordTone = new List<int>() { 12, 14, 16, 18 };
    //static List<int> Emin7Chord = new List<int>() { 9, 11, 13, 15 };
    //static List<int> Emin7ChordTone = new List<int>() { 16, 18, 20, 22 };
    //static List<int> Fmaj7Chord = new List<int>() { 10, 12, 14, 16 };
    //static List<int> Fmaj7ChordTone = new List<int>() { 17, 19, 21, 22 };
    //extended harmonies, simply get last value then +2

    //list of mixed chords and their chord licks
    //aka minor chords
    //Cmin7 C3 D#3 G3 As3 -- C4 D#4 G4 As4 - ok mapped!
    static List<int> Cmin7Chord = new List<int>() { 12, 15, 19, 22 };
    static List<int> Cmin7ChordTone = new List<int>() { 24, 27, 31, 34 };

    //C7 C3 E3 G3 A#3 - C4 E4 G4 A#4 - ok mapped! 
    static List<int> C7Chord = new List<int>() { 12, 16, 19, 22 };
    static List<int> C7ChordTone = new List<int>() { 24, 28, 31, 34 };

    //mother list that we can control later on - we can feed this into the spawner
    // List<List<int>> ChordList = new List<List<int>>() { Dmin7Chord, G7Chord, Cmaj7Chord, Cmin7Chord, C7Chord };
    //List<List<int>> LickList = new List<List<int>>() { Dmin7ChordTone, G7ChordTone, Cmaj7ChordTone, Cmin7ChordTone, C7ChordTone };

    //sequence 1 
    //sequence Dmin7, G43, Cmaj7,
    //G43 = D F G B
    //chord tones are

    //List<List<int>> ChordList = new List<List<int>>() { Dmin7Chord, G43Chord, Cmaj7Chord };
    //List<List<int>> LickList = new List<List<int>>() { Dmin7ChordTone, G43ChordTone, Cmaj7ChordTone};

    List<List<int>> ChordList = new List<List<int>>() { Dmin7Chord, G7Chord, Cmaj7Chord };
    List<List<int>> LickList = new List<List<int>>() { Dmin7ChordTone, G7ChordTone, Cmaj7ChordTone };

    //some crucial variables
    public int spawnCount = 0;

    //this method is to initialize important stuff for the piano roll
    public void SpawnRoll(List<int> indexList)
    {
        //for debugging purposes 
        int success = 0;

        //set prefabs
        GameObject whitePrefab, blackPrefab;
        whitePrefab = (GameObject)Resources.Load("Prefab/whitekeyprefab");
        blackPrefab = (GameObject)Resources.Load("Prefab/blackkeyprefab");

        //scan through the list of keys to spawn based on type
        for (int i = 0; i < indexList.Count; i++)
        {
            //get the transform position of the elements
            //it shouldnt matter anyway
            Vector3 keypos = pianoKeys[indexList[i]].transform.localPosition;

            //get the position of the element in indexList
            //if index is in blacklist, then spawn blackPrefab, else whitePrefab
            if (blacklist.Contains(indexList[i])) //change this later on 
            {
                //spawn a blackprefab
                spawnedBars[spawnCount] = Instantiate(blackPrefab);
                //debugging purposes only
                success++;
            }//endif
            else
            {
                //spawn a whiteprefab
                spawnedBars[spawnCount] = Instantiate(whitePrefab);
                //debugging purposes only
                success++;
            }//end whitePrefabs

            //set parent for proper positioning
            spawnedBars[spawnCount].transform.SetParent(rollManager.transform, true);

            //store information and spawn on location regardless of prefab
            // change size of spawned key
            spawnedBars[spawnCount].transform.localScale = new Vector3(pianoKeys[spawnCount].transform.localScale.x, 1, 1);

            //puts spawn in it proper position
            spawnedBars[spawnCount].transform.localPosition = new Vector3(keypos.x, spawnpoint + 50, keypos.z);

            //increase count of spawn cos of serializedfield
            spawnCount++;
            //remember the melodybars that should be pressed
            melodyToPress[indexList[i]] = true;
        }//endfor iterating loop list
        if (success == 4)
        {
            Debug.Log("Chord spawned");
        }

    }//end spawnRoll

    //rolls the spawned keys to the greenline
    public void RollKeys()
    {
        //roll the objects spawns downward
        for (int i = 0; i < spawnedBars.Length; i++) //based on the current #
        {
            //if there are bars spawned keep rolling )
            //if (spawnedBars.Length > 0)
            if (spawnedBars[i] != null)
            {
                Vector3 pos = spawnedBars[i].transform.position;
                //changed to -= since we need them to go down
                pos.y -= barSpeed;
                spawnedBars[i].transform.position = pos;

                //some destroy instructions here
                if (spawnedBars[i].transform.position.y + (spawnedBars[i].transform.localScale.y) < destroy_point.transform.position.y)
                {
                   Destroy(spawnedBars[i]);
                   //spawnedBars[i] = null; //changing it to null clears it so we can do more spawns
                    spawnNew = true;
                    //reset spawncount always when you destroy
                    spawnCount = 0; 
                    Debug.Log("Recent spawns destroyed. Can spawn new batch...");
                }//endif check destory point 
            }//endif movement

        }//end loop for to generate all spawns

    }//end roll keys 

    //start is for initialization 
    void Start()
    {
        //set greenline pos
        //these values are true never change them to transform.Position
        lowerpositionlimit = green_line.transform.localPosition.y;
        spawnpoint = spawn_top.transform.localPosition.y;
        ctr = 0;

        //this prevents the unending loops
        //spawn the first in the sequence
        SpawnRoll(ChordList[ctr]);
        //highlight their partner licks too
        HighlightLicks(LickList[ctr]);

        //cose we just recently spawned
        spawnNew = false;

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
        //if no more spawns spawn new
        if (spawnNew)
        {
            //remove current licks first
            RemoveLicks(LickList[ctr]);
            //remove the correct melody checks too
            RemoveMelodyCheck(ChordList[ctr]);

            //then increment
            if (ctr < ChordList.Count - 1)
            {
                ctr++;
            }
            //revert back to 0 when over
            else
            {
                ctr = 0;
            }

            //spawn new based on recent counter
            SpawnRoll(ChordList[ctr]);
            HighlightLicks(LickList[ctr]);

            //weve spawned again so no need until destroy 
            spawnNew = false;
        }//endif

        //if there are spawns then roll them
        //if(spawnedBars.Length > 0)
        if (!spawnNew)
        {
            //then just keep rolling them
            RollKeys();
        }//endif

    }//end update function

    //some auxilliary functions here that we need to call
    public void onNoteOn(int noteNumber, float velocity)
    {
        //Debug.Log("note number is " + noteNumber);
        isKeyPressed[noteNumber] = true;

        //highlights red if key pressed is not a lick or not in the roll
        if ((!isKeyHighLighted[noteNumber] && !improvToPress[noteNumber]) || !melodyToPress[noteNumber])
        {
            pianoKeys[noteNumber].GetComponent<Image>().color = Color.red;
        }//else treat it like a noteoff

        //highlights white if key pressed is a lick or is in the roll
        if ((isKeyHighLighted[noteNumber] && improvToPress[noteNumber]))
        {
            pianoKeys[noteNumber].GetComponent<Image>().color = Color.white;
            //remind yourself again to rehighlight it
            improvToHighlight[noteNumber] = true;

        }//endif
        if (melodyToPress[noteNumber])
        {
            pianoKeys[noteNumber].GetComponent<Image>().color = Color.white;

        }//endif

    }//endonNoteOn;

    public void onNoteOff(int noteNumber)
    {
        //FIRST! - the key is no longer pressed so set it to false duh
        isKeyPressed[noteNumber] = false;

        //THEN return to the appropriate color

        //if key was in lick and was pressed revert back to pink
        //if (improvToPress[noteNumber] && improvToHighlight[noteNumber])
        //if key released is in licklist? how to say this? 
        if (improvToHighlight[noteNumber])
        {
            pianoKeys[noteNumber].GetComponent<Image>().color = improvpink;
            improvToHighlight[noteNumber] = true; //change to false 
        }//end if
        //else make it black 
        else
        {
            pianoKeys[noteNumber].GetComponent<Image>().color = Color.black;
            //so nothing to change therefore

        }
    }//end bars pressed on note off 

    //lights up a group of keys based on the licks 
    public List<int> HighlightLicks(List<int> lickset)
    {
        //show all 4 as a for loops
        for (int i = 0; i < lickset.Count; i++)
        {
            //HIGHLIGHT PINK WHAT SHOULD BE PINK NOTHING MORE
            //if pressed, show white else show pink
            if (isKeyPressed[lickset[i]])
            {
                pianoKeys[lickset[i]].GetComponent<Image>().color = Color.white;
            }
            else
            {
                pianoKeys[lickset[i]].GetComponent<Image>().color = improvpink;
            }

            //flag the appropriate flags
            improvToPress[lickset[i]] = true; //for error checking of improv 
            isKeyHighLighted[lickset[i]] = true; //this works never remove this

        }//endfor
        return lickset;
    }//endHighlightLicks

    //when objects get destroyed, unhighlight the most recent lickset
    public void RemoveLicks(List<int> lickset)
    {
        //show all 4 as a for loops
        for (int i = 0; i < lickset.Count; i++)
        {
            //HIGHLIGHT PINK WHAT SHOULD BE PINK NOTHING MORE
            //if pressed, show white else show pink

            pianoKeys[lickset[i]].GetComponent<Image>().color = Color.black;
            //flag the appropriate flags

            //therefore these flags should change also
            improvToPress[lickset[i]] = false; //for error checking of improv
            improvToHighlight[lickset[i]] = false;

        }//endfor
        //return lickset;
    }//endremovelicks

    //we also need to uncorrect the correct melody pressed
    public void RemoveMelodyCheck(List<int> chordset)
    {
        for (int i = 0; i < chordset.Count; i++)
        {
            melodyToPress[chordset[i]] = false;
        }//endfor

    }//endRemoveMelodyCheck

}//endclass
