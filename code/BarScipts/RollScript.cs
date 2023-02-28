using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollScript : MonoBehaviour
{
    //some important variables here
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
    static List<int> Dmin7Chord = new List<int>() { 8, 10, 12, 14 };
    static List<int> Dmin7ChordTone = new List<int>() { 15, 17, 19, 20 };
    static List<int> Cmaj7Chord = new List<int>() { 7, 9, 11, 13 };
    static List<int> Cmaj7ChordTone = new List<int>() { 14, 16, 18, 19 };
    static List<int> G7Chord = new List<int>() { 11, 13, 15, 17 };
    static List<int> G7ChordTone = new List<int>() { 18, 20, 22, 24 };
    static List<int> Amin7Chord = new List<int>() { 5, 7, 9, 11 };
    static List<int> Amin7ChordTone = new List<int>() { 12, 14, 16, 18 };
    static List<int> Emin7Chord = new List<int>() { 9, 11, 13, 15 };
    static List<int> Emin7ChordTone = new List<int>() { 16, 18, 20, 22 };
    static List<int> Fmaj7Chord = new List<int>() { 10, 12, 14, 16 };
    static List<int> Fmaj7ChordTone = new List<int>() { 17, 19, 21, 22 };

    //this is special list for real time presses only 
    static List<int> PressedList = new List<int>();

    //another special list just for the secondarily raised octave
    static List<int> SecondOctave = new List<int>();

    //extended harmonies, simply get last value then +2

    //mother list that we can control later on
    List<List<int>> ChordList = new List<List<int>>() { Dmin7Chord, Cmaj7Chord, G7Chord, Amin7Chord, Emin7Chord, Fmaj7Chord };
    List<List<int>> LickList = new List<List<int>>() { Dmin7ChordTone, Cmaj7ChordTone, G7ChordTone, Amin7ChordTone, Emin7ChordTone, Fmaj7ChordTone };

    //spawn related variables
    //for the reverse piano roll
    const int keysCount = 68;
    GameObject[] spawnedBars = new GameObject[keysCount]; // bars linked to the pressed key
    //[SerializeField] List<GameObject> barsReleased = new List<GameObject>(); // bars linked to the released key


    //this method is to initialize important stuff for the piano roll
    public void spawnRoll()
    {
        int spawnCount = 0; 
        //set prefabs
        GameObject whitePrefab, blackPrefab;
        whitePrefab = (GameObject)Resources.Load("Prefab/whitekeyprefab");
        blackPrefab = (GameObject)Resources.Load("Prefab/blackkeyprefab");
        //sample instantiate use it later 
        //spawnedBars[spawnCount] = Instantiate(whitePrefab);

        //some logic here sets the sequence of keys for melody
    }

    //this method highlights

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
