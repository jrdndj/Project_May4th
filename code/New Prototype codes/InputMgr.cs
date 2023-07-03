/*
 * This file takes care of the series of chords and their 
 * corresponding time. 
 * They contain information about chord sequences, their lengths
 * which are then passed to rollscript and is dependent on the timings
 * 
 * note that error checking happens on ImprovMgr not here
 * this class only prepares the final sequence for rollscript (tbh im not sure
 * why we really have this) 
 * by TimeMgr
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : MonoBehaviour
{
    //==== crucial variables ===== /
    [SerializeField] GameObject RollManager;


    //==== crucial receiving functions ===== / 
    //a function that receives from ChordMgr
    public void ListReceiver(List<List<int>> ListReceived)
    {
        Debug.Log("Final Chord sequence received " + ListReceived);
    }//endListReceiver


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
