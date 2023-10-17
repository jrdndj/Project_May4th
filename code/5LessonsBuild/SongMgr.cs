//based from the tutorial of https://www.youtube.com/watch?v=ev0HsmgLScg&t=156s 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;


public class SongMgr : MonoBehaviour
{
    public static SongMgr Instance;
    public AudioSource audioSource;
    ///public Lane_Harmony[] harmony_lanes;
   // public Lane_Harmony[] lick_lanes; 
    public float songDelayInSeconds;
    public double marginOfError; // in seconds

    public int inputDelayInMilliseconds;


    public string fileLocation;
    //public float noteTime;
    //public float noteSpawnY;
    //public float noteTapY;
    //public float noteDespawnY
    //{
    //    get
    //    {
    //        return noteTapY - (noteSpawnY - noteTapY);
    //    }
    //}

    public static MidiFile midiFile;

    //environment related parts
   // public float spawnpoint, destroypoint; //y coord of the spawnpoint
    //for the spawn point
  // public GameObject spawn_top; // 0 350 0

    //for the lower position limit
   // public GameObject green_line; //formerly 0 -85 0


    // Start is called before the first frame update
    void Start()
    {
        //get values from gameobjects 
    // spawnpoint = spawn_top.transform.localPosition.y;
    //   destroypoint = green_line.transform.localPosition.y;
  

        ////assign it to noteSpawnY and noteTapY
        //noteSpawnY = spawnpoint;
        //noteTapY = destroypoint;

        ////read from web 
        //Instance = this;
        //if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        //{
        //    StartCoroutine(ReadFromWebsite());
        //}//end if 
        ////or by default read from file if not available
        //else
        //{
        //    ReadFromFile();
        //}//enndelse 
    }//end Start()

    public void GetSongInfo()
    {
        //read from web 
        Instance = this;
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite());
        }//end if 
        //or by default read from file if not available
        else
        {
            ReadFromFile(fileLocation);
        }//enndelse 

    }//end get SongInfo

    private IEnumerator ReadFromWebsite()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLocation))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }//endif web request
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();
                }
            }//end else
        }//end using unity web request
    }//end ienumerator readfrom website 

    public MidiFile ReadFromFile(string fileLocation)
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
        return midiFile;
    }//end readfrom File()

    public void GetDataFromMidi()
    {

        //we should also get note.Time, note.NoteNumber, note.Length

        //assign it in an array 


        var notes = midiFile.GetNotes();
        var tempoMap = midiFile.GetTempoMap();

        //print all notes
        foreach (var elements in notes)
        {
           // //Debug.Log("notename" + elements.ToString());
            int newNoteNumber = elements.NoteNumber - 36; //this doesnt realy change the value
           //// int newNoteNumber = elements.NoteNumber;


           // ////we need to store these info into a tempo map as TimeSpan 
           // //Debug.Log("notenumber is " + newNoteNumber); //with the offset for the keys
           // //Debug.Log("note timestamp is " + elements.TimeAs<MetricTimeSpan>(tempoMap));
           // //Debug.Log("note length is " + elements.LengthAs<MetricTimeSpan>(tempoMap));

           // //then generate a pixel to time map
        }

        //print the contents of tempoMap
        // var tempoMap = new TempoMap(); // Replace this with your actual TempoMap instance

        //foreach (var eventInfo in tempoMap.GetTempoChanges())
        //{
        //    var time = eventInfo.Time;
        //    var tempoMicrosecondsPerBeat = eventInfo.Value; // Corrected line

        //    Debug.Log($"Time: {time}, Tempo: {tempoMicrosecondsPerBeat} us per beat");
        //}

       var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
       // Debug.Log("note count " + notes.Count);
        //var numbers = 
      notes.CopyTo(array, 0);

        //foreach(var elements in notes)
        //{
        //    Debug.Log("we have " + elements.ToString()) ;
        //}


        //=== these lines of code are concenred with lane harmony cs which we dont need anymore 
        //foreach (var lane in harmony_lanes)
        //{
        //    lane.SetTimeStamps(array);
        //   // Debug.Log("timestamp: " + lane);
        //}

        //foreach (var lane in lick_lanes)
        //{
        //    lane.SetTimeStamps(array);
        //    // Debug.Log("timestamp: " + lane);
        //}
        //====== end lane harmony needed code

       // Invoke(nameof(StartSong), songDelayInSeconds);
    }//end gata dara from midi ()

    public void StartSong()
    {
      //  audioSource.Play();
    }//end startsong

    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }//end get audio sourcetime
    
    void Update()
    {

    }//end update

}//end class 