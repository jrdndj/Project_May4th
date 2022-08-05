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

    //spawnkey related variables
    public GameObject Spawn_prefab; //dont forget to drag the prefab to the script in the unity interface
    public GameObject Note;

    //stores the index of the piano keys for GetChildPosition.x
    float[] XCoords = new float[69]; 
    // e. g. c2 is 3, greenline is 68, 0 based index
  
    //standard speed considering 100 bpm 
    public float speed=100;

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
        //spawnkey must be in update and not on star

    }

    //get their XCoords and store them in the array for easy access
    /*
     * piano prefab has 69 objects
     * we have 0 to 68 index
     * white keys start from 2 (index) to 38 (index)
     * black keys start from 39 to 63 (index)
     * other elements 64 to 68
     */
    void GetXCoordinates()
    {
        for (int ctr = 0, ctr2 = 3; ctr < 68 && ctr2 <=63; ctr++, ctr2++)
        {
            XCoords[ctr] = this.gameObject.transform.GetChild(ctr2).GetChild(1).position.x;
            Debug.Log("Piano key is " + this.gameObject.transform.GetChild(ctr2) +
                " its X coordinate is  " + XCoords[ctr]);
        }
    }
    //what now jordan you need to map them 

    private void SpawnKey(string ChordName, long YScale)
    {
        /* spawn key at ideal place */ 
        //gets the x coordinates
        // c2 is 3, line is 1 for each note
        var XCord = this.gameObject.transform.GetChild(3).GetChild(1).position.x;
       //var XCord = XCoords[]
    
        //test values: Chord C2 C3 chord length 480
        // c2: x: -541.5     y: -149      z: 0
        // green line:  x: 0    y: -80     z: 0 
        Note = GameObject.Instantiate(Spawn_prefab, new Vector3(XCord, 130, 0), Quaternion.identity, Spawn_prefab.transform.parent);
        /* key spawned must have the size based on its YScale value from ChordInfo */
        Note.transform.localScale = new Vector3(30, YScale, 1);

        Debug.Log("Chord name is " + ChordName + " and its length is " + YScale );

        //this code is from Melody.CS
       // for (int i = 0; i < melodyNotes.Length; i++)
      //  {
            //update this code later to contain the list of chords extracted 
          //  GameObject keyRef = GameObject.Find(melodyNotes[i]);

            //update this to contain the x and y from chord info 
          //  GameObject note = GameObject.Instantiate(note_prefab, keyRef.transform.position + new Vector3(0, 500 + cumulativeTime * partitureSpeed, 0), Quaternion.Euler(0, 0, 0));
            //melody notes here is processed as an array and is called in the loop 
          //  note.GetComponent<Note>()._name = melodyNotes[i];
          //  note.transform.SetParent(canv.transform);

            //set new time for the notes
         //   cumulativeTime += (float)melodyDuration[i];
            //Debug.Log(cumulativeTime);
      //  }

    }

    // Start is called before the first frame update
    void Start()
    {
        GetXCoordinates();
        ReadFile();
    }

    // Update is called once per frame
    //this is where we put the code to update the position of the spawned keys
    void Update()
    {
        //green line is 68th element in piano prefab object
        var YCordGreenLine = this.gameObject.transform.GetChild(68).position.y;
        if (Note.transform.position.y <= YCordGreenLine)
        {
            Destroy(Note);
            Debug.Log("Object destroyed");
        }
        else Note.transform.position -= new Vector3(0, speed * Time.deltaTime, 0); //set to 5f for now
       //this moves the piano roll down based on speed times deltatime
       
    }
}
 