using System; //for TimeSpan to work
using System.Collections;
using System.Collections.Generic; //for lists to work 
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
    public float speed=500; 

    //note related variables
    public int index;
    long YScale = 0; // this will be the length 
    string NoteName = null; //adding null fixes the problem
    string ShowUpTime, EndTime;

    //song input variables
    string[] InputNotes = new string[3000];
    string[] InputShowUpTime = new string[3000];
    long[] InputChordLength = new long[3000];
    float[] InputXCoords = new float[3000];
    //bool splitted = false;

    //adding dictionary for the note elements
    //string1 is Key-time, string2 is elt-indexs of InputNotes
    Dictionary<string, List<string>> NoteTimes = new Dictionary<string, List<string>>();
    
    //coroutine related variables
    private IEnumerator spawn;
    private IEnumerator move;

    //thread timer related variables
    private float startWatch;
    //private int threadCtr = 0;
    public TimeSpan songTime;
    public float currentTime;
    public int startMinutes;
    public TimeSpan start;
    public int kCtr = 0; //used to track mapping with time and length
    //float globalStartTime = 0, globalEndTime = 0;


    public struct FetchList
    {
       public float TimePoint; //point of time where the keys spawn
       //index to the note from input stream as element of a list
       public List<Int32> NoteIndex; 
    }//endstruct

    FetchList[] fetch;

    //spawn related variables
    public int spawnNumber = 0;

    //GameObject[] bang = new GameObject[3000];

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
        //var midiFile = MidiFile.Read("Assets/MusicXML/Intermediate/Mozart - Sonata Facile 1st Movement.mid");
        //var midiFile = MidiFile.Read("Assets/MusicXML/improv sample.mid");

        //contains folder of improv lickss
        var midiFile = MidiFile.Read("Assets/MusicXML/ImprovLicks/Lick01.mid");

        //to confirm file was read, we play a sound preview for now
        /* using (var outputDevice = OutputDevice.GetByName("Microsoft GS Wavetable Synth"))
         using (var playback = midiFile.GetPlayback(outputDevice))
         {
             playback.Speed = 1.0; //initially 2.0 but we want regular speed
             playback.Play();
         }
        */
        //get MIDI duration to help us determine the timer

        TimeSpan midiFileDuration = midiFile.GetDuration<MetricTimeSpan>();
        songTime = midiFileDuration;
        Debug.Log("Song duration is " + midiFileDuration); //prints the duration 
        
        //GetSongInfo(midiFile: midiFile);
        return midiFile;

        //we gotta get ordered notes instead of chord info so it makes more sense
        // see https://www.codeproject.com/Articles/1200014/DryWetMIDI-High-level-processing-of-MIDI-files
        //var notes = objectsManager.Objects
        // note.Length -= 100;

    }//end ReadFile

    //we get the elements such as note, chord showup time and length from Note 
    void GetSongInfo(MidiFile midiFile)
    {

        //getting note info instead of chord info. 
        IEnumerable<Melanchall.DryWetMidi.Interaction.Note> notes = midiFile.GetNotes();

        //add  tempomap to do time span conversions
        TempoMap tempoMap = midiFile.GetTempoMap();
        //now lets check how the data looks like
        //Notes to Chords: look at me, I am boss now 
        int Ctr = 0;
        foreach (var row in notes.ToList())
        {
          //  Debug.Log("Extracted count: " + Ctr + " chord: " + row + " chord time: " + row.Time + " chord endtime: " + row.EndTime + " chord length: " + row.Length);
            //put in current stream
            NoteName = row.ToString();            
            MetricTimeSpan metricTime = TimeConverter.ConvertTo<MetricTimeSpan>(row.Time, tempoMap); //get rowtime from ticks to metrictime format
            TimeSpan bufferTime = (TimeSpan)metricTime;
            ShowUpTime = bufferTime.ToString(@"mm\:ss\:fff"); //store the string equivalent of it //removed @"hh\:mm\:ss" //should be in @"mm\:ss\:fff"

            //recycle metricTime to convert EndTime this time 
            metricTime = TimeConverter.ConvertTo<MetricTimeSpan>(row.EndTime, tempoMap);
            //recycle timespan
            bufferTime = (TimeSpan)metricTime;
            EndTime = bufferTime.ToString(@"mm\:ss\:fff"); //should be in @"mm\:ss\:fff"
            YScale = row.Length; //you need to convert from ticks to metric length if we wanna know when
            //but we need YScale for length for the prefab later on that's why
            //Debug.Log(NoteName + " will show up on " + ShowUpTime + " and will last " + YScale + " and will disappear on " + EndTime);

            //put all these values in the input stream 
            InputNotes[Ctr] = NoteName;
            InputShowUpTime[Ctr] = ShowUpTime;
            InputChordLength[Ctr] = YScale;
            //Debug.Log(NoteName + " >> " + ShowUpTime + " >> " + YScale );

            //this creates the hashmap of time and key pairs for spawning
            List<string> list1 = new List<string>();
            try
            {
                for (int i = 0; i < NoteTimes[ShowUpTime].Count; i++)
                {
                    list1.Add(NoteTimes[ShowUpTime][i]);
                }
                
            }
            catch(Exception e)
            {
                //Debug.Log("came here");
                NoteTimes[ShowUpTime] = new List<string>();
            }
            list1.Add(NoteName);
            //Debug.Log(list1);
            NoteTimes[ShowUpTime] = list1;
            //Debug.Log(NoteTimes);
            //thanks nuwan

            Ctr++;
        }//endforeach

        ////print contents of dictionary just to be sure
        //foreach (var item in NoteTimes)
        //{
        //    //print the time key
        //    Debug.Log(item.Key);
        //    for (int i = 0; i < NoteTimes[item.Key].Count; i++)
        //    {
        //        Debug.Log(NoteTimes[item.Key][i]);
        //    }
        //}//foreachdictionary print

        // notes > chord 
        // check this library for this solution
        //https://github.com/melanchall/drywetmidi/issues/17
        // IEnumerable<Chord> chords = midiFile.GetChords();

        //you will find chord definitions here https://melanchall.github.io/drywetmidi/api/Melanchall.DryWetMidi.Interaction.Chord.html 

    }//endgetsonginfo

    // in unity: Gs5, B5 etc
    // in midi file: "C2 C3" <- we need to split 
    void GetKeyIndex()
    {
        int Ctr = 0;
        int streamlength = InputNotes.Length;

        //we set the index in KeyIndex array so know the position
        for (Ctr = 0; Ctr < streamlength; Ctr++) 
        {
            for (int Ctr2 = 0; Ctr2 < 69; Ctr2++)
            {
                if (string.Equals(InputNotes[Ctr], KeyIndex[Ctr2]))
                {
                    index = Ctr2;
                 //   Debug.Log("The index of " + InputNotes[Ctr] + " is " + XCoords[index]);
                    //store the completed value in InputXCoords
                    InputXCoords[Ctr] = XCoords[index];
                }//endif
            }//endinnerfor
           // Ctr++;
        }//endouterfor
        Debug.Log("XCoords assigned and captured. ");

    }//endGetKeyIndex   

    //call the coroutine when the logic is satisfied (at time x)
       
   

    public IEnumerator SpawnKey(int Ctr)
    //private void SpawnKey(string NoteName, long YScale)
    {
        GameObject Spawn; 
        float keyStartTime = currentTime;

        //set spawn point - get the y coord of 69th element which is the green light
        var YCordSpawnPoint = this.gameObject.transform.GetChild(69).position.y;
        // Debug.Log("entered spawnkey");
        //test instantiate by child
        Spawn = GameObject.Instantiate(Spawn_prefab, new Vector3(InputXCoords[Ctr], YCordSpawnPoint+InputChordLength[Ctr], 0), Quaternion.identity, Spawn_prefab.transform.parent);
        //Debug.Log("note instantiated");
        //yield break;// return Note;

        // Debug.Log("note yielded");
        //Note.transform.SetParent(Note.transform); //set your parent
        //Note = Instantiate(Spawn_prefab); //instantiate as child
        Spawn.transform.localScale = new Vector3(40, InputChordLength[Ctr], 1); //set length
        //changed 20 from 30 to adjust to key size
        

        //uncomment these two to go back just in case
        //Note = GameObject.Instantiate(Spawn_prefab, new Vector3(InputXCoords[Ctr], 130, 0), Quaternion.identity, Spawn_prefab.transform.parent);
        //Note.transform.localScale = new Vector3(30, InputChordLength[Ctr], 1);
        Debug.Log(InputNotes[Ctr] + " was spawned ");

        //simply move them after? 
        //MoveKey(Note);


        //Vector3 Ypos = gameObject.transform.GetChild(68).position;
        ////Debug.Log("ks" + keyStartTime);
        ////float keyEndTime=100;
        ////  green line is 68th element in piano prefab object
        //var YCordGreenLine = this.gameObject.transform.GetChild(68).position.y;

        //if (Spawn.transform.position.y + (Spawn.transform.localScale.y / 2) <= YCordGreenLine)
        //{
        //    Destroy(Spawn);
        //    Debug.Log("Object destroyed");
        //}
        //else
        //{
        //    Debug.Log("delta time " + Time.deltaTime);
        //    //Note1.transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        //    //Note1.transform.position = Note1.transform.position - new Vector3(0, speed*Time.deltaTime, 0);
        //    //Ypos.y = Mathf.Lerp(transform.position.y, Ypos.y, 0.2f);
        //    Spawn.transform.position = new Vector3(Spawn.transform.position.x, Mathf.Lerp(transform.position.y, Ypos.y, 0.2f), Spawn.transform.position.z);

        //    // Note1.transform.position.y = Vector3.Lerp(Note1.transform.position.y, this.gameObject.transform.GetChild(68).position, Time.deltaTime);
        //    Debug.Log("transform position " + Spawn.transform.position.y);
        //    Debug.Log("Moving...");
        //}//set to 5f for now

        ////temporarily moved movekey here to move stuff
        //var YCordGreenLine = this.gameObject.transform.GetChild(68).position.y;
        //if (Note.transform.position.y + (Note.transform.localScale.y / 2) <= YCordGreenLine)
        //{
        //    Destroy(Note);
        //    Debug.Log("Object destroyed");
        //}
        //else
        //{
        //    Note.transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        //    Debug.Log("Position is " + Note.transform.position);
        //    Debug.Log("Moving...");
        //}//set to 5f for now

        //this is where the key moves down
        // var YCordGreenLine = this.gameObject.transform.GetChild(68).position.y;
        //if (Note.transform.position.y + (Note.transform.localScale.y / 2) <= YCordGreenLine)
        // {
        //   Destroy(Note);
        //  Debug.Log("Object destroyed");
        //}
        // else Note.transform.position -= new Vector3(0, speed * Time.deltaTime, 0); //set to 5f for now
        //this moves the piano roll down based on speed times deltatime


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
        //yield break;
        //yield return Note;
        StartCoroutine(MoveKey(Spawn, keyStartTime));
       // MoveKey(Spawn, keyStartTime);
        yield return Spawn;
        //Destroy(Spawn);
        //Debug.Log("Object destroyed");


    }//endspawnkey

    private IEnumerator MoveKey(GameObject Note1, float keyStartTime)
    //private void MoveKey(GameObject Note1, float keyStartTime)
    {

        //this is the target position vector of the LERP
        Vector3 Ypos = gameObject.transform.GetChild(68).position;
        var YCordGreenLine = this.gameObject.transform.GetChild(68).position.y;
       // float time = 0.01f;
        float time = 0; 

        //this is the start position of the object 
        Vector3 startPosition = Note1.transform.position;
        //the target interpolation is the y position plus half of its size 
        Vector3 targetPosition = new Vector3(Note1.transform.position.x, Ypos.y - (Note1.transform.localScale.y /2), -1);
        while (Note1.transform.position.y + (Note1.transform.localScale.y / 2) > YCordGreenLine)
        //changed rom >= to >. revert if it causes issues
            {
            Note1.transform.position = Vector3.Lerp(startPosition, targetPosition, time);
            // Debug.Log("startposition" + startPosition);
            //  Debug.Log("target" + targetPosition);
            time += Time.deltaTime;
            //original time

           
            //time = time * time * time * (time * (6f * time - 15f) + 10f);
            yield return null;            
        }        
        //Note1.transform.position = targetPosition;
        //yield return null; 
        //float keyEndTime=100;
        //  green line is 68th element in piano prefab object
        //var YCordGreenLine = this.gameObject.transform.GetChild(68).position.y;

        //if (Note1.transform.position.y + (Note1.transform.localScale.y / 2) <= YCordGreenLine)
        //{
        //    Destroy(Note1);
        //    Debug.Log("Object destroyed");
        //}
        //else
        //{
        //    Debug.Log("delta time " + Time.deltaTime);
        //    //Note1.transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        //    //Note1.transform.position = Note1.transform.position - new Vector3(0, speed*Time.deltaTime, 0);
        //   //Ypos.y = Mathf.Lerp(transform.position.y, Ypos.y, 0.2f);
        //    Note1.transform.position = new Vector3(Note1.transform.position.x, Mathf.Lerp(transform.position.y, Ypos.y, 0.2f), Note1.transform.position.z); 

        //    // Note1.transform.position.y = Vector3.Lerp(Note1.transform.position.y, this.gameObject.transform.GetChild(68).position, Time.deltaTime);
        //    Debug.Log("transform position " + Note1.transform.position.y);
        //    Debug.Log("Moving...");    
        //}//set to 5f for now
        //for (float ks = keyStartTime; ks <= keyEndTime; ks+=3 ) // 150*.02
        //{
        //    Debug.Log("start time " + ks + "end time " + keyEndTime);
        //    Debug.Log("initial y pos is" + Note1.transform.position.y);
        //    if (Note1.transform.position.y + (Note1.transform.localScale.y / 2) >= YCordGreenLine)
        //    {
        //        //goes down
        //        //Note1.transform.position = Note1.transform.position + new Vector3(0, -ks, 0);

        //        Note1.transform.position = Vector3.Lerp(Note1.transform.position, this.gameObject.transform.GetChild(68).position, Time.deltaTime);
        //        // Note1.transform.position.y = Note1.transform.position.y - ks;
        //        Debug.Log("y is " + Note1.transform.position.y);
        //        //Debug.Log("ks moving is " + ks);
        //        //Debug.Log("Moving...");

        //    }
        //    else
        //    {
        //        keyEndTime = currentTime; 
        //        //Destroy(Note1);
        //        //Debug.Log("Object destroyed");
        //    }
        //}//endfor
        //if (Note.transform.position.y + (Note.transform.localScale.y / 2) <= YCordGreenLine)
        //{
        //    Destroy(Note);
        //    Debug.Log("Object destroyed");
        //}
        //else
        //{
        //    //continuously call thius 'timer' until object is destroyed
        //    for (; keyStartTime <=  )
        //    {
        //        Note.transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        //        Debug.Log("Moving...");
        //    }//to 
        //}//set to 5f for now
        //this moves the piano roll down based on speed times deltatime
        //  }//endwhile
        //yield return null;
        Destroy(Note1);
        Debug.Log("Object destroyed");
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

        //set timer to zero here
        currentTime = 0.000f; //current time is stored in seconds, start at 00
        start = TimeSpan.Zero;
        startWatch = Time.time;

        //STEP 04: Spawn keys based on x positions based from key index
        // SpawnKey();      

    }

    // Update is called once per frame
    //this is where we put the code to update the position of the spawned keys
    void Update()
    {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        //Debug.Log("XXXX " + time.ToString(@"mm\:ss\:fff"));
        //timespan should have only 3 decimal places (truncate to 3) 
        foreach (var item in NoteTimes)
        {
            //compare current time with NoteTimes[item.Key]
            if (String.Compare(time.ToString(@"mm\:ss\:fff"), item.Key) == 0)
            {
                //Debug.Log("item.Key is " + item.Key + " and current time is " + time.ToString(@"mm\:ss\:fff"));
                //iterate through the rest of the items in List<>
                for (int nCtr = 0; nCtr < NoteTimes[item.Key].Count; nCtr++)
                {
                    //spawn here                   
                    //Debug.Log("Should spawn " + NoteTimes[item.Key][nCtr] + " at " + item.Key);
                    StartCoroutine(SpawnKey(kCtr));
                    
                    //StartCoroutine(MoveKey(Spawn, keyStartTime));

                    kCtr++;                    
                    }//enditerator
             }//endifcomparison
              //move time!

           
        }//endforeach

        //should be outside foreach
        //currentTime += (Time.deltaTime * 0.05f); 
        currentTime += Time.deltaTime;
       // currentTime += (Time.deltaTime * 0.051f)/2; //addded 0 cos it skips a lot
        //Debug.Log("current" + currentTime);
        //Debug.Log("item.Key is " + item.Key + " and current time is " + time.ToString(@"mm\:ss\:fff"));

        //anything below this will be replaced by the new algorithm above

        ////this checks how many has been spawned 
        //int nSpawned = InputNotes.Length;

        ////pop it like a stack
        //TimeSpan time = TimeSpan.FromSeconds(currentTime);
        //for (threadCtr = 0; threadCtr <= InputNotes.Length-nSpawned && nSpawned > 0 ; threadCtr++ ){
        //    Debug.Log("current time is " + time.ToString(@"mm\:ss\:fff"));
        //    if (String.Compare(time.ToString(@"mm\:ss\:fff"), InputShowUpTime[threadCtr]) == 0){
        //        Debug.Log("InputShowUpTime of " + InputNotes[threadCtr] + " is " + InputShowUpTime[threadCtr]);
        //        SpawnKey(threadCtr);
        //        nSpawned--;
        //        //decrement for each spawned key
        //        //after spawn just move them
        //        }//endif
        //    Debug.Log("move to next character. current time: " + time.ToString(@"mm\:ss\:fff"));
        //}//endfor that scans through each of elements in the stream

        ////only increment to the next time when all keys on that time have been spawned
        //currentTime += Time.deltaTime*0.01f; //multiply by 100
        //Debug.Log(">>> Current time is " + currentTime);


        ////for (threadCtr = 0; threadCtr<InputNotes.Length; threadCtr++) {
        ////    //timespan should be here 
        ////    TimeSpan time = TimeSpan.FromSeconds(currentTime);
        ////    //assume things can spawn at the start of time
        ////    if (String.Compare(time.ToString(@"mm\:ss\:fff"), InputShowUpTime[threadCtr])==0){
        ////        SpawnKey(threadCtr);
        ////        //after spawn just move them 
        ////    }//endif
        ////    if (String.Compare(time.ToString(@"mm\:ss\:fff"), InputShowUpTime[threadCtr+1]) == 0)
        ////    {
        ////        SpawnKey(threadCtr+1);
        ////        //check the next n keys 
        ////    }//endif 


        ////    //delta time and threadctr should not move
        ////    currentTime += Time.deltaTime;
        ////    //Debug.Log("current time is " + currentTime.ToString(@"mm\:ss\:fff"));
        ////    Debug.Log("current time is " + time.ToString(@"mm\:ss\:fff"));
        ////    Debug.Log("InputShowUpTime of " + InputNotes[threadCtr] + " is " + InputShowUpTime[threadCtr]);

        // }//for loop to time the spawning of keys

        //dont move for now just spawn at the right time 
        //move = MoveKey(Note);
        // StartCoroutine(move);

        //spawn = SpawnKey();
        //StartCoroutine(spawn);
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

    }//endUpdate()
}
 