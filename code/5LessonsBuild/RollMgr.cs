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

    public static RollMgr Instance;

    //this helps the mapping of keys similar to that midi hardware
    [SerializeField] List<GameObject> pianoKeys = new List<GameObject>();

    //this lets us know which keys are black 
    List<int> blacklist = new List<int>() { 1, 3, 6, 8, 10, 13, 15, 18, 20, 22, 25, 27, 30, 32, 34, 37, 39, 42, 44, 46, 49, 51, 54, 56, 58 };
    bool isBlackPrefab = false;


    //==== midi related variables
    public static MidiFile midiFile; // MIDI file asset
    //public GameObject notePrefab; // Prefab for representing MIDI notes
    //public float pixelsPerBeat = 40.0f; // Width of one beat in pixels
    //public float beatHeight = 25.0f; // Height of one beat in pixels

    public float pixelsPerBeat = 0.1f; // Width of one beat in pixels
    public float beatHeight = 0.1f; // Height of one beat in pixels
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

            //the solution is found in this link
            // https://www.codeproject.com/Articles/1200014/DryWetMIDI-High-level-processing-of-MIDI-files#tempo

            //call a rescalling algorithm here that considers midi notedata


            var noteTime = note.TimeAs<MetricTimeSpan>(tempoMap);
            //var noteYPos = note.TimeAs<BarBeatTicksTimeSpan>(tempoMap).Beats;
            //Debug.Log("noteYpos values " + note.NoteName.ToString() + " YPos " + noteYPos);


            /*
             * this solution might work so we gotta test this
             *  // Calculate Y position (height) based on note.NoteNumber
            float yPosition = (noteNumber - 24) * pixelsPerBeat; // Assuming MIDI note C2 is note 24

            // Use note.Length directly to determine the width of the object in beats
            float widthInBeats = (float)note.Length;

            // Calculate the width of the object based on widthInBeats and pixelsPerBeat
            float width = widthInBeats * pixelsPerBeat;

            // Create a new instance of the prefab and set its position and size
            GameObject noteObject = Instantiate(notePrefab);
            noteObject.transform.position = new Vector3(xPosition, yPosition, 0.0f);
            noteObject.transform.localScale = new Vector3(width, pixelsPerBeat, 1.0f);
       
             * 
             * **/

            //Debug.Log("noteYPos values " + noteYPos);
            //var noteDuration = note.LengthAs<MetricTimeSpan>(tempoMap);
            // use note.LengthAs<Beats>() but divided by 16 
            var noteDuration = (float) note.Length;
            Debug.Log("noteDuration " +note.ToString() + "duration : " + noteDuration);

            //check their current height 
            var noteHeight = note.TimeAs<MetricTimeSpan>(tempoMap).Milliseconds; //this tells us how long a prefab should be
            Debug.Log("noteHeight " + note.ToString() + " height " + noteHeight);

            //check the correct number in the piano key array - OK correct 
            int noteNumber = note.NoteNumber - 36; //added offset
            Debug.Log("noteNumber " + note.ToString() + " note number " + noteNumber);


            GameObject noteObject;

            //we need the location of the pianoKeys
            //Debug.Log("noteNumber is " + noteNumber);
            Vector3 keypos = pianoKeys[noteNumber].transform.localPosition;

            //==== x position and width doesnt matter regardless of black or white prefab

            // convert duration in divisible multiplers for scale

            // Calculate X position in pixels based on note.Time
            // float xPosition = (float)noteTime.TotalMicroseconds / 1000.0f * pixelsPerBeat; // Convert microseconds to milliseconds
            float xPosition = (float)noteTime.TotalMicroseconds / 1000.0f; // Convert microseconds to milliseconds


           // Debug.Log("x position here is " + xPosition);
            // Calculate Y position (height) based on note.NoteNumber
            //float yPosition = (noteNumber) * beatHeight; // removed -24 here cos offset is already added
            float yPosition = pianoKeys[noteNumber].transform.position.y; // removed -24 here cos offset is already added

            //Debug.Log("yposition here is " + yPosition);

            // Calculate the width of the object based on note.Duration
            //  float width = (float)noteDuration.TotalMicroseconds / 1000.0f * pixelsPerBeat; // Convert microseconds to milliseconds
            //float width = (float)noteDuration.TotalSeconds; //should be seconds but this is a multiplier from the basic prefab size 

           // Debug.Log("width here is " + width);

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

            //== maybe this should be here first 
            //get the actual size and then get the half of it for proper positioning
            RectTransform noteScale = noteObject.GetComponent<RectTransform>();

            //set the refereference size which is the defaults of the prefab not the piano keys 
            noteObject.transform.localScale = new Vector3(noteScale.transform.localScale.x, noteDuration, 1.0f); //scale here is the multipler based on duration

                //adjust the size based on the sizedelta
            noteScale.sizeDelta = new Vector2(noteScale.sizeDelta.x, noteScale.sizeDelta.y ); //removed - barSpeed * 5

            //put it in its y position - x is the keynumber aka yposition, y is the height, so thats x position
            noteObject.transform.localPosition = new Vector3(yPosition, xPosition, keypos.z);

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




            // Create a new instance of the prefab and set its position and size
            // GameObject noteObject = Instantiate(notePrefab);
            //noteObject.transform.position = new Vector3(keypos.x, yPosition, 0.0f);
            //noteObject.transform.localScale = new Vector3(width, beatHeight, 1.0f);
        }//end foreach

        Debug.Log("Reached end of spawning");
    }//end generate piano roll

    //== press related scripts

}//end RollMgr
