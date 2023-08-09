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
    //this is for BarManager
   [SerializeField] GameObject BarManager;

    //spawnkey related variables
    public GameObject Spawn_prefab; //dont forget to drag the prefab to the script in the unity interface
    //public GameObject Note;

    //for easier mapping of the keys in the piano
    string[] KeyIndex = new string[69];

    //stores the index of the piano keys for GetChildPosition.x
    float[] XCoords = new float[69]; 
    // e. g. c2 is 3, greenline is 68, 0 based index
  
    //standard speed considering 150 bpm 
    public float speed=500;

    //number for notes to spawn
    public int noteCtr = 0;

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

    //we also map the keys from the midi file to into the keyboard itself
    //to do this we use a dictionary as well
    Dictionary<string, List<string>> FileChords = new Dictionary<string, List<string>>();

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
        IEnumerable<Melanchall.DryWetMidi.Interaction.Chord> songchords = midiFile.GetChords();

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
            noteCtr++;
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
             //   NoteTimes[ShowUpTime] = new List<string>();
                Debug.Log("Exception " + e);
            }
            list1.Add(NoteName);
            //Debug.Log(list1);
            NoteTimes[ShowUpTime] = list1;
            //Debug.Log(NoteTimes);
            //thanks nuwan

            Ctr++;
        }//endforeach

        ////print contents of dictionary just to be sure
        //  foreach (var item in NoteTimes)
        // {
        //print the time key
        //   Debug.Log(item.Key);
        //    for (int i = 0; i < NoteTimes[item.Key].Count; i++)
        //    {
        //        Debug.Log(NoteTimes[item.Key][i]);
        //   }
        //  }//foreachdictionary print

        //TEST SENT CHORD
      //  BarManager.GetComponent<BarScript>().spawnKeys("C#2", noteCtr);

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
       
   
   


    // Start is called before the first frame update
    void Start()
    {
      

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
                    //  StartCoroutine(SpawnKey(kCtr));
                    //uncomment if ready to play

                    //StartCoroutine(MoveKey(Spawn, keyStartTime));

                    //send the stuff here to BarScript
                    

                    kCtr++;                    
                    }//enditerator
             }//endifcomparison
  
        }//endforeach

        //should be outside foreach
        //currentTime += (Time.deltaTime * 0.05f); 
        currentTime += Time.deltaTime;
       // currentTime += (Time.deltaTime * 0.051f)/2; //addded 0 cos it skips a lot

    }//endUpdate()
}
 