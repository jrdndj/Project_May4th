//this script was from Kate Sawada's tutorial
// more info https://github.com/KateSawada/midi_visualizer_tutorial_01/blob/vol01/Assets/Scripts/BarScript.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    const int keysCount = 68; //changed from 88
    public int mode; //can be 1, 2, or 3

    //color variables
    Color32 improvpink = new Color32(255, 150, 234, 255);
    Color32 green = Color.green;
    Color32 yellow = Color.yellow;

    //serialized field for the greenline
    [SerializeField] GameObject GreenLine;

    //adding a enum check for the piano keys 
    List<int> blacklist = new List<int>() { 1, 3, 6, 8, 10, 13, 15, 18, 20, 22, 25, 27, 30, 32, 34, 37, 39, 42, 44, 46, 49, 51, 54, 56, 58 };
    List<int> whitelist = new List<int>() { 0, 2, 4, 5, 7, 9, 11, 12, 14, 16, 17, 19, 21, 23, 24, 26, 28, 29, 31, 33, 35, 36, 38, 40, 41, 43, 45, 47, 48, 50, 52, 53, 55, 57, 59, 60 };
    //systematically first octave white is 0th to 6th, second octave is 7th to 13th etc
    /**
     * octave of 2 index is 0 to 6
     * octave of 3 index is 7 to 13
     * octave of 4 index is 14 to 20
     * octave of 5 index is 21 to 27
     */

    //this list contains their index in whitelist so we dont need to super hardcode
    //this means I want to return value in the 8th as the index of whitelist

    //list of stuff
    //static List<int> Dmin7Chord = new List<int>() { 8, 10, 12, 14 };
    //static List<int> Dmin7ChordTone = new List<int>() { 15, 17, 19, 20 };
    //static List<int> Cmaj7Chord = new List<int>() { 7, 9, 11, 13 };
    //static List<int> Cmaj7ChordTone = new List<int>() { 14, 16, 18, 19 };
    //static List<int> G7Chord = new List<int>() { 11, 13, 15, 17 };
    //static List<int> G7ChordTone = new List<int>() { 18, 20, 22, 24 };
    //static List<int> Amin7Chord = new List<int>() { 5, 7, 9, 11 };
    //static List<int> Amin7ChordTone = new List<int>() { 12, 14, 16, 18 };
    //static List<int> Emin7Chord = new List<int>() { 9, 11, 13, 15 };
    //static List<int> Emin7ChordTone = new List<int>() { 16, 18, 20, 22 };
    //static List<int> Fmaj7Chord = new List<int>() { 10, 12, 14, 16 };
    //static List<int> Fmaj7ChordTone = new List<int>() { 17, 19, 21, 22 };

    //this is special list for real time presses only 
    static List<int> PressedList = new List<int>();

    //another special list just for the secondarily raised octave
    static List<int> SecondOctave = new List<int>();

    //extended harmonies, simply get last value then +2

    ////mother list that we can control later on
    //List<List<int>> ChordList = new List<List<int>>() { Dmin7Chord, Cmaj7Chord, G7Chord, Amin7Chord, Emin7Chord, Fmaj7Chord };
    //List<List<int>> LickList = new List<List<int>>() { Dmin7ChordTone, Cmaj7ChordTone, G7ChordTone, Amin7ChordTone, Emin7ChordTone, Fmaj7ChordTone };


    //so we can map each key in the virtual piano to the key
    [SerializeField] List<GameObject> whiteKeys = new List<GameObject>();
    [SerializeField] List<GameObject> blackKeys = new List<GameObject>();

    [SerializeField] GameObject barManager;

    //for the reverse piano roll
    GameObject[] barsPressed = new GameObject[keysCount]; // bars linked to the pressed key
    [SerializeField] List<GameObject> barsReleased = new List<GameObject>(); // bars linked to the released key

    ////for the proper piano roll
    //GameObject[] barstoSpawn = new GameObject[keysCount]; //in the end we just need 68
    //[SerializeField] List<GameObject> barsSpawned = new List<GameObject>();


    //add a collected of higlighted bars but we just need their indices
    //[SerializeField] List<int> barsHighlighted = new List<int>();

    bool[] isKeyPressed = new bool[keysCount];

    float barSpeed = (float)0.65; //from 0.05 0.15 was ok 
    float upperPositionLimit = (float)700; //changed from 100

    //these are the color related objects
    public ColorBlock theColor;
    public Color alpha;
    public Button theButton;

    int chordctr = 0;
    int lickctr = 0;

    // Start is called before the first frame update
    void Start()
    {
        //randomize function for next chord tone
        var rand = UnityEngine.Random.Range(0, 6); //there are 6 in the list

        mode = 1;

        //both mode 1 and 2 begin with the standard chord Dmin7
        if (mode == 1)
        {
            //  HighlightChords(ChordList[0], yellow);
            chordctr++;
            //  Debug.Log("mode 1");
        }
        if (mode == 2)
        {
            //   HighlightChords(ChordList[0], yellow);
            //  HighlightLicks(LickList[0], improvpink);
            //  chordctr++;
            //  lickctr++;
            //   Debug.Log("mode 2");
        }
        if (mode == 3)
        {
            //   HighlightLicks(LickList[0], improvpink);
            //   lickctr++;
            //  Debug.Log("mode 3");
        }
        if (mode == 4)
        {
            //add secondoctave to list to initialize
            //   LickList.Add(SecondOctave);
            //    HighlightLicks(LickList[LickList.Count - 1], improvpink); //get the latest one 
        }//this is for the secondary octave based on press
        //control condition

        //call HighlightLicks here
        //HighlightChords(ChordList[rand]);
        // HighlightLicks(LickList[rand]);

        //for the midi related 
        for (int i = 0; i < 68; i++)
        {
            // initialize: keys are not pressed
            isKeyPressed[i] = false;
            barsPressed[i] = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //randomize function for next chord tone
        var rand = UnityEngine.Random.Range(0, 6); //there are 6 in the list s

        int allkeyscleared = 0;

        ////this moves the keys in the barstoSpawn list
        //for (int i = barsSpawned.Count - 1; i >= 0; i--)
        //{
        //    Vector3 pos = barstoSpawn[i].transform.position;
        //    //their half starts at the upper limit 
        //    pos.y = upperPositionLimit+(barstoSpawn[i].transform.localScale.y / 2); //all bars spawn at the top

        //    // destroy bars when it reached the greenline y position
        //    if (pos.y + (barstoSpawn[i].transform.localScale.y / 2) < GreenLine.transform.position.y)
        //    {
        //        Destroy(barstoSpawn[i]);
        //      barsPressed.RemoveAt(i); 
        //    }//endifbarsreleased
        //    else
        //    {
        //        pos.y -= barSpeed * 2; //changed from 2
        //        barstoSpawn[i].transform.position = pos;
        //    }//end else bars released

        //}//endmove barspressed

        //from this line below  are the reverse piano roll commands  ========
        //for (int i = 0; i < 68; i++)
        //{
        //    if (isKeyPressed[i] && barsPressed[i] != null)
        //    {
        //        Vector3 scale = barsPressed[i].transform.localScale;
        //        scale.y += barSpeed * 2; //changed from *2 
        //        barsPressed[i].transform.localScale = scale;
        //        Vector3 pos = barsPressed[i].transform.position;
        //        pos.y += barSpeed;
        //        barsPressed[i].transform.position = pos;
        //    }
        //}//endforiskeyPressed

        ////released keys
        //for (int i = barsReleased.Count - 1; i >= 0; i--)
        //{
        //    Vector3 pos = barsReleased[i].transform.position;

        //    // destroy bars when it reached upperPositionLimit
        //    if (pos.y + (barsReleased[i].transform.localScale.y / 2) > upperPositionLimit)
        //    {
        //        Destroy(barsReleased[i]);
        //        barsReleased.RemoveAt(i);
        //    }//endifbarsreleased
        //    else
        //    {
        //        pos.y += barSpeed * 2; //changed from 2
        //        barsReleased[i].transform.position = pos;
        //    }//end else bars released
        //}//end checking of all bars

        //above this line are the reverse piano roll commands  ========

        //check all keys
        //  allkeyscleared = checkAllKeysIfWhite();

        //show different suggestive higlights here 
        //if (allkeyscleared == 0) //0 means all keys are white
        //{

        //    //then we can call another highlight
        //    //HighlightChords(ChordList[rand]);
        //    //HighlightLicks(LickList[rand]);
        //    if (mode == 1 && chordctr < ChordList.Count)
        //    {
        //        HighlightChords(ChordList[chordctr], yellow);
        //        chordctr++;

        //    }
        //    if (mode == 2 && chordctr < ChordList.Count && lickctr < LickList.Count)
        //    {
        //        HighlightChords(ChordList[chordctr], yellow);
        //        HighlightLicks(LickList[lickctr], improvpink);
        //        chordctr++;
        //        lickctr++;
        //    }
        //    if (mode == 3 && lickctr < LickList.Count)
        //    {
        //        HighlightLicks(LickList[lickctr], improvpink);
        //        lickctr++;
        //    }
        //    if (mode == 4)//no need for additional criteria since this will be unique
        //    {
        //        HighlightLicks(LickList[LickList.Count - 1], improvpink); //get the latest one 
        //    }//end of by press mode improv suggestion

        //    if (chordctr == 4) chordctr = 0;
        //    if (lickctr == 4) lickctr = 0;


        //}//endcheckkeyswhite
    }//end on note off

    //public void spawnKeys(string noteName, int noteCtr)
    //{

    //    int noteNumber = 40;
    //    Debug.Log(noteName + " should spawn here bruh ");
    //    //have some processing here to receive the note then assign it to its x coord
    //    //sets the spawn to match that of the gameobject Bar in resources
    //    GameObject barPrefab;
    //    barPrefab = (GameObject)Resources.Load("Prefab/Bar");
    //    barstoSpawn[noteNumber] = Instantiate(barPrefab);

    //    //now get theem to spawn on the XCord of the based on white/blackkeys
    //    //these spawn at the top
    //    //barstoSpawn[noteNumber].transform.position = new Vector3(whiteKeys[noteNumber].transform.localPosition.x, upperPositionLimit, 0);
    //}//end spawnKeys

    public void onNoteOn(int noteNumber, float velocity)
    {
        // clearfy that the key is pressed
        isKeyPressed[noteNumber] = true;

        //Vector3 wkpos = whiteKeys[noteNumber].transform.position;
        //Debug.Log("noteNumber is " + noteNumber);

        //xcord should be x of whitekeys
        //barsPressed[noteNumber].transform.position = new Vector3(whiteKeys[noteNumber].transform.localPosition.x, 0, 0);

        //should i comment these too?
        // create bar object
        //GameObject barPrefab;
        // barPrefab = (GameObject)Resources.Load("Prefab/Bar");
        // barsPressed[noteNumber] = Instantiate(barPrefab);
        //should instantiate it on the Xcoord of the gameobject

        //we are now spawning at the local position of the button in the scene
        if (blacklist.Contains(noteNumber)) //we have this info on the note pressed
        {
            //use blackKeys BUT get their index
            //blacklist.IndexOf(noteNumber); //and use this as blackKey index
            //  barsPressed[noteNumber].transform.position = new Vector3(blackKeys[blacklist.IndexOf(noteNumber)].transform.position.x, 0, 0); //used position
            //uncomment line below if things fuck up 
            //  barsPressed[noteNumber].transform.SetParent(barManager.transform, true);

            //do these even matter? lets remove later
            theButton = blackKeys[blacklist.IndexOf(noteNumber)].GetComponent<Button>();
            theColor = blackKeys[blacklist.IndexOf(noteNumber)].GetComponent<Button>().colors;
            theColor.pressedColor = Color.white;
            blackKeys[blacklist.IndexOf(noteNumber)].GetComponent<Image>().color = Color.white;
            //changed to red unless they are correct
        }//endifblackkeys
        else
        {   //use whiteKeys
            //we shouldnt put -36
            //  barsPressed[noteNumber].transform.position = new Vector3(whiteKeys[whitelist.IndexOf(noteNumber)].transform.position.x, 0, 0);

            //uncomment line below if things fuck up 
            //   barsPressed[noteNumber].transform.SetParent(barManager.transform, true);

            //color changing methods
            theButton = whiteKeys[whitelist.IndexOf(noteNumber)].GetComponent<Button>();
            theColor = whiteKeys[whitelist.IndexOf(noteNumber)].GetComponent<Button>().colors;
            theColor.pressedColor = Color.white;
            whiteKeys[whitelist.IndexOf(noteNumber)].GetComponent<Image>().color = Color.white;

        }//endwhitekeys
    }//end onNoteOn

    public void onNoteOff(int noteNumber)
    {
        //also change colors of the button when released
        if (blacklist.Contains(noteNumber))
        {
            //use blackKeys BUT get their index
            //blacklist.IndexOf(noteNumber); //and use this as blackKey index
            //barsPressed[noteNumber].transform.position = new Vector3(blackKeys[blacklist.IndexOf(noteNumber)].transform.position.x, 0, 0);
            // barsPressed[noteNumber].transform.SetParent(barManager.transform, true);

            //color changing methods
            //color related blocks
            //higlightkey
            //get button information for color transformation
            theButton = blackKeys[blacklist.IndexOf(noteNumber)].GetComponent<Button>();
            theColor = blackKeys[blacklist.IndexOf(noteNumber)].GetComponent<Button>().colors;
            theColor.pressedColor = Color.white;
            blackKeys[blacklist.IndexOf(noteNumber)].GetComponent<Image>().color = Color.black;

        }
        else
        {   //use whiteKeys
            //we shouldnt put -36
            //barsPressed[noteNumber].transform.position = new Vector3(whiteKeys[whitelist.IndexOf(noteNumber)].transform.position.x, 0, 0);
            //barsPressed[noteNumber].transform.SetParent(barManager.transform, true);

            //color changing methods
            theButton = whiteKeys[whitelist.IndexOf(noteNumber)].GetComponent<Button>();
            theColor = whiteKeys[whitelist.IndexOf(noteNumber)].GetComponent<Button>().colors;
            theColor.pressedColor = Color.white;
            whiteKeys[whitelist.IndexOf(noteNumber)].GetComponent<Image>().color = Color.black;

        }//endcolorwhitekeys

        //  barsReleased.Add(Clone(barsPressed[noteNumber]));
        //  DestroyImmediate(barsPressed[noteNumber]);
        //  isKeyPressed[noteNumber] = false;
    }//end bars pressed on note off 

    //lights up a group of keys based on the licks 
    public List<int> HighlightChords(List<int> chordset, Color32 color)
    {
        //show all 4 as a for loop
        for (int i = 0; i < chordset.Count; i++)
        {
            theButton = whiteKeys[chordset[i]].GetComponent<Button>();
            theColor = whiteKeys[chordset[i]].GetComponent<Button>().colors;
            //theColor.highlightedColor = Color.yellow;
            theColor.highlightedColor = color;
            whiteKeys[chordset[i]].GetComponent<Image>().color = color;
        }//endfors
        return chordset;
        //====== highlight chord tones next in blue 
    }//endHighlightMelodyChords

    //lights up a group of keys based on the licks 
    public List<int> HighlightLicks(List<int> lickset, Color32 improvpink)
    {
        //fetch set of notes and their counterpart notenumbers in midi
        // then call the respective lighting buttons

        //====== highlight chord tones next in blue 

        //show all 4 as a for loop
        for (int i = 0; i < lickset.Count; i++)
        {
            theButton = whiteKeys[lickset[i]].GetComponent<Button>();
            theColor = whiteKeys[lickset[i]].GetComponent<Button>().colors;
            // theColor.highlightedColor = new Color32(255, 150, 234, 255); //pink color
            theColor.highlightedColor = improvpink;
            whiteKeys[lickset[i]].GetComponent<Image>().color = theColor.highlightedColor;
        }//endfor
        return lickset;
    }//endHighlightLicks

    GameObject Clone(GameObject obj)
    // reference: https://develop.hateblo.jp/entry/2018/06/30/142319
    {
        var clone = GameObject.Instantiate(obj) as GameObject;
        clone.transform.parent = obj.transform.parent;
        clone.transform.position = obj.transform.position; //changed from LocalPosition
        clone.transform.localScale = obj.transform.localScale;
        return clone;
    }

    public int checkAllKeysIfWhite()
    {
        int clear = 0;
        //this is scanning through the white list and changing them to white 
        for (int i = 0; i < 36; i++)
        {
            theButton = whiteKeys[i].GetComponent<Button>();
            theColor = whiteKeys[i].GetComponent<Button>().colors;
            if (whiteKeys[i].GetComponent<Image>().color != Color.white)
            {
                clear++;
            }//endif
        }//endfor white keys

        //add the same method but for black keys here
        for (int i = 0; i < 25; i++)
        {
            theButton = blackKeys[i].GetComponent<Button>();
            theColor = blackKeys[i].GetComponent<Button>().colors;
            if (blackKeys[i].GetComponent<Image>().color != Color.white)
            {
                clear++;
            }//endif
        }//endfor white keys

        return clear;
    }//endcheck all checks if white

    //im not sure this function does really anything
    //public void getHighlightedKeys()
    //{
    //    for (int i = 0; i < 36; i++)
    //    {
    //        theButton = whiteKeys[i].GetComponent<Button>();
    //        theColor = whiteKeys[i].GetComponent<Button>().colors;
    //        if (whiteKeys[i].GetComponent<Image>().color != Color.white)
    //        {
    //            // barsHighlighted.Add(i);
    //        }//endif
    //    }//endfor white keys

    //    //add the same method but for black keys here
    //    for (int i = 0; i < 25; i++)
    //    {
    //        theButton = blackKeys[i].GetComponent<Button>();
    //        theColor = blackKeys[i].GetComponent<Button>().colors;
    //        if (blackKeys[i].GetComponent<Image>().color != Color.white)
    //        {
    //            //barsHighlighted.Add(i);
    //        }//endif
    //    }//endfor white keys
    //}//endHiglightedkeys

    //detect the note number then suggest 1-3-5-7 but on the second next octave
    public void raiseOctavePressed(int noteNumber)
    {
        int key = 0;
        //get the notenumber then return the list for highlightlicks

        //we need a control here to see - limit to 60 only  
        if (noteNumber < 28) //anything more than this would cause an exception
        {
            key = 7;
            //add the next immediate octave of it
            //  PressedList.Add(key += noteNumber);
            //keep adding the rest until it has at least 4
            for (int i = 1; i < 4; i++) //yes this should be 1 not 0
            {
                //     PressedList.Add(key + 2);
            }//end for i should have at least 4 new keys 

            //add list to licklist
            // LickList.Add(PressedList);
            // HighlightLicks(LickList[LickList.Count - 1], improvpink);
        }//endof the control limit
        else //anything more than this would cause an exception
        {
            key = 7;
            //add the next immediate octave of it
            //   PressedList.Add(key -= noteNumber);
            //keep adding the rest until it has at least 4
            for (int i = 1; i < 4; i++) //yes this should be 1 not 0
            {
                //      PressedList.Add(key - 2);
            }//end for i should have at least 4 new keys 

            //add list to licklist
            //  LickList.Add(PressedList);
            //  HighlightLicks(LickList[LickList.Count - 1], improvpink); //gets the last element in the list
        }//endof the control limit 
    }//endraiseOctavePressed


    public int secondOctaveRaised(int noteNumber)
    {
        //get the chord
        int key = 14;

        if (noteNumber < 35)
        {
            // SecondOctave.Add(key += noteNumber);

            //then we add the rest in the sequence at least 4 times
            for (int i = 1; i < 4; i++) //yes this should be 1 not 0
            {
                //  SecondOctave.Add(key + 2);
            }
            //  LickList.Add(SecondOctave);
        }//because we jump two more octves thats why
        return SecondOctave.Count;
        //get the chordtone then simply plus 7
    }

}//endclass