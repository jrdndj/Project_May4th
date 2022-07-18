using System.Collections;
using System.Collections.Generic;
//using System; //added for TimeSpan
using System.Linq; //added for toList of chords
using UnityEngine;
//added this to consider drywetmidi libraries
using Melanchall.DryWetMidi.Core; //read and write midi 
using Melanchall.DryWetMidi.Multimedia; //playback and output
using Melanchall.DryWetMidi.Interaction; //to get song info

public class ParseMIDI : MonoBehaviour
{
    public GameObject prefab;
    //putting this here cos it should be here (for spawning the key)

    /*
    * we need the following method if we will rewrite the midi - 
    * especially on the improv part
    * and the adaptive part
    * 
    */
    // midiFile.Write("Assets/MusicXML/New/improvised00.mid");
    //commented for now since this task is out of scope 130720222
    //all newly written as in New folder for the improvised version per user

    //for issues we can revert to
    //https://github.com/melanchall/drywetmidi#getting-started 


    /*
       this method reads the file received from the file browser master
       verifies it by playing it and passing it to the
       next method for further info extraction
     */
    private void ReadFile()
    {

        /*
        * reads a MIDI file from the provided URL or file location
        * 
        * for now we put a temporary midifile that we will be using all througout
        * however it would be great if we integrate it by passing
        * the file from the unity simple file browser master
        * 
        */

        var midiFile = MidiFile.Read("Assets/MusicXML/Intermediate/Mozart - Sonata Facile 1st Movement.mid");

        //commented for now since we dont 
        //to confirm file was read, we play a sound preview for now
       /* using (var outputDevice = OutputDevice.GetByName("Microsoft GS Wavetable Synth"))
        using (var playback = midiFile.GetPlayback(outputDevice))
        {
            playback.Speed = 1.0; //initially 2.0 but we want regular speed
            playback.Play();
        }
       */
        //pass file to know duration and get chord info
        GetSongInfo(midiFile: midiFile);
    }

    /*
        now we need to parse it in musicXML so we can get features such as
        pitch
        timbre
        tempo
        dynamics
        etc

        we do this by getting the duration of the midi 
   */
    void GetSongInfo(MidiFile midiFile)
    {

        // check this library for this solution
        //https://github.com/melanchall/drywetmidi/issues/17
        //TempoMap tempoMap = midiFile.GetTempoMap();
        IEnumerable<Chord> chords = midiFile.GetChords();

    //you will find chord definitions here https://melanchall.github.io/drywetmidi/api/Melanchall.DryWetMidi.Interaction.Chord.html 

        //try to display the chord info in unity log Debug.Log
        // reference here https://stackoverflow.com/questions/32587866/ienumerable-and-console-writeline 
        int Ctr = 0;
        long YScale = 0; // this will be the length 
        string ChordName = null; //adding null fixes the problem
        float ShowUpTime, EndTime; 
        foreach (var row in chords.ToList())
        { 
            Debug.Log(" count: " + Ctr + " chord: " + row + " chord time: " + row.Time + " chord endtime: " + row.EndTime + " chord length: " + row.Length);
            ChordName = row.ToString();
            ShowUpTime = row.Time;
            EndTime = row.EndTime;
            YScale = row.Length; 
            Ctr++;
        }
        //14072022 1746 displaying just chords only so we need the full note info
        // we have all these info in the logs now we just need to pass them around

        //consider writing this into csv so we can compare later on with the timing of user keypress

        //pass actual values to spawnkey instead of chord info
  

        SpawnKey(ChordName, YScale);

    }

    private void SpawnKey(string ChordName, long YScale)
    {

        //reference from this site https://docs.unity3d.com/ScriptReference/Object.Instantiate.html 
        Instantiate(prefab, new Vector3(YScale, 0, 0));
        Debug.Log("Chord name is " + ChordName + " and its length is " + YScale );

    }


    /*
     * unit default methods ignore them for now
     */

    // Start is called before the first frame update
    void Start()
    {
        ReadFile();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
