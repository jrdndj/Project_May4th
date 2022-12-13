using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //needed for this specific script
using UnityEngine.InputSystem.Layouts; //needed for this specific script

//reference site: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.4/manual/QuickStartGuide.html

public class MIDIPress : MonoBehaviour
{
    //removed for this example focus on Update only
    // Start is called before the first frame update
   void Start()
    {
        InputSystem.onDeviceChange += (device, change) =>
        {
            //these are routine note events so dont delete these
            //lines for nows
            if (change != InputDeviceChange.Added) return;

            var midiDevice = device as Minis.MidiDevice;
            if (midiDevice == null) return;

            //note on click to capture events original copy
           // midiDevice.onWillNoteOn += (note, velocity) =>
           // {
            //    Debug.Log(string.Format(
             //       "Note On #{0} ({1}) vel:{2:0.00} ch:{3} dev:'{4}'",
             //       note.noteNumber,
              //      note.shortDisplayName,
              //      velocity,
              //      (note.device as Minis.MidiDevice)?.channel,
               //     note.device.description.product
                //));
           // }; //end of midiDevice on WillNoteOn

            //revised onWillNoteOn
            midiDevice.onWillNoteOn += (note, velocity) =>
            {
                Debug.Log(string.Format(
                    "Note On #{0} ({1}) vel:{2:0.00}",
                    note.noteNumber,
                    note.shortDisplayName,
                    velocity    
                ));
            }; //end of midiDevice on WillNoteOn

            //we need to pass note.shortDisplayName to trigger the event

            //we dont need velocity for note off
            midiDevice.onWillNoteOff += (note) => {
                Debug.Log(string.Format(
                    "Note Off #{0} ({1})",
                    note.noteNumber,
                    note.shortDisplayName 
                ));
            }; //end of midiDevice on WillNoteOff

        }; //end of InputSystem.ondeviceChange

    }

    // Update is called once per frame
    void Update()
    {

      //todo something later
        
    }
}
