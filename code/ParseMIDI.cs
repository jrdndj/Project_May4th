using System.Collections;
using System.Collections.Generic;
//using System; //added for TimeSpan
using UnityEngine;
//added this to consider drywetmidi libraries
using Melanchall.DryWetMidi.Core; //read and write midi 
using Melanchall.DryWetMidi.Multimedia; //playback and output
using Melanchall.DryWetMidi.Interaction; //to get song info

public class ParseMIDI : MonoBehaviour
{
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

        //to confirm file was read, we play a sound preview for now
        using (var outputDevice = OutputDevice.GetByName("Microsoft GS Wavetable Synth"))
        using (var playback = midiFile.GetPlayback(outputDevice))
        {
            playback.Speed = 1.0; //initially 2.0 but we want regular speed
            playback.Play();
        }

        //pass file to know duration and get chord info
        getSongInfo(midiFile: midiFile);
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
    void getSongInfo(MidiFile midiFile)
    {

        // check this library for this solution
        //https://github.com/melanchall/drywetmidi/issues/17
        //TempoMap tempoMap = midiFile.GetTempoMap();
        IEnumerable<Chord> chords = midiFile.GetChords();
    }


    /*
     * unit default methods ignore them for now
     */

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
