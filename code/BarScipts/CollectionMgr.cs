/*
 * This serves as Model class for important chord-length information. It works
 * with ChordMgr to map the notes to the piano keys. The partner improv of the 
 * chord is also included here. 
 * 
 * Works with InputMgr for scoring purposes (especially timing)
 * Works with ChordMgr for the mapping with the piano keys then send to RolLScript
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionMgr : MonoBehaviour
{
    //dictionary of chord and length sequences 
    Dictionary<string, int> BluesSeq001 = new Dictionary<string, int>()
    {
        {"C7", 4},
        {"F7", 2},
        {"C7", 2},
        {"G7", 1},
        {"F7", 1},
        {"C7", 2}
    };

    Dictionary<string, int> JazzSeq001 = new Dictionary<string, int>()
    {
        {"Dm7", 2},
        {"G7", 2},
        {"CM7", 2},
        {"rest", 2},
    };

    Dictionary<string, int> JazzSeq003 = new Dictionary<string, int>()
    {
        {"Dm7", 4},
        {"G7", 4},
        {"CM7", 4}
    };

    Dictionary<string, int> JazzSeq004 = new Dictionary<string, int>()
    {
        {"Dm7", 4},
        {"G7", 4},
        {"C7", 2},
        {"C7", 2},
    };

    Dictionary<string, int> JazzSeq012 = new Dictionary<string, int>()
    {
        {"Dm7", 2},
        {"G7", 2},
        {"CM7", 2},
        {"A7", 2},
        {"Dm7", 1}
    };

    //some function here that sends something based on what user picks
    public Dictionary<string, int> SendToChordMgr()
    {
        Dictionary<string, int> SequenceToSend = new Dictionary<string, int>();

        return SequenceToSend; 
    }


    //===== some sample dictionary codes just to help you 
    //    //this creates the hashmap of time and key pairs for spawning
    //    List<string> list1 = new List<string>();
    //            try
    //            {
    //                for (int i = 0; i<NoteTimes[ShowUpTime].Count; i++)
    //                {
    //                    list1.Add(NoteTimes[ShowUpTime][i]);
    //                }

    //            }
    //            catch (Exception e)
    //{
    //    //Debug.Log("came here");
    //    //   NoteTimes[ShowUpTime] = new List<string>();
    //    Debug.Log("Exception " + e);
    //}
    //list1.Add(NoteName);
    ////Debug.Log(list1);
    //NoteTimes[ShowUpTime] = list1;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
