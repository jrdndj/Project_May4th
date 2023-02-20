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

    [SerializeField] GameObject BarManager;

    // Start is called before the first frame update
    void Start()
    {
        InputSystem.onDeviceChange += (device, change) =>
        {
            if (change != InputDeviceChange.Added) return;
            var midiDevice = device as Minis.MidiDevice;
            if (midiDevice == null) return;

            midiDevice.onWillNoteOn += (note, velocity) => {
                Debug.Log(string.Format(
                    "Pressed #{0} {1} vel:{2:0.00}  ",
                    note.noteNumber,
                    note.shortDisplayName,
                    velocity
                ) + System.DateTime.UtcNow.ToString(@"mm\:ss\:fff"));

               BarManager.GetComponent<BarScript>().onNoteOn(note.noteNumber - keyOffset, velocity);
            };

            midiDevice.onWillNoteOff += (note) => {
                Debug.Log(string.Format(
                    "Released #{0} {1} ",
                    note.noteNumber,
                    note.shortDisplayName
                ) + System.DateTime.UtcNow.ToString(@"mm\:ss\:fff"));

                BarManager.GetComponent<BarScript>().onNoteOff(note.noteNumber - keyOffset);
            };
        };
    }

    // Update is called once p  er frame
    void Update()
    {

    }
}