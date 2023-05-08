/*
 * This serves as Model class for important chord-length information. It works
 * with ChordMgr to map the notes to the piano keys. The partner improv of the 
 * chord is also included here. 
 * 
 * Works with InputMgr for scoring purposes (especially timing)
 * Works with ChordMgr for the mapping with the piano keys then send to RolLScript
 *
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //for the toggle

public class CollectionMgr : MonoBehaviour
{
    //we need to send a message to RollManager 
    [SerializeField] GameObject ChordManager; //for the pianoroll


    /*
    * Here we describe the different sequences that the user can choose from
    * what we need to know is every chord has a semitone or a chordtone
    * 
    * */

    //solution was inspired from https://stackoverflow.com/questions/8002455/how-to-easily-initialize-a-list-of-tuples
    List<(string, int)> Blues001 = new List<(string, int)>
    {
        ("C7", 4),
        ("F7", 2),
        ("C7", 2),
        ("G7", 1),
        ("F7", 1),
        ("C7", 2)
    };
    List<(string, int)> Blues002 = new List<(string, int)>
    {
        ("C7", 2),
        ("C7", 2),
        ("F7", 2),
        ("C7", 2),
        ("G7", 2),
        ("F7", 2),
        ("C7", 2)
    };

    List<(string, int)> JazzSeq001 = new List<(string, int)>
    {
        ("Dm7", 2),
        ("G7", 2),
        ("CM7", 2),
        ("rest", 2)
    };

    List<(string, int)> JazzSeq003 = new List<(string, int)>
    {
        ("Dm7", 4),
        ("G7", 4),
        ("CM7", 4)
    };

    List<(string, int)> JazzSeq004 = new List<(string, int)>
    {
        ("Dm7", 4),
        ("G7", 4),
        ("C7", 2),
        ("C7", 2)
    };

    List<(string, int)> JazzSeq012 = new List<(string, int)>
    {
        ("Dm7", 2),
        ("G7", 2),
        ("CM7", 2),
        ("A7", 2),
        ("Dm7", 1)
    };

    List<(string, int)> JazzSeq013 = new List<(string, int)>
    {
        ("Dm7", 4),
        ("G7", 4),
        ("CM7", 4),
        ("A7", 1)
    };

    List<(string, int)> JazzSeq014 = new List<(string, int)>
    {
        ("Dm7", 4),
        ("G7", 4),
        ("CM7", 4),
        ("A7", 1),
        ("CM7", 4),
        ("CM7", 4),
        ("Dm7", 4),
        ("G7", 4),
        ("A7", 4),
    };

    List<(string, int)> JazzSeq015 = new List<(string, int)>
    {
        ("Dm7", 4),
        ("G7", 4),
        ("CM7", 4),
        ("A7", 4),
        ("CM7", 4),
        ("CM7", 4),
        ("Dm7", 4),
        ("G7", 4),
        ("A7", 4),
        ("CM7", 4),
        ("CM7", 4),
        ("Dm7", 4),
        ("G7", 4),
        ("CM7", 4)
    };

    //we use this to send to ChordMgr 
    public List<(string, int)> SendToChordMgr(List<(string, int)> SequenceToSend)
    {
        //all this ever does is send the right sequence 
        return SequenceToSend; 
    }

    // Start is called before the first frame update
    void Start()
    {

        //lets send something here - now just one sequence
        //but soon it will be something the user picks

        ////some crucial toggle lines
        //rollmodelistener.GetComponent<Toggle>();
        //rollmodelistener.onValueChanged.AddListener(delegate
        //{
        //    ToggleValueChanged(rollmodelistener);
        //});

        ChordManager.GetComponent<ChordMgr>().ChordMapper(JazzSeq015);
        //ChordManager.GetComponent<ChordMgr>().ChordMapper(Blues001);
        //04 for now cos 01 has rest. i have yet to deal with that 

    }

    // Update is called once per frame
    void Update()
    {

    }

    //======== toggle related functions begin here

    //public void ToggleValueChanged(Toggle change)
    //{

    //    //Debug.Log(change.isOn);

    //    if (change.isOn)
    //    {
    //        //Debug.Log(timebarlistener.isOn);
    //        ChordManager.GetComponent<ChordMgr>().ChordMapper(JazzSeq015);
    //        Debug.Log("Selected: Rolling Mode");
    //    }
    //    else
    //    {
    //        // Debug.Log("off");

    //    }
    //}//end toggle value changed

}
