using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollScript : MonoBehaviour
{
    //all the keys in one set
    [SerializeField] List<GameObject> pianoKeys = new List<GameObject>();

    //an important element to manage all children of spawns
    [SerializeField] GameObject rollManager;

    //some more serialised lists for spawning and destroying
    //for the reverse piano roll
    GameObject[] spawnedBars = new GameObject[keysCount]; // bars linked to the pressed key
    [SerializeField] List<GameObject> rollingBars = new List<GameObject>(); // bars linked to the released key
    const int keysCount = 61; //or is it 68?
    //const int lowerpositionlimit = 0; 

    //still need my enum for black key spawning
    List<int> blacklist = new List<int>() { 1, 3, 6, 8, 10, 13, 15, 18, 20, 22, 25, 27, 30, 32, 34, 37, 39, 42, 44, 46, 49, 51, 54, 56, 58 };

    //this list contains their index in whitelist so we dont need to super hardcode
    //this means I want to return value in the 8th as the index of whitelist

    //systematically first octave white is 0th to 6th, second octave is 7th to 13th etc
    /**
     * octave of 2 index is 0 to 11
     * octave of 3 index is 12 to 23
     * octave of 4 index is 24 to 35
     * octave of 5 index is 36 to 47
     * octave of 6 index is 48 to 59
     * octave of 7 index is only 60
     */

    //list of white only chords and their chord licks
    //D3 F3 A3 C4 --- D4 F4 A4 C5 - ok mapped!
    static List<int> Dmin7Chord = new List<int>() { 14, 17, 21, 24 };
    static List<int> Dmin7ChordTone = new List<int>() { 26, 29, 33, 36 };

    //C3 E3 G3 B3 --- C4 E4 G4 B4 - ok mapped!
    static List<int> Cmaj7Chord = new List<int>() { 12, 16, 19, 23 };
    static List<int> Cmaj7ChordTone = new List<int>() { 24, 28, 31, 35 };

    //G3 B3 D4 F4 --- G4 B4 D5 F5 - ok mapped!
    static List<int> G7Chord = new List<int>() { 19, 23, 26, 29 };
    static List<int> G7ChordTone = new List<int>() { 31, 35, 38, 41 };

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
    List<List<int>> ChordList = new List<List<int>>() { Dmin7Chord, G7Chord, Cmaj7Chord, Cmin7Chord, C7Chord };
    List<List<int>> LickList = new List<List<int>>() { Dmin7ChordTone, G7ChordTone, Cmaj7ChordTone, Cmin7ChordTone, C7ChordTone };

    //some crucial variables
    int spawnCount = 0;

    //for some reason I need the Clone method just to be safe
    GameObject Clone(GameObject obj)
    // reference: https://develop.hateblo.jp/entry/2018/06/30/142319
    {
        var clone = GameObject.Instantiate(obj) as GameObject;
        clone.transform.parent = obj.transform.parent;
        clone.transform.position = obj.transform.position; //changed from LocalPosition
        clone.transform.localScale = obj.transform.localScale;
        return clone;
    }

    //this method is to initialize important stuff for the piano roll
    public void spawnRoll(List<int> indexList)
    {
        int success = 0;

        //set prefabs
        GameObject whitePrefab, blackPrefab;
        whitePrefab = (GameObject)Resources.Load("Prefab/whitekeyprefab");
        blackPrefab = (GameObject)Resources.Load("Prefab/blackkeyprefab");
        for (int i = 0; i < indexList.Count; i++)
        {
            //get the position of the element in indexList
            Vector3 wkpos = pianoKeys[indexList[i]].transform.position;
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
            //set the height first then put them into position
            spawnedBars[spawnCount].transform.localScale = new Vector3(pianoKeys[spawnCount].transform.localScale.x, 5, 0);
            spawnedBars[spawnCount].transform.position = new Vector3(pianoKeys[indexList[i]].transform.position.x-40, 120-(spawnedBars[spawnCount].transform.localScale.y/2), 0);
            spawnCount++;

            //transfer them to the serialize list so they move
            rollingBars.Add(Clone(spawnedBars[spawnCount]));
        }//endfor iterating loop list
        if (success == 4)
        {
            Debug.Log("Chord spawned");
        }

        rollKeys();
    }//end spawnRoll

    public void rollKeys()
    {
        float barSpeed = (float)0.65; //from 0.05 0.15 was ok 
        float lowerpositionlimit = (float)700; //changed from 100 
        //released keys
        for (int i = rollingBars.Count - 1; i >= 0; i--)
        {
            Vector3 pos = spawnedBars[i].transform.position;

            // destroy bars when it reached upperPositionLimit
            if (pos.y + (spawnedBars[i].transform.localScale.y / 2) > lowerpositionlimit)
            {
                Destroy(spawnedBars[i]);
                rollingBars.RemoveAt(i);
            }//endifbarsreleased
            else
            {
                pos.y += barSpeed * 2; //changed from 2
                spawnedBars[i].transform.position = pos;
            }//end else bars released
        }//end checking of all bars

    }//end roll keys 

    //this method highlights

    // Start is called before the first frame update
    void Start()
    {
        //get the first batch and spawn them
        spawnRoll(C7Chord);
        //or could be an element in the list of Chords 

    }//end start function

    // Update is called once per frame
    void Update()
    {
        //move all that spawned, spawn a new one if necessar

        //have a function here to highlight improv licks


    }//end update function

    //some auxilliary functions here that we need to call

    //lights up a group of keys based on the licks 
    public List<int> HighlightLicks(List<int> lickset, Color32 improvpink)
    {
        //fetch set of notes and their counterpart notenumbers in midi
        // then call the respective lighting buttons

        //====== highlight chord tones next in blue 

        //show all 4 as a for loop
        for (int i = 0; i < lickset.Count; i++)
        {
            //    theButton = whiteKeys[lickset[i]].GetComponent<Button>();
            //    theColor = whiteKeys[lickset[i]].GetComponent<Button>().colors;
            //    // theColor.highlightedColor = new Color32(255, 150, 234, 255); //pink color
            //    theColor.highlightedColor = improvpink;
            //    whiteKeys[lickset[i]].GetComponent<Image>().color = theColor.highlightedColor;
        }//endfor
        return lickset;
    }//endHighlightLicks
}
