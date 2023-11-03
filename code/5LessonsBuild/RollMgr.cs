// this shall be the new rollscript
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
    // [SerializeField] GameObject AudioManager;

    GameObject noteObject;

    public GameObject green_line; //formerly 0 -85 0
    public GameObject spawnpoint; //for the spawnpoint
                                  // Move the noteObject to the destroy point (final Y position) based on note.Time
    float destroyY;

    public static RollMgr Instance;

    //this helps the mapping of keys similar to that midi hardware
    [SerializeField] List<GameObject> pianoKeys = new List<GameObject>();

    //this lets us know which keys are black 
    List<int> blacklist = new List<int>() { 1, 3, 6, 8, 10, 13, 15, 18, 20, 22, 25, 27, 30, 32, 34, 37, 39, 42, 44, 46, 49, 51, 54, 56, 58 };
    bool isBlackPrefab = false;
    bool batchSpawned = false;

    int numOfSpawns = 0;
    int spawnCount = 0;

    public float fallSpeed = 100.0f; // Adjust this to control the speed of falling - was 100

    //storing the first y for the spawning of harmony
    float firstYpos = 0.0f;

    //==== midi related variables
    public static MidiFile midiFile; // MIDI file asset
    public float pixelsPerBeat = 20.0f; // height of one beat in pixels - shouldnt this be 1? 
    public string Filename; // this should be manipulated by ImprovManager

    //public int SelectedSong; //which will be sent to PlayDelayedAudio for Audiomanager

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

        destroyY = green_line.GetComponent<RectTransform>().position.y;



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
    public void GeneratePianoRoll(Color32 spawncolor, int spawntype) //1 for melody, 2 for licks 
    {
        //numOfSpawns = 0; //fresh start
        //piano related configs
        //set prefabs
        GameObject whitePrefab, blackPrefab;
        whitePrefab = (GameObject)Resources.Load("Prefab/whitekeyprefab");
        blackPrefab = (GameObject)Resources.Load("Prefab/blackkeyprefab");



        // midi related configs
        // MidiFile midi = MidiFile.Read(Filename); // Read the MIDI file from ImprovMgr
        MidiFile midi = midiFile;
        Debug.Log("Successfully read " + Filename);

        //routine getting of files

        var notes = midi.GetNotes();
        var tempoMap = midi.GetTempoMap();

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

            //GameObject noteObject;

            spawnCount++;

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

            /**
             * The algorithm is
             * 01 get x position for the key
             * 02 set the y position based on its time when it should appear
             * 03 set the height based on its duration
             * 04 change the scale based on its height
             * 05 adjust to consider the half height of the objects 
             * **/

            //==routine commands for proper game object management

            //set parent after instantiate for proper positioning
            noteObject.transform.SetParent(rollManager.transform, true);

            //the assignment of positions should only take place after the setting of parent

            // Calculate X position in pixels based on note.Time
            float xPosition = pianoKeys[noteNumber].transform.position.x;

            //calculate their position
            //  float yPosition = ((float)noteTime.TotalMicroseconds / 1000000.0f) * pixelsPerBeat; //latest working
            //should be somewhere between 1000000 and 2400000
            float yPosition = ((float)noteTime.TotalMicroseconds / 2400000.0f) * pixelsPerBeat; //testing
            //but we also need to raise the keys to half of its height so see below 

            //=== i still need this but only for harmony or type 1
            //store here the first y position
            if (spawnCount == 1)
            {
                firstYpos = yPosition;
            }

            //this firstyposition becomes an offset for melody type spawns

            float zPosition = pianoKeys[noteNumber].transform.position.z; //should be always 0 or 1 only and based on the piannkey

            // Calculate the height of the object based on note.Duration 
            float noteHeight = (float)noteDuration.TotalMicroseconds / 10000000.0f * pixelsPerBeat;    //height is too much so consider reducing
            //float noteHeight = (float)noteDuration.TotalMicroseconds / 150000.0f;  //test mode

            //get the height of that object
            float objectHeight = noteObject.GetComponent<RectTransform>().rect.height * 2;

            //change the size (localscale) of the object based on the computed height
            noteObject.transform.localScale = new Vector3(1, noteHeight, 1);
            //consider the half of the shape when setting the position
            //get the half of the object - some computation here
            //  noteObject.transform.position = new Vector3(xPosition, spawnpoint.transform.position.y + yPosition + (objectHeight*2), zPosition); // Set the position

            //get the half of the object - some computation here
            // noteObject.transform.position = new Vector3(xPosition, spawnpoint.transform.position.y + yPosition + (objectHeight * 2), zPosition); // latest working
            noteObject.transform.position = new Vector3(xPosition, spawnpoint.transform.position.y + yPosition + objectHeight, zPosition); // changes to test

            //== if type 1, adjust it one more time
            if (spawntype==1)
            {
                noteObject.transform.position = new Vector3(xPosition, spawnpoint.transform.position.y + yPosition + objectHeight + objectHeight, zPosition); // changes to test
            }//end adjust 

            //set color to yellow or pink based on type
            noteObject.GetComponent<Image>().color = spawncolor; //pink for now SOLID it later
            // or if we are calling this again, ddoing two spawns then yeah this can be made simpler 

            //make colors darker if dark prefab
            if (isBlackPrefab)
            {
                Color darkerColor = new Color();
                darkerColor = (Color)improvpink * 0.75f;
                noteObject.GetComponent<Image>().color = darkerColor;
            }//endisBlackprefabcheck

            //now do some computation for the falling


            //should be something like (SpawnScale.rect.height + (SpawnScale.rect.height)))

            // Start the coroutine to make the note fall at a constant speed
           StartCoroutine(FallAtEndOfDuration(noteNumber, noteObject.transform, noteObject.transform.position.y, destroyY - (objectHeight)));
            //it should end on the half  


            //done all routine spawn methods
            numOfSpawns++;

        }//end foreach

        Debug.Log("Reached end of spawning");
        batchSpawned = true; // signal for ImprovManager to rollKeys
        Debug.Log("Total prefabs spawned: " + numOfSpawns);

        //get noteObject count
    }//end generate piano roll

    //== press related scripts

    // == some press related functions
    public void onNoteOn(int noteNumber, float velocity)
    {
        //    default behaviour is show white
        pianoKeys[noteNumber].GetComponent<Image>().color = Color.white;

     

    }//endonNoteOn;

    //when user releases a pressed key as per MIDIScript 
    public void onNoteOff(int noteNumber)
    {
        //    default behaviour is show black upon release
        pianoKeys[noteNumber].GetComponent<Image>().color = Color.black;

   


    }//end OnNoteOff

    //=== logic related scripts


    private IEnumerator FallAtEndOfDuration(int noteNumber, Transform noteTransform, float initialY, float destroyY)
    {
        float elapsedTime = 0;
         float duration = Mathf.Abs(destroyY - initialY) / fallSpeed;// working latest if fallspeed = 100
        //float duration = 200.00f; //testing 
        //Debug.Log("speed is now " + duration);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            noteTransform.position = new Vector3(noteTransform.position.x, Mathf.Lerp(initialY, destroyY, t), noteTransform.position.z);
            elapsedTime += Time.deltaTime; //if fallspeed is high what happens
           // Debug.Log("elapsed time is " + elapsedTime);
            yield return null;
        }

        noteTransform.position = new Vector3(noteTransform.position.x, destroyY, noteTransform.position.z);

    }//end fallatendofduration

    /**
     * somewhere inside rollkeys when they touch the green line for the first time, 
     * we call   //Invoke("PlayDelayedAudio", 0.0f);
        //                    // Invoke("PlayDelayedAudio", 0.5f); //for the one that starts with the rest
    * which calls this function
    *
     * **/
    private void PlayDelayedAudio()
    {

        //decentralising to AudioManager game object 
        //  AudioManager.GetComponent<AudioManager>().ChangeAudioSelection(1); //change this one 
        //Instance.MotifToPlay.Play();
    }//end PlayDelayed Audio();

    //highlight related function
}//end RollMgr
