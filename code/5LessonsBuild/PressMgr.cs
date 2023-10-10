using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
                //Debug.Log(string.Format(
                //    "Pressed #{0} {1} vel:{2:0.00}  ",
                //    note.noteNumber,
                //    note.shortDisplayName,
                //    velocity
                //) + System.DateTime.UtcNow.ToString(@"mm\:ss\:fff"));
                //noteon is processed by RollManager
                Instance.onNoteOn(note.noteNumber - keyOffset, velocity);


                //ImprovManager.GetComponent<ImprovMgr>().onNoteOn(note.noteNumber - keyOffset, velocity);


            }; //important onWillNoteOn function 

            midiDevice.onWillNoteOff += (note) =>
            {
                //Debug.Log(string.Format(
                //    "Released #{0} {1} ",
                //    note.noteNumber,
                //    note.shortDisplayName
                //) + System.DateTime.UtcNow.ToString(@"mm\:ss\:fff"));

                //noteoff is processed by rollmanager
                Instance.onNoteOff(note.noteNumber - keyOffset);

                //ImprovManager.GetComponent<ImprovMgr>().onNoteOff(note.noteNumber - keyOffset);

                //decommissioning BarManager for now
                //BarManager.GetComponent<BarScript>().onNoteOff(note.noteNumber - keyOffset);
            };
        };

    }

    // Update is called once per frame
    void Update()
    {

    }//end update

   // == some press related functions
    public void onNoteOn(int noteNumber, float velocity)
    {

        //render object first
       // Image tempImageRenderer = RollMgr.GetComponent<RollMgr>().pianoKeys[noteNumber].GetComponent<Image>();

        //default behaviour is show white

        //RollMgr.GetComponent<RollMgr>().pianoKeys[noteNumber].GetComponent<Image>().color = Color.red;



    }//endonNoteOn;

    //when user releases a pressed key as per MIDIScript 
    public void onNoteOff(int noteNumber)
    {

       

    }//end OnNoteOff

}//endd PressMgr
