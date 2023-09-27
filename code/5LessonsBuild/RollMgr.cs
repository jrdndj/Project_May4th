// this shall be the new rollscript
// we start somethjing clean rollscript will be a legacy copy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class RollMgr : MonoBehaviour
{
    //==== environment related variables

    //an important element to manage all children of spawns
    [SerializeField] GameObject rollManager;

    //this helps the mapping of keys similar to that midi hardware
    [SerializeField] List<GameObject> pianoKeys = new List<GameObject>();

    //this lets us know which keys are black 
    List<int> blacklist = new List<int>() { 1, 3, 6, 8, 10, 13, 15, 18, 20, 22, 25, 27, 30, 32, 34, 37, 39, 42, 44, 46, 49, 51, 54, 56, 58 };
    bool isBlackPrefab = false; 


    //==== midi related variables
    public static MidiFile midiFile; // MIDI file asset
    public GameObject notePrefab; // Prefab for representing MIDI notes
    public float pixelsPerBeat = 40.0f; // Width of one beat in pixels
    public float beatHeight = 25.0f; // Height of one beat in pixels
    public string Filename; // this should be manipulated by ImprovManager


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //===== function definitions here
    public void GeneratePianoRoll()
    {
        //piano related configs
        //set prefabs
        GameObject whitePrefab, blackPrefab;
        whitePrefab = (GameObject)Resources.Load("Prefab/whitekeyprefab");
        blackPrefab = (GameObject)Resources.Load("Prefab/blackkeyprefab");

        // midi related configs
        MidiFile midi = MidiFile.Read(Filename); // Read the MIDI file from ImprovMgr
        Debug.Log("Successfully read " + Filename);
        var tempoMap = midi.GetTempoMap();

        var notes = midi.GetNotes(); // Get all the MIDI notes

        foreach (Melanchall.DryWetMidi.Interaction.Note note in notes)
        {
            //configs and declarations first 

            var noteTime = note.TimeAs<MetricTimeSpan>(tempoMap);
            var noteDuration = note.LengthAs<MetricTimeSpan>(tempoMap);

            int noteNumber = note.NoteNumber;

            GameObject noteObject;

            //we need the location of the pianoKeys
            Vector3 keypos = pianoKeys[noteNumber].transform.localPosition;

            //==== x position and width doesnt matter regardless of black or white prefab

            // Calculate X position in pixels based on note.Time
            float xPosition = (float)noteTime.TotalMicroseconds / 1000.0f * pixelsPerBeat; // Convert microseconds to milliseconds

            // Calculate Y position (height) based on note.NoteNumber
            float yPosition = (noteNumber - 24) * beatHeight; // Assuming MIDI note C2 is note 24

            // Calculate the width of the object based on note.Duration
            float width = (float)noteDuration.TotalMicroseconds / 1000.0f * pixelsPerBeat; // Convert microseconds to milliseconds


            //===== decide on prefab and color here 

            //check using noteNumber if it must be a black or white prefab
            //this affects the shape and color 
            if (blacklist.Contains(noteNumber)) //change this later on 
            {
                // Create a new instance of the prefab and set its position and size
                noteObject = Instantiate(blackPrefab);
                noteObject.transform.position = new Vector3(keypos.x, yPosition, 0.0f);
                noteObject.transform.localScale = new Vector3(width, beatHeight, 1.0f);


                //spawn a blackprefab
                // spawnedBars[spawnCount] = Instantiate(blackPrefab);
                //we need this for the colors later
                isBlackPrefab = true;
                //debugging purposes only
                //  success++;
            }//endif
            else
            {
                // Create a new instance of the prefab and set its position and size
                noteObject = Instantiate(whitePrefab);
                noteObject.transform.position = new Vector3(keypos.x, yPosition, 0.0f);
                noteObject.transform.localScale = new Vector3(width, beatHeight, 1.0f);
                //spawn a whiteprefab
                //   spawnedBars[spawnCount] = Instantiate(whitePrefab);
                //debugging purposes only
                //  isBlackPrefab = false;
                //  success++;
            }//end whitePrefabs

            

        
            // Create a new instance of the prefab and set its position and size
           // GameObject noteObject = Instantiate(notePrefab);
            //noteObject.transform.position = new Vector3(keypos.x, yPosition, 0.0f);
            //noteObject.transform.localScale = new Vector3(width, beatHeight, 1.0f);
        }
    }//end generate piano roll

}//end RollMgr
