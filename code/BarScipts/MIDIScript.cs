//this script was from Kate Sawada's tutorial
// more info https://github.com/KateSawada/midi_visualizer_tutorial_01/blob/vol01/Assets/Scripts/MidiScript.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MIDIScript : MonoBehaviour
{
    // midi note number of lowest key in your midi device
    // 21: A0
    int keyOffset = 36; //from 21 

    //=== old generation of managers 
    [SerializeField] GameObject BarManager; //for the improv
    [SerializeField] GameObject RollManager; //for the pianoroll

    //=== new generation of Managers
    [SerializeField] GameObject ImprovManager; //improv controls
    [SerializeField] GameObject ChordManager; //chord key mappings
    [SerializeField] GameObject InputManager; //chord time mappings
    [SerializeField] GameObject TimeManager; //time and bar controls
    [SerializeField] GameObject VizManager; //everything that is toggled


    // Start is called before the first frame update
    void Start()
    {
        InputSystem.onDeviceChange += (device, change) =>
        {
            if (change != InputDeviceChange.Added) return;
            var midiDevice = device as Minis.MidiDevice;
            if (midiDevice == null) return;

            midiDevice.onWillNoteOn += (note, velocity) =>
            {
                Debug.Log(string.Format(
                    "Pressed #{0} {1} vel:{2:0.00}  ",
                    note.noteNumber,
                    note.shortDisplayName,
                    velocity
                ) + System.DateTime.UtcNow.ToString(@"mm\:ss\:fff"));
                //noteon is processed by RollManager
                //RollManager.GetComponent<RollScript>().onNoteOn(note.noteNumber - keyOffset, velocity);

                ImprovManager.GetComponent<ImprovMgr>().onNoteOn(note.noteNumber - keyOffset, velocity);


            }; //important onWillNoteOn function 

            midiDevice.onWillNoteOff += (note) =>
            {
                Debug.Log(string.Format(
                    "Released #{0} {1} ",
                    note.noteNumber,
                    note.shortDisplayName
                ) + System.DateTime.UtcNow.ToString(@"mm\:ss\:fff"));

                //noteoff is processed by rollmanager
                //RollManager.GetComponent<RollScript>().onNoteOff(note.noteNumber - keyOffset);

                ImprovManager.GetComponent<ImprovMgr>().onNoteOff(note.noteNumber - keyOffset);

                //decommissioning BarManager for now
                //BarManager.GetComponent<BarScript>().onNoteOff(note.noteNumber - keyOffset);
            };
        };
    }//end Start

    // Update is called once p  er frame
    void Update()
    {

    }//end update
}//end MIDIScript