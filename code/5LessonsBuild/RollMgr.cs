﻿// this shall be the new rollscript
// we start somethjing clean rollscript will be a legacy copy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

//for songmgr
using System.IO;
using UnityEngine.Networking;
using System;

public class RollMgr : MonoBehaviour
{
    //==== environment related variables

    //an important element to manage all children of spawns
    [SerializeField] GameObject rollManager;
    [SerializeField] GameObject songManager;

    public GameObject green_line; //formerly 0 -85 0
    public GameObject spawnpoint; //for the spawnpoint


    public static RollMgr Instance;

    //this helps the mapping of keys similar to that midi hardware
    [SerializeField] List<GameObject> pianoKeys = new List<GameObject>();

    //this lets us know which keys are black 
    List<int> blacklist = new List<int>() { 1, 3, 6, 8, 10, 13, 15, 18, 20, 22, 25, 27, 30, 32, 34, 37, 39, 42, 44, 46, 49, 51, 54, 56, 58 };
    bool isBlackPrefab = false;
    bool batchSpawned = false;

    int numOfSpawns = 0; 


    //==== midi related variables
    public static MidiFile midiFile; // MIDI file asset
    //public GameObject notePrefab; // Prefab for representing MIDI notes
    //public float pixelsPerBeat = 40.0f; // Width of one beat in pixels
    //public float beatHeight = 25.0f; // Height of one beat in pixels

    public float pixelsPerBeat = 20.0f; // height of one beat in pixels
    //public float beatHeight = 0.1f; // Height of one beat in pixels
    public string Filename; // this should be manipulated by ImprovManager

    public AudioSource audioSource;

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


    }

    // Update is called once per frame
    void Update()
    {

    }

    //===== function definitions here

    /* this means RollMgr must have the song fetching functions from song manager */
    //from SongManager.cs

    // call SongManager methods since RollMgr calling stuff creates exception
    public void InvokeSongManager()
    {
        //give songmaneger the filename to get file 
        midiFile = songManager.GetComponent<SongMgr>().ReadFromFile(Filename);

    }//end InvokeSongManager


    public void StartSong()
    {
        //  audioSource.Play();
    }//end startsong

    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }//end get audio sourcetime



    // this generates piano roll from the file read
    public void GeneratePianoRoll()
    {
        //piano related configs
        //set prefabs
        GameObject whitePrefab, blackPrefab;
        whitePrefab = (GameObject)Resources.Load("Prefab/whitekeyprefab");
        blackPrefab = (GameObject)Resources.Load("Prefab/blackkeyprefab");



               // midi related configs
        // MidiFile midi = MidiFile.Read(Filename); // Read the MIDI file from ImprovMgr
        MidiFile midi = midiFile;
        Debug.Log("Successfully read " + Filename);
        var tempoMap = midi.GetTempoMap();

        var notes = midi.GetNotes(); // Get all the MIDI notes


        foreach (Melanchall.DryWetMidi.Interaction.Note note in notes)
        {
            Debug.Log("Spawning...");

            //configs and declarations first

            var noteTime = note.TimeAs<MetricTimeSpan>(tempoMap);
            // Debug.Log("noteTime " + note.ToString() + "time : " + noteTime);

            var noteDuration = note.LengthAs<MetricTimeSpan>(tempoMap);
            // Debug.Log("noteDuration " +note.ToString() + "duration : " + noteDuration);

            //check the correct number in the piano key array - OK correct 
            int noteNumber = note.NoteNumber - 36; //added offset
                                                   // Debug.Log("noteNumber " + note.ToString() + " note number " + noteNumber);

            //var noteHeight = note.TimeAs<MetricTimeSpan>(tempoMap).Milliseconds; //this tells us how long a prefab should be
            //  Debug.Log("noteHeight " + note.ToString() + " height " + noteHeight);

            //then instantiate object with these parameters

            GameObject noteObject;

            //===== decide on prefab and color here 

            //check using noteNumber if it must be a black or white prefab
            //this affects the shape and color 
            if (blacklist.Contains(noteNumber)) //change this later on 
            {
                // Create a new instance of the prefab and set its position and size
                noteObject = Instantiate(blackPrefab);

                //we need this for the colors
                isBlackPrefab = true;

            }//endif
            else
            {
                // Create a new instance of the prefab and set its position and size
                noteObject = Instantiate(whitePrefab);

                //we need this for the colors
                isBlackPrefab = false;

            }//end whitePrefabs

            //==routine commands for proper game object management

            //set parent after instantiate for proper positioning
            noteObject.transform.SetParent(rollManager.transform, true);

            //the assignment of positions should only take place after the setting of parent
            // Calculate X position in pixels based on note.Time
            float xPosition = pianoKeys[noteNumber].transform.position.x;
            // Debug.Log(note.ToString() + " xPosition is: " + xPosition);

            //calculate their position
            float yPosition = ((float)noteTime.TotalMicroseconds / 1000000.0f) * pixelsPerBeat;
            // Debug.Log(note.ToString() + " yPosition is: " + yPosition);

            float zPosition = pianoKeys[noteNumber].transform.position.z;

            Debug.Log(note.ToString() + " XYZ " + xPosition + ", " + yPosition + ", " + zPosition);


            //calculate the current height
            // Calculate the height of the object based on note.Duration
            float noteHeight = (float)noteDuration.TotalMicroseconds / 10000000.0f * pixelsPerBeat; // Convert microseconds to milliseconds
            //height is too much so consider reducing
            Debug.Log(note.ToString() + " height is: " + noteHeight);



            //=== after some routine commands, we now set their position based on these variables

            noteObject.transform.localScale = new Vector3(1, noteHeight, 1); // Set the size          
            noteObject.transform.position = new Vector3(xPosition, spawnpoint.transform.position.y + yPosition, zPosition); // Set the position



            //set color to yellow or pink based on type
            noteObject.GetComponent<Image>().color = improvpink; //pink for now SOLID it later
            // or if we are calling this again, ddoing two spawns then yeah this can be made simpler 

            //make colors darker if dark prefab
            if (isBlackPrefab)
            {
                Color darkerColor = new Color();
                darkerColor = (Color)improvpink * 0.75f;
                noteObject.GetComponent<Image>().color = darkerColor;
            }//endisBlackprefabcheck

            //now do some computation for the falling
            // Move the noteObject to the destroy point (final Y position) based on note.Time
            //float destroyY = yPosition - (float)noteTime.TotalMicroseconds / 1000.0f * pixelsPerBeat;
            float destroyY = green_line.GetComponent<RectTransform>().position.y;
            StartCoroutine(MoveNoteObject(noteObject.transform, new Vector3(xPosition, destroyY, zPosition), (float)noteDuration.TotalMicroseconds / 1000.0f));


            //done all routine spawn methods
            numOfSpawns++; 

        }//end foreach

        Debug.Log("Reached end of spawning");
        batchSpawned = true; // signal for ImprovManager to rollKeys
        Debug.Log("Total prefabs spawned: " + numOfSpawns);

        //get noteObject count
    }//end generate piano roll

    //== press related scripts

    // Coroutine to smoothly move note objects
    private IEnumerator MoveNoteObject(Transform noteTransform, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0;
        Vector3 initialPosition = noteTransform.position;



        while (elapsedTime < duration)
        {
            noteTransform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        noteTransform.position = targetPosition;
    }



}//end RollMgr
