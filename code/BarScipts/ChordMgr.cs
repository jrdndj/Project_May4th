/*
 * This file acts as a model class for all chord-key relationships. 
 * The indices found in this file directly refer to their index in a piano 
 * serialized game object that the rest of the project can access. 
 * 
 * dependent to: RollManager
 * dependent of: ImprogManager
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordMgr : MonoBehaviour
{

    [SerializeField] GameObject ChordManager;
    [SerializeField] GameObject RollManager;

    //=========== CHORD RELATED VARIABLES ==========/

    //I think this should be moved to InputMgr just to be sure. 
    List<int> blacklist = new List<int>() { 1, 3, 6, 8, 10, 13, 15, 18, 20, 22, 25, 27, 30, 32, 34, 37, 39, 42, 44, 46, 49, 51, 54, 56, 58 };

    //systematically first octave white is 0th to 6th, second octave is 7th to 13th etc
    /**
     * octave of 2 index is 0 to 11
     * octave of 3 index is 12 to 23
     * octave of 4 index is 24 to 35
     * octave of 5 index is 36 to 47
     * octave of 6 index is 48 to 59
     * octave of 7 index is only 60
     */
    //list of some white only chords and their chord licks
    //D3 F3 A3 C4 --- D4 F4 A4 C5 - ok mapped! - higher D5 F5 A5 C6
    static List<int> Dmin7Chord = new List<int>() { 14, 17, 21, 24 };
    static List<int> Dmin7ChordTone = new List<int>() { 26, 29, 33, 36, 38, 41, 45, 48 };
    static List<int> Dmin7HalfStep = new List<int>() { 25, 28, 32, 35 };
    static List<int> Dmin7Above = new List<int>() { 28, 31, 35, 38 };

    //C3 E3 G3 B3 --- C4 E4 G4 B4 - ok mapped! - C5 E5 G5 B5
    static List<int> Cmaj7Chord = new List<int>() { 12, 16, 19, 23 };
    static List<int> Cmaj7ChordTone = new List<int>() { 24, 28, 31, 35, 36, 40, 43, 47 };
    static List<int> Cmaj7HalfStep = new List<int>() { 23, 27, 30, 34 };
    static List<int> CMaj7Above = new List<int>() { 26, 29, 33, 36 };

    //G3 B3 D4 F4 --- G4 B4 D5 F5 - ok mapped! 
    static List<int> G7Chord = new List<int>() { 19, 23, 26, 29 };
    static List<int> G7ChordTone = new List<int>() { 31, 35, 38, 41 };

    //D3 F3 G3 B3 --- D4 Fs4 G4 B4 - ok mapped! - D5 Fs5 G5 B5
    static List<int> G43Chord = new List<int>() { 14, 17, 19, 23 };
    static List<int> G43ChordTone = new List<int>() { 26, 29, 31, 35, 38, 41, 43, 47 };
    static List<int> G43HalfStep = new List<int>() { 25, 30, 34 }; //removed 29 here cos of overlap
    static List<int> G43Above = new List<int>() { 28, 33, 36 }; //removed 31 here cos of overlap

    //Amin7 A3 C4 E4 G4 --- A4 C5 E5 G5 - ok mapped!
    static List<int> Amin7Chord = new List<int>() { 21, 24, 28, 31 };
    static List<int> Amin7ChordTone = new List<int>() { 33, 36, 40, 43 };

    //Emin7 E3 G3 B3 D4 --- E4 G4 B4 D5 - ok mapped!
    static List<int> Emin7Chord = new List<int>() { 16, 19, 23, 26 };
    static List<int> Emin7ChordTone = new List<int>() { 28, 31, 35, 38 };

    //Fmaj7Chord F3 A3 C4 E4 --- F4 A4 C5 E 5 - ok mapped!
    static List<int> Fmaj7Chord = new List<int>() { 17, 21, 24, 28 };
    static List<int> Fmaj7ChordTone = new List<int>() { 29, 33, 36, 40 };
    //extended harmonies, simply get last value then +2

    //F7Chord F3 A3 C4 Ds4 --- 
    static List<int> F7Chord = new List<int>() { 17, 21, 24, 27 };

    // Blues Semitone on EScale E4 G4 A4 A#4 B4 D5 E5 A#5
    static List<int> ESemiTone = new List<int>() { 28, 31, 33, 34, 35, 38, 40, 43, 45, 46, 47, 50, 52 };
    static List<int> CSemiTone = new List<int>() { 24, 27, 29, 30, 31, 34, 36, 39, 41, 42, 43, 46, 48 };
    // F4 Gs4 As4 B4 C5 Ds5 F5
    static List<int> FSemiTone = new List<int>() { 29, 32, 34, 35, 36, 39, 41, 44, 46, 47, 48, 51, 53 };
    static List<int> GSemiTone = new List<int>() { 31, 34, 36, 37, 38, 41, 43, 47, 49, 50, 51, 54, 56 };

    //list of mixed chords and their chord licks
    //aka minor chords
    //Cmin7 C3 D#3 G3 As3 -- C4 D#4 G4 As4 - ok mapped!
    static List<int> Cmin7Chord = new List<int>() { 12, 15, 19, 22 };
    static List<int> Cmin7ChordTone = new List<int>() { 24, 27, 31, 34 };

    //C7 C3 E3 G3 A#3 - C4 E4 G4 A#4 - ok mapped! 
    static List<int> C7Chord = new List<int>() { 12, 16, 19, 22 };
    static List<int> C7ChordTone = new List<int>() { 24, 28, 31, 34 };

    //A2 Cs3 E3 G3 - A3 Cs4 E4 G4 - ok mapped! - A4 Cs5 E5 G5
    static List<int> A7Chord = new List<int>() { 9, 13, 16, 19 };
    static List<int> A7ChordTone = new List<int>() { 21, 25, 28, 31, 33, 37, 40, 43 };
    //combined with the chord tone, should only show the halfsteps
    static List<int> A7HalfStep = new List<int>() { 20, 24, 27, 30 };
    static List<int> A7Above = new List<int>() { 23, 26, 29, 33 };

    //====== SOME BLUES IMPROV variables =====
    List<string> BluesChordNames = new List<string>() { "C7", "C7", "C7", "C7",
                                                           "F7", "F7", "C7", "C7",
                                                              "G7", "F7", "C7", "C7",};
    List<List<int>> EBluesScale = new List<List<int>>() { C7Chord, C7Chord, C7Chord, C7Chord,
                                                           F7Chord, F7Chord, C7Chord, C7Chord,
                                                            G7Chord, F7Chord, C7Chord, C7Chord };
    List<List<int>> EBluesImprov = new List<List<int>>() { CSemiTone, CSemiTone, CSemiTone, CSemiTone,
                                                            FSemiTone, FSemiTone, CSemiTone, CSemiTone,
                                                             GSemiTone, FSemiTone, CSemiTone, CSemiTone};

    List<string> ChordNames = new List<string>() { "Dmin7", "G7", "Cmaj7", "A7", "Dmin7", "G7", "Cmaj7", "Cmaj7" };
    List<List<int>> ChordList = new List<List<int>>() { Dmin7Chord, G43Chord, Cmaj7Chord, A7Chord, Dmin7Chord, G43Chord, Cmaj7Chord, Cmaj7Chord };
    List<List<int>> LickList = new List<List<int>>() { Dmin7ChordTone, G43ChordTone, Cmaj7ChordTone, A7ChordTone, Dmin7ChordTone, G43ChordTone, Cmaj7ChordTone, Cmaj7ChordTone };
    List<List<int>> HalfStepList = new List<List<int>>() { Dmin7HalfStep, G43HalfStep, Cmaj7HalfStep, A7HalfStep, Dmin7HalfStep, G43HalfStep, Cmaj7HalfStep, Cmaj7HalfStep };
    List<List<int>> StepAboveList = new List<List<int>>() { Dmin7Above, G43Above, CMaj7Above, A7Above, Dmin7Above, G43Above, CMaj7Above, Cmaj7HalfStep };


    // The main goal is to pass and make vital chord info public or accessible
    void Start()
    {
        //call some functions that need to send data to another model class file

        
    }

    // Update is called once per frame
    void Update()
    {
        //there is really nothing to update there. 
    }
}//end ChordMgr
