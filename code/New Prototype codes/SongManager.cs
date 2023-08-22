//based from the tutorial of https://www.youtube.com/watch?v=ev0HsmgLScg&t=156s 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//drywetmidi frameworks
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Core;
using System.IO;
using UnityEngine.Networking; // for the web request 
using System;

public class SongManager : MonoBehaviour
{
    //required components
    public static SongManager Instance;
    public AudioSource audioSource;

    //some song elements
    public float songDelayInSeconds; //for the delay of playing the song
    public int inputDelayInMilliseconds; //in case there is a problem with the input device
    public double marginOfError; // in seconds

    //midi file related variables
    public string fileLocation;
    public float noteTime; //how much time the note is gonna be on the screen
    //how much time it travels from spawn poistion until the note tapped
    public float noteSpawnY;
    public float noteTapY;

    public float noteDespanY
    {
        get
        {
            return noteTapY - (noteSpawnY - noteTapY);
        }
    }

    public static MidiFile midiFile; 


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite());
        }
        else
        {
            ReadFromFile();
        }

    }

    private void ReadFromFile()
    {
        //throw new NotImplementedException();
    }

    private IEnumerator ReadFromWebsite()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLocation))
        {
            yield return www.SendWebRequest();

            //check for web error just in case 
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                //else send everything to memtory data 
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();
                }
            }
        }
    }

    private void GetDataFromMidi()
    {
        //get notes from the notes
        var notes = midiFile.GetNotes(); //make an array instead of an ICollection class
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count]; //this creates an emptyu array
        notes.CopyTo(array, 0); //put the contents into the array

        Invoke(nameof(StartSong), songDelayInSeconds);
    }// end getdatafrom midi

    public void StartSong()
    {
        audioSource.Play(); 
    }

    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
