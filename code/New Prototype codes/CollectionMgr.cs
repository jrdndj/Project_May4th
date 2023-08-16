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

    public int genre = 0; //1 if jazz 2 if blues
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

    List<(string, int)> JazzSeq016 = new List<(string, int)>
    {
        ("Dm7", 4),
        ("G7", 4),
        ("CM7", 4),
        ("A7", 4),
        ("Dm7", 4),
        ("G7", 4),
        ("CM7", 4),
        ("CM7", 4)
    };

    List<(string, int)> JazzSeq017 = new List<(string, int)>
    {
         ("Dm7", 4),
        ("G7", 4),
        ("CM7", 4),
        ("A7", 4)
    };


    List<(string, int)> PracticeJazz01 = new List<(string, int)>
    {
        ("Dm7", 4),
        ("G7", 4),
        ("CM7", 4)
    };

    List<(string, int)> PracticeJazz02 = new List<(string, int)>
    {
        ("Dm7", 4),
        ("G7", 4),
        ("CM7", 4),
        ("A7", 4)

    };

    List<(string, int)> PracticeJazz03 = new List<(string, int)>
    {
        ("Dm7", 2),
        ("G7", 2),
        ("CM7", 2),
        ("A7", 2)
    };

    List<(string, int)> CoreJazz01 = new List<(string, int)>
    {
        ("Dm9", 2),
        ("G13", 2),
        ("CM9", 2)
    };

    //have a list of motifs here
    // contains lick 1 only 
    List<(string, int)> JazzMotifs01 = new List<(string, int)>
    {
        ("D4", 1),
        ("F4", 1),
        ("A4", 1),
        ("C5", 1),
        ("D4", 1),
        ("F4", 1),
        ("G4", 1),
        ("B4", 1),
        ("D4", 1),
        ("E4", 1),
        ("G4", 1),
        ("B4", 2) //prolonged
    }; //end jazzmotif01

    //partner harmony should also be here
    List<(string, int)> JazzHarmony01 = new List<(string, int)>
    {
        ("Dm9", 4),
        ("G13", 4),
        ("CM9", 5)
    }; //end jazzharmony01

    // contains lick 1 and 2 only 
    List<(string, int)> JazzMotifs02 = new List<(string, int)>
    {
        ("D4", 1),
        ("F4", 1),
        ("A4", 1),
        ("C5", 1),
        ("B4", 1),
        ("A5", 1),
        ("G5", 1),
        ("F5", 1),
        ("E5", 1),
        ("G5", 1),
        ("B5", 1),
        ("D5", 1),
        ("C5", 1),
        ("G5", 1) 
    }; //end jazzmotif02

    //partner harmony should also be here
    List<(string, int)> JazzHarmony02 = new List<(string, int)>
    {
        ("Dm9", 4),
        ("G13", 4),
        ("CM9", 6)
    }; //end jazzharmony02

    // contains lick 3 only 
    List<(string, int)> JazzMotifs03 = new List<(string, int)>
    {
        ("rest", 1),
        ("E4", 1),
        ("F4", 1),
        ("A4", 1),
        ("C5", 1),
        ("E5", 1),
        ("D5", 1),
        ("A5", 1),
        ("As5", 1),
        ("As5", 1),
        ("A5", 1),
        ("G5", 1),
        ("Fs5", 1),
        ("F5", 1),
        ("E5", 1),
        ("D5", 1),
        ("B5", 1),
        ("C5", 1),
        ("E5", 1),
        ("G5", 1),
        ("B5", 1),
        ("A5", 1),
        ("E5", 1)
    }; //end jazzmotif03

    //===== end of all pre defined motifs, chord progressions 

    //we use this to send to ChordMgr 
    public List<(string, int)> SendToChordMgr(List<(string, int)> SequenceToSend)
    {
        //all this ever does is send the right sequence 
        return SequenceToSend;
    }

    // Start is called before the first frame update
    void Start()
    {
        //ChordManager.GetComponent<ChordMgr>().ChordMapper(PracticeJazz01);
        //ChordManager.GetComponent<ChordMgr>().ChordMapper(JazzSeq017);

        //so we use this for the harmoney 
        ChordManager.GetComponent<ChordMgr>().ChordMapper(JazzHarmony01);

        //set motif details so everything is ready in the background
        ChordManager.GetComponent<ChordMgr>().SetMotifDetails(JazzMotifs03);
        //send motif and QA information here 
        //ChordManager.GetComponent<ChordMgr>().GetMotifList(JazzMotifs01);
        //we still need this for the harmony

        //ChordManager.GetComponent<ChordMgr>().ChordMapper(Blues001);
    }

    // Update is called once per frame
    void Update()
    {
        if (genre == 2)
        {
            ChordManager.GetComponent<ChordMgr>().ChordMapper(Blues001);
        }

    }//end update

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
