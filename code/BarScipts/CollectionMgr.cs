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
        ("C7", 1),
        ("F7", 2),
        ("C7", 2),
        ("G7", 1),
        ("F7", 1),
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
        ChordManager.GetComponent<ChordMgr>().ChordMapper(JazzSeq004);
        //04 for now cos 01 has rest. i have yet to deal with that 

    }

    // Update is called once per frame
    void Update()
    {
    

    }
}
