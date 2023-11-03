using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PressMgr : MonoBehaviour
{
   [SerializeField] GameObject RollMgr; //for the pianoKeys
   [SerializeField] GameObject ImprovMgr; //for the modes
    public static PressMgr Instance;

    int keyOffset = 36; //from 21

    //commented references for mode
   // public int modeValue = 9, lessonValue = 9, guidanceValue = 9; //mode, lesson and guidance mgrs need these


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
                //record everything
                Debug.Log(string.Format(
                    "Pressed #{0} {1} vel:{2:0.00}  ",
                    note.noteNumber,
                    note.shortDisplayName,
                    velocity
                ) + System.DateTime.UtcNow.ToString(@"mm\:ss\:fff"));
                //noteon is processed by RollManager
                RollMgr.GetComponent<RollMgr>().onNoteOn(note.noteNumber - keyOffset, velocity);                      

            }; //important onWillNoteOn function 

            midiDevice.onWillNoteOff += (note) =>
            {
                //record everything
                Debug.Log(string.Format(
                    "Released #{0} {1} ",
                    note.noteNumber,
                    note.shortDisplayName
                ) + System.DateTime.UtcNow.ToString(@"mm\:ss\:fff"));

                //noteoff is processed by rollmanager
                RollMgr.GetComponent<RollMgr>().onNoteOff(note.noteNumber - keyOffset);

                
            };
        };

    }

    // Update is called once per frame
    void Update()
    {

    }//end update



}//endd PressMgr
