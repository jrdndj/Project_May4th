using System.Collections;
using System.Collections.Generic;
//using System; //added for TimeSpan
using System.Linq; //added for toList of chords
using UnityEngine;
//added this to consider drywetmidi libraries
using Melanchall.DryWetMidi.Core; //read and write midi 
//using Melanchall.DryWetMidi.Multimedia; //playback and output
using Melanchall.DryWetMidi.Interaction; //to get song info

public class ParseMIDI : MonoBehaviour
{

    //spawnkey related variables
    public GameObject Spawn_prefab; //dont forget to drag the prefab to the script in the unity interface
    public GameObject Note;

    //for easier mapping of the keys in the piano
    string[] KeyIndex = new string[69];

    //stores the index of the piano keys for GetChildPosition.x
    float[] XCoords = new float[69]; 
    // e. g. c2 is 3, greenline is 68, 0 based index
  
    //standard speed considering 150 bpm 
    public float speed=150;

    //note related keywords
    public int index;
    long YScale = 0; // this will be the length 
    string NoteName = null; //adding null fixes the problem
    float ShowUpTime, EndTime;

    //song input variables
    string[] InputNotes = new string[3000];
    float[] InputShowUpTime = new float[3000];
    long[] InputChordLength = new long[3000];
    float[] InputXCoords = new float[3000];
    //bool splitted = false;

    //coroutine related variables
    private IEnumerator spawn;
    private IEnumerator move;

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

        for (int ctr = 0, ctr2 = 3; ctr < 68 && ctr2 <= 63; ctr++, ctr2++)
        {
            //store key value in index of KeyIndex
            KeyIndex[ctr] = this.gameObject.transform.GetChild(ctr2).name;
            //store their value in XCords
            XCoords[ctr] = this.gameObject.transform.GetChild(ctr2).GetChild(1).position.x;
            //Debug.Log("Piano key is " + this.gameObject.transform.GetChild(ctr2) +
            //   " its X coordinate is  " + XCoords[ctr]);
            Debug.Log("Piano key is " + KeyIndex[ctr] + " its X coordinate is  " + XCoords[ctr]);
            /*so in principle
            KeyIndex[0] = C2
            XCoords[0] = x position of c2
            */
        }//endfor
    }//endGetXCoordinates

    /*
       this method reads the file received from the file browser master
       verifies it by playing it and passing it to the
       next method for further info extraction
     */
    private MidiFile ReadFile()
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
        //GetSongInfo(midiFile: midiFile);
        return midiFile;

        //we gotta get ordered notes instead of chord info so it makes more sense
        // see https://www.codeproject.com/Articles/1200014/DryWetMIDI-High-level-processing-of-MIDI-files
        //var notes = objectsManager.Objects
        // note.Length -= 100;

    }//end ReadFile

    /*
        now we need to parse it in musicXML so we can get features such as
        pitch
        timbre
        tempo
        dynamics
        etc
   */
    void GetSongInfo(MidiFile midiFile)
    {

        //getting note info instead of chord info. 
        IEnumerable<Melanchall.DryWetMidi.Interaction.Note> notes = midiFile.GetNotes();
        //now lets check how the data looks like
        //Notes to Chords: look at me, I am boss now 
        int Ctr = 0;
        foreach (var row in notes.ToList())
        {
            Debug.Log("Extracted count: " + Ctr + " chord: " + row + " chord time: " + row.Time + " chord endtime: " + row.EndTime + " chord length: " + row.Length);
            //put in current stream
            NoteName = row.ToString();
            ShowUpTime = row.Time;
            EndTime = row.EndTime;
            YScale = row.Length;

            //put all these values in the input stream 
            InputNotes[Ctr] = NoteName;
            InputShowUpTime[Ctr] = ShowUpTime;
            InputChordLength[Ctr] = YScale;
            Ctr++;
        }//endforeach

        // notes > chord 
        // check this library for this solution
        //https://github.com/melanchall/drywetmidi/issues/17
        // IEnumerable<Chord> chords = midiFile.GetChords();

        //you will find chord definitions here https://melanchall.github.io/drywetmidi/api/Melanchall.DryWetMidi.Interaction.Chord.html 
        //  int Ctr = 0;
        //foreach (var row in chords.ToList())
        //{ 
        //Debug.Log(" count: " + Ctr + " chord: " + row + " chord time: " + row.Time + " chord endtime: " + row.EndTime + " chord length: " + row.Length);
        //  ChordName = row.ToString();
        // ShowUpTime = row.Time;
        //EndTime = row.EndTime;
        //YScale = row.Length;

        //put all these values in the input stream which
        //InputChords[Ctr] = ChordName;
        //InputShowUpTime[Ctr] = ShowUpTime;
        //InputChordLength[Ctr] = YScale;
        //Ctr++;
        //}
        //Debug.Log("Song size is " + Ctr++);
        //consider writing this into csv so we can compare later on with the timing of user keypress
 
        //return (NoteName, YScale);
     //   SpawnKey(NoteName, YScale);

        //no need to pass since the arrays have been updated
    }//endgetsonginfo

    //we set the index in KeyIndex array so know the position
    // in unity: Gs5, B5 etc
    // in midi file: "C2 C3" <- we need to split 
    void GetKeyIndex()
    {
        int Ctr = 0;
        int streamlength = InputNotes.Length;

        for(Ctr = 0; Ctr < streamlength; Ctr++) 
        {
            for (int Ctr2 = 0; Ctr2 < 69; Ctr2++)
            {
                if (string.Equals(InputNotes[Ctr], KeyIndex[Ctr2]))
                {
                    index = Ctr2;
                    Debug.Log("The index of " + InputNotes[Ctr] + " is " + XCoords[index]);
                    //store the completed value in InputXCoords
                    InputXCoords[Ctr] = XCoords[index];
                }//endif
            }//endfor
           // Ctr++;
        }//endouterfor

    }//endGetKeyIndex

    private IEnumerator SpawnKey()
    //private void SpawnKey(string NoteName, long YScale)
    {
        while (true)
        {
            int songlength = InputNotes.Length;
            for (int Ctr = 0; Ctr < songlength; Ctr++)
            {
                Note = GameObject.Instantiate(Spawn_prefab, new Vector3(InputXCoords[Ctr], 130, 0), Quaternion.identity, Spawn_prefab.transform.parent);
                Note.transform.localScale = new Vector3(30, InputChordLength[Ctr], 1);
            }//endfor

            /* spawn key at ideal place */
            //gets the x coordinates
            // c2 is 3, line is 1 for each note
            // var XCord = this.gameObject.transform.GetChild(3).GetChild(1).position.x;
            //var XCord = XCoords[]

            // c2: x: -541.5     y: -149      z: 0
            // green line:  x: 0    y: -80     z: 0 
            // Note = GameObject.Instantiate(Spawn_prefab, new Vector3(XCord, 130, 0), Quaternion.identity, Spawn_prefab.transform.parent);
            /* key spawned must have the size based on its YScale value from ChordInfo */
           // Note.transform.localScale = new Vector3(30, YScale, 1);

            //Debug.Log("Chord name is " + NoteName + " and its length is " + YScale);
            yield return null;
        }//endofwhile
    }//endspawnkey

    private IEnumerator MoveKey()
    {
        while (true)
        {
            //green line is 68th element in piano prefab object
            var YCordGreenLine = this.gameObject.transform.GetChild(68).position.y;
            if (Note.transform.position.y + (Note.transform.localScale.y / 2) <= YCordGreenLine)
            {
                Destroy(Note);
                Debug.Log("Object destroyed");
            }
            else Note.transform.position -= new Vector3(0, speed * Time.deltaTime, 0); //set to 5f for now
                                                                                       //this moves the piano roll down based on speed times deltatime
        }//endwhile
    }

    // Start is called before the first frame update
    void Start()
    {

        //STEP 01: get all xcoordinates relative to the piano object for easy loading
        GetXCoordinates();

        //STEP 02: Extract info in chord and store info in data structures
        GetSongInfo(ReadFile());
        //ReadFile();

        //STEP 03: Get Key Index of extracted chords
        GetKeyIndex();
        //mappings are correct and do not cause issue now

        //STEP 04: Spawn keys based on x positions based from key index
        SpawnKey();
        
    }

    // Update is called once per frame
    //this is where we put the code to update the position of the spawned keys
    void Update()
    {
        spawn = SpawnKey();
        StartCoroutine(spawn);
        // move = MoveKey();
        //StartCoroutine(move);

        //green line is 68th element in piano prefab object
        //var YCordGreenLine = this.gameObject.transform.GetChild(68).position.y;
        //if (Note.transform.position.y+(Note.transform.localScale.y/2) <= YCordGreenLine)
        // {
        //    Destroy(Note);
        //    Debug.Log("Object destroyed");
        // }
        // else Note.transform.position -= new Vector3(0, speed * Time.deltaTime, 0); //set to 5f for now
        //this moves the piano roll down based on speed times deltatime

    }
}
 