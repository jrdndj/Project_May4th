// this shall be the new rollscript
// we start somethjing clean rollscript will be a legacy copy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

//using RtMidi
using RtMidi.LowLevel;

//for songmgr
using System.IO;
using UnityEngine.Networking;
using System;

//we dont need new event directives then xD 

sealed class RollMgr : MonoBehaviour
{
    #region Private members;
    MidiProbe _probe;

    List<MidiOutPort> _ports = new List<MidiOutPort>();

    // Does the port seem real or not?
    // This is mainly used on Linux (ALSA) to filter automatically generated
    // virtual ports.
    bool IsRealPort(string name)
    {
        return !name.Contains("Through") && !name.Contains("RtMidi");
    }

    // Scan and open all the available output ports.
    void ScanPorts()
    {
        for (var i = 0; i < _probe.PortCount; i++)
        {
            var name = _probe.GetPortName(i);
            Debug.Log("MIDI-out port found: " + name);
            _ports.Add(IsRealPort(name) ? new MidiOutPort(i) : null);
        }
    }

    // Close and release all the opened ports.
    void DisposePorts()
    {
        foreach (var p in _ports) p?.Dispose();
        _ports.Clear();
    }

    #endregion

    #region MonoBehaviour implementation

    //==== environment related variables

    //an important element to manage all children of spawns
    [SerializeField] GameObject rollManager, metronome;
    [SerializeField] GameObject songManager, ImprovManager;
    // [SerializeField] GameObject AudioManager;

    GameObject noteObject;
    public GameObject back, pause, forward;

    public GameObject green_line; //formerly 0 -85 0
    public GameObject spawnpoint; //for the spawnpoint
                                  // Move the noteObject to the destroy point (final Y position) based on note.Time
    float destroyY;

    public static RollMgr Instance;
    public bool reload = true; 

    //this helps the mapping of keys similar to that midi hardware
    [SerializeField] List<GameObject> pianoKeys = new List<GameObject>();

    //this lets us know which keys are black 
    List<int> blacklist = new List<int>() { 1, 3, 6, 8, 10, 13, 15, 18, 20, 22, 25, 27, 30, 32, 34, 37, 39, 42, 44, 46, 49, 51, 54, 56, 58 };
    bool isBlackPrefab = false;
    // bool batchSpawned = false;

    int numOfSpawns = 0;
    int spawnCount = 0;
    public int numOfEvents = 0;

    public float latestY = 0; 

    //storing the first y for the spawning of harmony
    float firstYpos = 0.0f;

    //==== lesson related variables
    // int userMode = 9; // default is 9, 1 is waL, 2 is test 3 is try

    //==== midi related variables
    public static MidiFile midiFile; // MIDI file asset
    public float fallSpeed = 0.0f; // Adjust this to control the speed of falling - was 100
    public float pixelsPerBeat = 0.0f; // height of one beat in pixels - shouldnt this be 1? 
    public string Filename; // this should be manipulated by ImprovManager

    public bool IsMotifPlaying = false;
    public bool isTouched = false; 

    public List<(float Time, float Duration, int NoteNumber)> noteInfo = new List<(float Time, float Duration, int NoteNumber)>();

    //public int SelectedSong; //which will be sent to PlayDelayedAudio for Audiomanager

    //playback related objects and variables
    [SerializeField] GameObject AudioManager;
    public AudioClip[] clips; 
    public AudioSource audioSource;
    public bool IsRhythmPlaying = false; 

    //=== midi out related variables 
    public int swingcount = 0;
    public int swingfrequency = 2;
    public int velocity = 80;
    int keysCount = 61;
    public int ctr = 0;

    //=========== COLOR RELATED VARIABLES ==========/
    //these are the color related objects
    public ColorBlock theColor;
    public Button theButton;
    Color32 improvpink = new Color32(255, 150, 234, 255);
    Color32 yellow = Color.yellow;
    Color32 belowpink = new Color32(75, 0, 130, 255);  //this is indigo akshully
    Color32 blues = new Color32(65, 105, 225, 255); // this is for the blues blue
    Color32 restblack = Color.black; //for the rests

    //this will now be the one receiving the MIDI events    //TODO Solidify 
    System.Collections.IEnumerator MIDIMessenger(List<(float Time, float Duration, int NoteNumber)> note) //this should be programmed to receive MIDI events
    {

        //algorithm
        // step 01: receive MIDI event as a parameter. thiis is triggered by the touching or
        // by isMotifPlaying variable
        // step 02: whne triggered, midimessenger sends to midiout to play sounds.
        // we need to SOLIDIFY this function so we can receive all types of message

        //routine methods: scan for midi ports. never modify this code 
        // _probe = new MidiProbe(MidiProbe.Mode.Out);

        foreach (var noteEvent in note)
        {
            //NOTEON
            pianoKeys[noteEvent.NoteNumber - 36].GetComponent<Image>().color = improvpink;
            //change velocity
            swingcount++;
            if (swingcount % swingfrequency == 0) //change force of press every other press to simulate swing press
            {
                velocity = 60;
            }
            else velocity = 80;
            if (swingcount == 5) swingcount = 0;

            foreach (var port in _ports)
            {
                port?.SendNoteOn(0, noteEvent.NoteNumber, velocity);
            }

            yield return new WaitForSeconds(noteEvent.Duration);

            //NOTEOFF
            // Debug.Log("released key" + noteEvent.NoteNumber);
            pianoKeys[noteEvent.NoteNumber - 36].GetComponent<Image>().color = Color.black;
            foreach (var port in _ports)
            {
                port?.SendNoteOff(0, noteEvent.NoteNumber);
            }
            yield return new WaitForSeconds(noteEvent.Duration);

        }//end for all
    }//end MIDIMessenger

    //putting here a more reusable version 
    System.Collections.IEnumerator playNote(int index, float Duration)
    {
        pianoKeys[index - 36].GetComponent<Image>().color = improvpink;
        swingcount++;
        if (swingcount % swingfrequency == 0) //change force of press every other press to simulate swing press
        {
            velocity = 60;
        }
        else velocity = 80;
        if (swingcount == 5) swingcount = 0;

        foreach (var port in _ports)
        {
            port?.SendNoteOn(0, index, velocity);
        }//endforeach for ports midi out

        //next
        ctr++;

        yield return new WaitForSeconds(Duration);

        //=== then key off
        pianoKeys[index - 36].GetComponent<Image>().color = Color.black;
        foreach (var port in _ports)
        {
            port?.SendNoteOff(0, index);
        }
        yield return new WaitForSeconds(Duration);

    }//end playNote

    public void ReleaseAllPresses()
    {
        foreach (var port in _ports)
        {
            for (int noteNumber = 0; noteNumber <= 127; noteNumber++)
            {
                // Send "note off" message for each note number
                port.SendNoteOff(0, noteNumber);
            }
        }

    }

    //putting here a more reusable version 
    System.Collections.IEnumerator playNoteTryYourself(int index, float Duration)
    {
        pianoKeys[index - 36].GetComponent<Image>().color = improvpink;
      

        //next
        ctr++;

        yield return new WaitForSeconds(Duration);

        //=== then key off
        pianoKeys[index - 36].GetComponent<Image>().color = Color.black;
        yield return new WaitForSeconds(Duration);

    }//end playNote



    //this is a copy of MIDIMessenger but with no midi out
    System.Collections.IEnumerator MIDIMessengerTryYourself(List<(float Time, float Duration, int NoteNumber)> note) //this should be programmed to receive MIDI events
    {

        foreach (var noteEvent in note)
        {
            //LIGHT KEYS - NO SOUND
            pianoKeys[noteEvent.NoteNumber - 36].GetComponent<Image>().color = improvpink;

            yield return new WaitForSeconds(noteEvent.Duration);

            //REMOVE KEY LIGHT - STILL NO SOUND
            pianoKeys[noteEvent.NoteNumber - 36].GetComponent<Image>().color = Color.black;

            yield return new WaitForSeconds(noteEvent.Duration);

        }//end for all
    }//end MIDIMessenger try yourself

    // this is an interface
    System.Collections.IEnumerator MIDIMessagInterface() //this should be programmed to receive MIDI events
    {

        //===== these are the template methods for this interface if things get lost 
        // Send an all-sound-off message.
        foreach (var port in _ports) port?.SendAllOff(0);

        for (var note = 36; note <= 96; note++) // 
        {
            //var note = 40 + (i % 30);
            //var note = (i % 30) - 36 ;
            //var note = i;

            //offset of -36 for the layout of the piano in unity
            Debug.Log("MIDI Out: Note On " + note);
            pianoKeys[note - 36].GetComponent<Image>().color = Color.white; //offset is 21 not 36
            foreach (var port in _ports) port?.SendNoteOn(0, note, 100);

            yield return new WaitForSeconds(0.1f);

            //note release 
            Debug.Log("MIDI Out: Note Off " + note);
            pianoKeys[note - 36].GetComponent<Image>().color = Color.black; //offset is 21 not 36
            foreach (var port in _ports) port?.SendNoteOff(0, note);

            yield return new WaitForSeconds(0.1f);

            //=== end template method 
        }//end for loop

        yield return new WaitForSeconds(0.1f);
    }//end interface

    //bringing back the old classic Start method for the sake of it 
    void Start()
    {

        //and this is the only thing we really need
        destroyY = green_line.GetComponent<RectTransform>().position.y;

        //routine methods: scan for midi ports. never modify this code 
        _probe = new MidiProbe(MidiProbe.Mode.Out);
      
    }//endstart

    // Update is called once per frame
    void Update()
    {

        // Rescan when the number of ports changed.
        if (_ports.Count != _probe.PortCount)
        {
            DisposePorts();
            ScanPorts();
        }//end port count

    }//end update

    //mandatory method for port detection 
    void OnDestroy()
    {
        _probe?.Dispose();
        DisposePorts();
    }

    #endregion

    //== startup


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


    //this should generate the MIDIout events
    public void GenerateMIDIEvents(String improvfilename)
    {

        MidiFile midi = midiFile;
     //   Debug.Log("Successfully read for midi events" + Filename);

        //routine getting of files
        var notes = midi.GetNotes();
        var tempoMap = midi.GetTempoMap();

        //consolate all three then load into noteInfo
        var noteCollection = new List<(float Time, float Duration, int NoteNumber)>();

        //this is the one for data passing
        foreach (Melanchall.DryWetMidi.Interaction.Note note in notes)
        {

            // float noteTime = (float) note.TimeAs<MetricTimeSpan>(tempoMap);
            float noteTime = ((float)note.TimeAs<MetricTimeSpan>(tempoMap).TotalMicroseconds / 100000000.0f);
            // Debug.Log("noteTime " + note.ToString() + "time : " + noteTime);

            //   float noteDuration = (float) note.LengthAs<MetricTimeSpan>(tempoMap);
            float noteDuration = (float)note.LengthAs<MetricTimeSpan>(tempoMap).TotalMicroseconds / 2400000.0f;
            // Debug.Log("noteDuration " + note.ToString() + "duration : " + noteDuration);

            //check the correct number in the piano key array - OK correct 
            int noteNumber = note.NoteNumber; //added offset
                                              //  Debug.Log("noteNumber " + note.ToString() + " note number " + noteNumber);

            //group thhem together
            var noteSet = (Time: noteTime, Duration: noteDuration, NoteNumber: noteNumber);

            //and add into note collection
            noteInfo.Add(noteSet);

        }//end for midi passing only

        numOfEvents = notes.Count;
     //   Debug.Log("There are " + numOfEvents + "midi out events");
    }// end generate MIDI Events


    // this generates piano roll from the file read
    public void GeneratePianoRoll(Color32 spawncolor, int spawntype, int userMode) //1 for melody, 2 for licks 
    {

        int sequencecount = 0; 
        reload = false; 
        //numOfSpawns = 0; //fresh start
        //piano related configs
        //set prefabs
        GameObject whitePrefab, blackPrefab;
        whitePrefab = (GameObject)Resources.Load("Prefab/new_whiteprefab");
        blackPrefab = (GameObject)Resources.Load("Prefab/new_blackprefab");
        // midi related configs
        // MidiFile midi = MidiFile.Read(Filename); // Read the MIDI file from ImprovMgr
        MidiFile midi = midiFile;
     //   Debug.Log("Successfully read " + Filename);

        //routine getting of files

        var notes = midi.GetNotes(); // note that this info must be sent to MIDIMessageReceiver

        var tempoMap = midi.GetTempoMap();

        //consolate all three then load into noteInfo
        var noteCollection = new List<(float Time, float Duration, int NoteNumber)>();

        //this is for spawning and rolling so have another one for data passing
        foreach (Melanchall.DryWetMidi.Interaction.Note note in notes)
        {
            //configs and declarations first

            var noteTime = note.TimeAs<MetricTimeSpan>(tempoMap);
            // Debug.Log("noteTime " + note.ToString() + "time : " + noteTime);

            var noteDuration = note.LengthAs<MetricTimeSpan>(tempoMap);
            // Debug.Log("noteDuration " +note.ToString() + "duration : " + noteDuration);

            //check the correct number in the piano key array - OK correct 
            int noteNumber = note.NoteNumber - 36; //added offset

            spawnCount++;

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

            /**
             * The algorithm is
             * 01 get x position for the key
             * 02 set the y position based on its time when it should appear
             * 03 set the height based on its duration
             * 04 change the scale based on its height
             * 05 adjust to consider the half height of the objects 
             * **/

            //==routine commands for proper game object management

            //set parent after instantiate for proper positioning
            noteObject.transform.SetParent(rollManager.transform, true);

            //the assignment of positions should only take place after the setting of parent

            // Calculate X position in pixels based on note.Time
            float xPosition = pianoKeys[noteNumber].transform.position.x;

      

            //calculate their position -
            float yPosition = ((float)noteTime.TotalMicroseconds / 1600000.0f * pixelsPerBeat); //for testing
                                                                                                //1000000.0f                              //should be somewhere between 1000000 and 2400000
                                                                                                // float yPosition = ((float)noteTime.TotalMicroseconds / 2400000.0f) * pixelsPerBeat; //latest working

    
            ////=== i still need this but only for harmony or type 1
            ////store here the first y position
            //if (spawnCount == 1)
            //{
            //    firstYpos = yPosition;
            //}

            //this firstyposition becomes an offset for melody type spawns

            float zPosition = pianoKeys[noteNumber].transform.position.z ; //should be always 0 or 1 only and based on the piannkey

            // Calculate the height of the object based on note.Duration
            //this was the last working - 28.03.2024
            //    float noteHeight = (float)noteDuration.TotalMicroseconds / 240000.0f;  //test mode //change 10 to 24 if ever
            //240000.0f the correct version
            //get the height of that object

            
            float noteHeight = (float)noteDuration.TotalMicroseconds / 240000 ;  //test mode //change 10 to 24 if ever
            if (isBlackPrefab) {
                Debug.Log("sample noteheight is " + noteHeight);
            }
            // float objectHeight = GetComponent<Renderer>().bounds.size.y;
            float objectHeight = noteObject.GetComponent<RectTransform>().rect.height * 2; //latest working

  

            //readjust the height f some notes that are too long a
            if (noteHeight>=4)
            {
                noteHeight = (noteHeight/2) + 0.2f; 
            }


            // float blacknoteHeight = objectHeight / 2; 
            //change the size (localscale) of the object based on the computed height
            noteObject.transform.localScale = new Vector3(1, noteHeight, 1);
            //consider the half of the shape when setting the position
            //get the half of the object - some computation here
            //  noteObject.transform.position = new Vector3(xPosition, spawnpoint.transform.position.y + yPosition + (objectHeight*2), zPosition); // Set the position

            //get the half of the object - some computation here
            // noteObject.transform.position = new Vector3(xPosition, spawnpoint.transform.position.y + yPosition + (objectHeight * 2), zPosition); // latest working
            //=== this was the last working 
            //   noteObject.transform.position = new Vector3(xPosition, spawnpoint.transform.position.y + yPosition + objectHeight * 2f, zPosition); // changes to test
            // or NoteHeight/2

            //testing this below 28.03.2024 - this is better
       //     noteObject.transform.position = new Vector3(xPosition, yPosition + objectHeight * 2f, zPosition); // changes to test

            noteObject.transform.position = new Vector3(xPosition, spawnpoint.transform.position.y + yPosition, zPosition); // changes to test
            //noteObject.transform.position = new Vector3(xPosition, yPosition, zPosition); // changes to test


            //== if type 1, adjust it one more time
            if (spawntype == 1)
            {
        //        noteObject.transform.position = new Vector3(xPosition, spawnpoint.transform.position.y + yPosition + yPosition + objectHeight*2, zPosition); // changes to test
            }//end adjust


            //set color to yellow or pink based on type
            noteObject.GetComponent<Image>().color = spawncolor; //pink for now SOLID it later
            // or if we are calling this again, ddoing two spawns then yeah this can be made simpler 

            //make colors darker if dark prefab
            if (isBlackPrefab)
            {
                Color darkerColor = new Color();
                darkerColor = (Color)improvpink * 0.75f;
                noteObject.GetComponent<Image>().color = darkerColor;
                //add more height for black prefabs to offset for their height
               // noteObject.transform.position = new Vector3(xPosition, spawnpoint.transform.position.y + yPosition + objectHeight + objectHeight + blacknoteHeight, zPosition); // changes to test

            }//endisBlackprefabcheck

            //now do some computation for the falling

            //remember latestY
           // latestY = noteObject.transform.position.y;

            //should be something like (SpawnScale.rect.height + (SpawnScale.rect.height)))

            // Start the coroutine to make the note fall at a constant speed
            //  StartCoroutine(FallAtEndOfDuration(noteNumber, noteObject.transform, noteObject.transform.position.y, destroyY - (objectHeight)));
            StartCoroutine(FallAtEndOfDuration(noteNumber, noteObject, noteObject.transform.position.y, destroyY - (objectHeight), userMode));

            //it should end on the half

            //done all routine spawn methodss
            numOfSpawns++;

        }//end foreach

       // latestY = 
        //add sequence count
        sequencecount++;

        //get the latest position
        //by this point we're done
        if (sequencecount<=4)
        {
            //ImprovManager.GetComponent<ImprovMgr>().LoadSequence(ImprovManager.GetComponent<ImprovMgr>().modeValue, ImprovManager.GetComponent<ImprovMgr>().lessonValue, ImprovManager.GetComponent<ImprovMgr>().guidanceValue, sequencecount);
        }
        //improvmanager load sequence


        //  Debug.Log("Reached end of spawning");
        //   batchSpawned = true; // signal for ImprovManager to rollKeys
        //  Debug.Log("Total prefabs spawned: " + numOfSpawns);

        //get noteObject count
    }//end generate piano roll

    //===
    public void DestroyObject(GameObject rollingobject)
    {
        Destroy(rollingobject);
    }//end destroy object

    // == some press related functions
    public void onNoteOn(int noteNumber, float velocity)
    {
      //  Debug.Log("notenumber is!" + noteNumber);
        //    default behaviour is show white
       

        //simulate press for these three UI button controls
        if (noteNumber == 57)
        {
            // Debug.Log("back!");
           // pianoKeys[noteNumber].GetComponent<Image>().color = Color.white;
            //simulate back press
            back.GetComponent<BackClick>().OnBackButtonClick();
        }else if (noteNumber == 59)
        {
          //  pianoKeys[noteNumber].GetComponent<Image>().color = Color.white;
            pause.GetComponent<PauseClic>().OnStopButtonClick();
        }
        else if (noteNumber == 60)
        {
         //   pianoKeys[noteNumber].GetComponent<Image>().color = Color.white;
            forward.GetComponent<ForwardClick>().OnNextButtonClick();
        }


        pianoKeys[noteNumber].GetComponent<Image>().color = Color.white;
    }//endonNoteOn;

    //when user releases a pressed key as per MIDIScript 
    public void onNoteOff(int noteNumber)
    {
        //    default behaviour is show black upon release
        pianoKeys[noteNumber].GetComponent<Image>().color = Color.black;

    }//end OnNoteOff

    //need a cleanup function
    public void CleanupKeyboard()
    {
        //show all 4 as a for loops
        for (int i = 0; i < keysCount; i++)
        {
            pianoKeys[i].GetComponent<Image>().color = Color.black;
        }//endfor

    }//endremovelicks


    //==== Roll related scripts

    //lerp related falling
    //private IEnumerator FallAtEndOfDuration(int noteNumber, Transform noteTransform, float initialY, float destroyY)
    private IEnumerator FallAtEndOfDuration(int noteNumber, GameObject rollingObject, float initialY, float destroyY, int userMode)
    {
        float elapsedTime = 0;
        float duration = Mathf.Abs(destroyY - initialY) / fallSpeed;// working latest if fallspeed = 100
                                                                    //float duration = 200.00f; //testing 
                                                                    //Debug.Log("speed is now " + duration);

        // Debug.Log("fallspeed is " + fallSpeed);
        //get the height of that object
        float objectHeight = (noteObject.GetComponent<RectTransform>().rect.height*2); //latest working *2


        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration; // ? 
                                              //  float t = duration / elapsedTime;
                                              //something here that checks time when it falls

            float beatsBeforeEnd = 4f; // Four beats before the end
            float thresholdT = 1f - (beatsBeforeEnd / 100f); // Threshold value for t corresponding to four beats before the end

          

            try
            {

                //// Assuming t is calculated somewhere else
                //if (t <= thresholdT && !metronome.GetComponent<Metronome>().metronomestarted)
                //{
                //    metronome.GetComponent<Metronome>().FourBeatStart();
                //}

                //this is where we check if the rolls touch the greenline
                if ((rollingObject.transform.position.y - (objectHeight)) <= green_line.GetComponent<RectTransform>().position.y)
                {                                //  
                    isTouched = true;

                    if (!metronome.GetComponent<Metronome>().metronomestarted)
                    {
                        metronome.GetComponent<Metronome>().metronomestarted = true; 
                        metronome.GetComponent<Metronome>().StartMetronome();
                    }//end check if metronome started

                    if (!IsMotifPlaying && userMode == 1 && ctr <= noteInfo.Count) //if waL play tunes 
                    {

                        StartCoroutine(playNote(noteInfo[ctr].NoteNumber, noteInfo[ctr].Duration));
                      //  DestroyObject(rollingObject);

                        //have the coroutine here
                        //  StartCoroutine(MIDIMessenger(noteInfo));
                        // StartCoroutine(MIDIMessenger(noteInfo));
                        //   Debug.Log("playing tunes");
                        //check to true
                        //  ctr++;
                        //   IsMotifPlaying = true;
                    }//end is motifyplaying
                    else if (!IsMotifPlaying && (userMode == 3) || ImprovManager.GetComponent<ImprovMgr>().CanReload)//its just try yourself mode
                    {
                        StartCoroutine(playNoteTryYourself(noteInfo[ctr].NoteNumber, noteInfo[ctr].Duration));
                       // DestroyObject(rollingObject);
                        //have the coroutine here
                        //   StartCoroutine(MIDIMessengerTryYourself(noteInfo));
                        // StartCoroutine(MIDIMessenger(noteInfo));
                        //    Debug.Log("hi tunes");
                        //check to true
                        //   IsMotifPlaying = true;
                    }//end else if user mode 3

                    //destroy here === revert if fckup          
                    DestroyObject(rollingObject);

                }//end check if touch
                else //keep moving
                {                   
                    rollingObject.transform.position = new Vector3(rollingObject.transform.position.x, Mathf.Lerp(initialY, destroyY, t), rollingObject.transform.position.z);
                    elapsedTime += Time.deltaTime;
                }//else

            }//endtry
            catch (System.Exception nre) //ignore this
            {
                // Debug.Log("previous object destroyed so all is good" + nre.Message);
                yield break;
            }//end catch
            
            yield return null;
        }//end while duration lerp function
        reload = true;

        //comment this just to see revert if fck up 
        // noteTransform.position = new Vector3(noteTransform.position.x, destroyY, noteTransform.position.z);

    }//end fallatendofduration

    //highlight related function
    System.Collections.IEnumerator StartUp(int index)
    {
        pianoKeys[index].GetComponent<Image>().color = improvpink;
        foreach (var port in _ports)
        {
            port?.SendNoteOn(0, index, 60);
        }//endforeach for ports midi out

        //next
        ctr++;

        yield return new WaitForSeconds(0.1f);

        //=== then key off
        pianoKeys[index].GetComponent<Image>().color = Color.black;
        foreach (var port in _ports)
        {
            port?.SendNoteOff(0, index);
        }
        yield return new WaitForSeconds(0.1f);

    }//end startup

    //rhythm related scripts
    //selecting audio for audiosource
    //playing the audio source with delay
    //public void PlayDelayedAudio()
    //{
    //    //decentralising to AudioManager game object 
    //    AudioManager.GetComponent<AudioManager>().ChangeAudioSelection(0); //change this one
    //    Instance.audioSource.Play(); 
    //    //Instance.MotifToPlay.Play();
    //}
}//end RollMgr
