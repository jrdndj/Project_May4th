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
using System.Linq; //for advance queries
using UnityEngine.UI; //for the toggle 

public class ChordMgr : MonoBehaviour
{

    // [SerializeField] GameObject CollectionMgr;
    [SerializeField] GameObject InputManager;
    [SerializeField] GameObject ImprovManager;
    [SerializeField] GameObject RollManager;
    [SerializeField] GameObject BarPrefab; //for the size of the Bar

    //[SerializeField] GameObject CollectionManager; //improv controls

    int improvmode = 0; //0 if jazz, 1 if blues

    int chordtoneiterator = 12;
    //=========== ENVIRONMENT RELATED VARIABLES ==========/

    //where the chords are mapped
    //doesnt have to change, we send their key mappings
    List<List<int>> ChordListToSend = new List<List<int>>();
    List<List<int>> JazzListToSend = new List<List<int>>();
    List<List<int>> BluesListToSend = new List<List<int>>();
    List<List<int>> HalfStepToSend = new List<List<int>>();
    List<List<int>> StepAboveToSend = new List<List<int>>();
    List<string> ChordNamesToSend = new List<string>();
    List<int> LengthListToSend = new List<int>();
    List<int> YListPlotter = new List<int>(); //this is for the Y positions of the spawns that RollMgr will need
    List<int> LickListToSend = new List<int>(); //this is for highlight licklist of press mapper
                                                // we will put all the following lists in a masterlist for easy comparison
    List<int> OnWaitYListPlotter = new List<int>();
    //List<int> MotifList = new List<int>();
    List<List<int>> ManyLists = new List<List<int>>() { C7, Cm7, CM7, Dm7, G7, Am7, Em7, FM7, F7, A7 };

    List<int> tempPressed = new List<int>();

    //=== motif related data structures 
    //  List<(string, int)> LocalMotif = new List<(string, int)>();
    public List<int> MotifSizes = new List<int>(); //has to be public so Rm can access it
    List<int> MotifList = new List<int>(); // contains the key number for proper positioning
    List<int> MotifYPlots = new List<int>();

    //=========== CHORD RELATED VARIABLES ==========/

    //I think this should be moved to InputMgr just to be sure. 
    List<int> blacklist = new List<int>() { 1, 3, 6, 8, 10, 13, 15, 18, 20, 22, 25, 27, 30, 32, 34, 37, 39, 42, 44, 46, 49, 51, 54, 56, 58 };

    //list of some white only chords and their chord licks
    //D3 F3 A3 C4 --- D4 F4 A4 C5 - ok mapped! - higher D5 F5 A5 C6
    static List<int> Dm7 = new List<int>() { 14, 17, 21, 24 };
    static List<int> Dm7ct = new List<int>() { 26, 29, 33, 36, 38, 41, 45, 48 };
    static List<int> Dm7hs = new List<int>() { 25, 28, 32, 35, 37, 40, 44, 47 };
    static List<int> Dm7sa = new List<int>() { 28, 31, 35, 38 };
    static List<int> Dm7st = new List<int>() { 26, 29, 33, 36, 38, 41, 45, 48 }; //fix this! 

    //C3 E3 G3 B3 --- C4 E4 G4 B4 - ok mapped! - C5 E5 G5 B5
    static List<int> CM7 = new List<int>() { 12, 16, 19, 23 };
    static List<int> CM7ct = new List<int>() { 24, 28, 31, 35, 36, 40, 43, 47 };
    static List<int> CM7hs = new List<int>() { 23, 27, 30, 34, 35, 39, 42, 46 };
    static List<int> CM7sa = new List<int>() { 26, 29, 33, 36 };

    //we decommission this since we use G43-second inversion instead for better crabbing of G7
    ////G3 B3 D4 F4 --- G4 B4 D5 F5 - ok mapped! 
    //static List<int> G7 = new List<int>() { 19, 23, 26, 29 };
    //static List<int> G7ct = new List<int>() { 31, 35, 38, 41 };

    //D3 F3 G3 B3 --- D4 Fs4 G4 B4 - ok mapped! - D5 Fs5 G5 B5
    static List<int> G7 = new List<int>() { 14, 17, 19, 23 };
    static List<int> G7ct = new List<int>() { 26, 29, 31, 35, 38, 41, 43, 47 };
    static List<int> G7hs = new List<int>() { 25, 29, 30, 34, 37, 40, 42, 46 };
    static List<int> G7sa = new List<int>() { 28, 33, 36 };

    //Amin7 A3 C4 E4 G4 --- A4 C5 E5 G5 - ok mapped!
    static List<int> Am7 = new List<int>() { 21, 24, 28, 31 };
    static List<int> Am7ct = new List<int>() { 33, 36, 40, 43 };
    static List<int> Am7st = new List<int>() { 33, 36, 40, 43 }; //map this

    //Emin7 E3 G3 B3 D4 --- E4 G4 B4 D5 - ok mapped!
    static List<int> Em7 = new List<int>() { 16, 19, 23, 26 };
    static List<int> Em7ct = new List<int>() { 28, 31, 35, 38 };
    static List<int> Em7st = new List<int>() { 28, 31, 35, 38 };//fix this

    //Fmaj7Chord F3 A3 C4 E4 --- F4 A4 C5 E 5 - ok mapped!
    static List<int> FM7 = new List<int>() { 17, 21, 24, 28 };
    static List<int> FM7ct = new List<int>() { 29, 33, 36, 40 };
    static List<int> FM7st = new List<int>() { 29, 33, 36, 40 }; //fix
    //extended harmonies, simply get last value then +2

    //F7Chord F3 A3 C4 Ds4 --- 
    static List<int> F7 = new List<int>() { 17, 21, 24, 27 };

    // Blues Semitone on EScale E4 G4 A4 A#4 B4 D5 E5 A#5
    static List<int> E7st = new List<int>() { 28, 31, 33, 34, 35, 38, 40, 43, 45, 46, 47, 50, 52 };
    static List<int> C7st = new List<int>() { 24, 27, 29, 30, 31, 34, 36, 39, 41, 42, 43, 46, 48 };
    // F4 Gs4 As4 B4 C5 Ds5 F5
    static List<int> F7st = new List<int>() { 29, 32, 34, 35, 36, 39, 41, 44, 46, 47, 48, 51, 53 };
    static List<int> G7st = new List<int>() { 31, 34, 36, 37, 38, 41, 43, 47, 49, 50, 51, 54, 56 };

    //list of mixed chords and their chord licks
    //aka minor chords
    //Cmin7 C3 D#3 G3 As3 -- C4 D#4 G4 As4 - ok mapped!
    static List<int> Cm7 = new List<int>() { 12, 15, 19, 22 };
    static List<int> Cm7ct = new List<int>() { 24, 27, 31, 34 };
    static List<int> Cm7st = new List<int>() { 24, 27, 31, 34 }; //fix

    //C7 C3 E3 G3 A#3 - C4 E4 G4 A#4 - ok mapped! 
    static List<int> C7 = new List<int>() { 12, 16, 19, 22 };
    static List<int> C7ct = new List<int>() { 24, 28, 31, 34 };

    //A2 Cs3 E3 G3 - A3 Cs4 E4 G4 - ok mapped! - A4 Cs5 E5 G5
    static List<int> A7 = new List<int>() { 9, 13, 16, 19 };
    static List<int> A7ct = new List<int>() { 21, 25, 28, 31, 33, 37, 40, 43 };
    //combined with the chord tone, should only show the halfsteps
    static List<int> A7hs = new List<int>() { 20, 24, 27, 30, 32, 36, 39, 42 };
    static List<int> A7sa = new List<int>() { 23, 26, 29, 33 };
    static List<int> A7st = new List<int>() { 21, 25, 28, 31, 33, 37, 40, 43 }; //fix this

    //======= Swing Related Variables ====
    public List<int> DScaleSwing = new List<int>();
    public List<int> GScaleSwing = new List<int>();
    public List<int> CScaleSwing = new List<int>();


    //does the mapping and sends the right lists to responsible functions
    public void ChordMapper(List<(string, int)> ReceivedList)
    {
        /*
         * General algorithim
         * get the receive list - ok 
         * extract the string names and their lengths - ok 
         * determine their semitones and chordtones improvs - ok 
         * send improvs to ImprovMgr - ok 
         * send the correct list for InputMgr - ok 
         * **/

        //iterate through each in the list and then assign or map
        foreach (var chord in ReceivedList)
        {
            //Debug.Log("Mapping " + chord );
            //use .Item1 is the name of the string, .Item2 is the length
            //Debug.Log(chord.Item1);

            string chordname = chord.Item1;
            //check chord name and add the corresponding improvs as well
            switch (chordname)
            {
                //these should be arranged musically  
                case "C7":
                    {
                        ChordListToSend.Add(C7);
                        JazzListToSend.Add(C7ct);
                        BluesListToSend.Add(C7st);
                        ChordNamesToSend.Add("C7");
                        HalfStepToSend.Add(CM7hs);
                        StepAboveToSend.Add(CM7sa);
                        break;
                    }//end Cm7
                case "Cm7":
                    {
                        ChordListToSend.Add(Cm7);
                        JazzListToSend.Add(Cm7ct);
                        BluesListToSend.Add(C7st);
                        ChordNamesToSend.Add("Cm7");
                        HalfStepToSend.Add(CM7hs);
                        StepAboveToSend.Add(CM7sa);
                        break;
                    }//end Cm7
                case "CM7":
                    {
                        ChordListToSend.Add(CM7);
                        JazzListToSend.Add(CM7ct);
                        BluesListToSend.Add(C7st);
                        ChordNamesToSend.Add("CMaj7");
                        HalfStepToSend.Add(CM7hs);
                        StepAboveToSend.Add(CM7sa);
                        break;
                    }//end CM7
                case "CM9":
                    {
                        ChordListToSend.Add(CM7);
                        JazzListToSend.Add(CM7ct);
                        BluesListToSend.Add(C7st);
                        ChordNamesToSend.Add("CMaj9");
                        HalfStepToSend.Add(CM7hs);
                        StepAboveToSend.Add(CM7sa);
                        break;
                    }//end CM9
                case "Dm7":
                    {
                        ChordListToSend.Add(Dm7);
                        JazzListToSend.Add(Dm7ct);
                        BluesListToSend.Add(Dm7st);
                        ChordNamesToSend.Add("Dm7");
                        HalfStepToSend.Add(Dm7hs);
                        StepAboveToSend.Add(Dm7sa);
                        break;
                    }//end Dm7

                case "Dm9":
                    {
                        ChordListToSend.Add(Dm7);
                        JazzListToSend.Add(Dm7ct);
                        BluesListToSend.Add(Dm7st);
                        ChordNamesToSend.Add("Dm9");
                        HalfStepToSend.Add(Dm7hs);
                        StepAboveToSend.Add(Dm7sa);
                        break;
                    }//end Dm7
                case "Em7":
                    {
                        ChordListToSend.Add(Em7);
                        JazzListToSend.Add(Em7ct);
                        BluesListToSend.Add(E7st);
                        ChordNamesToSend.Add("Em7");
                        break;
                    }//end Em7
                case "F7":
                    {
                        ChordListToSend.Add(F7);
                        JazzListToSend.Add(FM7ct);
                        BluesListToSend.Add(F7st);
                        ChordNamesToSend.Add("F7");
                        break;
                    }//end FM7
                case "FM7":
                    {
                        ChordListToSend.Add(FM7);
                        JazzListToSend.Add(FM7ct);
                        BluesListToSend.Add(F7st);
                        ChordNamesToSend.Add("FM7");
                        break;
                    }//end FM7
                case "G7":
                    {
                        ChordListToSend.Add(G7);
                        JazzListToSend.Add(G7ct);
                        BluesListToSend.Add(G7st);
                        ChordNamesToSend.Add("G7");
                        HalfStepToSend.Add(G7hs);
                        StepAboveToSend.Add(G7sa);
                        break;
                    }//end G7
                case "G13":
                    {
                        ChordListToSend.Add(G7);
                        JazzListToSend.Add(G7ct);
                        BluesListToSend.Add(G7st);
                        ChordNamesToSend.Add("G13");
                        HalfStepToSend.Add(G7hs);
                        StepAboveToSend.Add(G7sa);
                        break;
                    }//end G13
                case "A7":
                    {
                        ChordListToSend.Add(A7);
                        JazzListToSend.Add(A7ct);
                        BluesListToSend.Add(A7st);
                        ChordNamesToSend.Add("A7");
                        HalfStepToSend.Add(A7hs);
                        StepAboveToSend.Add(A7sa);
                        break;
                    }//end A7 
                case "Am7":
                    {
                        ChordListToSend.Add(Am7);
                        JazzListToSend.Add(Am7ct);
                        BluesListToSend.Add(Am7st);
                        ChordNamesToSend.Add("Am7");
                        HalfStepToSend.Add(A7hs);
                        StepAboveToSend.Add(A7sa);
                        break;
                    }//end Am7

                //this is for both
                case "rest":
                    {
                        //no need to deal for this version of the code 
                        break;
                    }//end rest case
                default:
                    {
                        //TODO: catch "rest" in the piano during spawn
                        ChordListToSend.Add(Am7);
                        JazzListToSend.Add(Am7ct);
                        BluesListToSend.Add(Am7st);
                        ChordNamesToSend.Add("rest");
                        break;
                    }//end default and rest case
            }//end switch

            //then immediate add their length to lengthlist
            LengthListToSend.Add(chord.Item2);

        }//end foreach scan of received list

        //now that we have the details we need to send them to InputMgr and ImprovMgr

        //SEND KEY INFORMATION
        ImprovManager.GetComponent<ImprovMgr>().ListReceiver(ChordListToSend, JazzListToSend, BluesListToSend, HalfStepToSend, StepAboveToSend);
        //also send it straight to RollManager for the keys to spawn 
        RollManager.GetComponent<RollScript>().ListReceiver(ChordListToSend, JazzListToSend, BluesListToSend, HalfStepToSend, StepAboveToSend);

        //extra step to compute offset 
        //map ycoords of spawn and pass to RollMgr too
        MapSpawnYCoords();


        //we also need to send the time details!
        //SEND TIME INFORMATION 
        ImprovManager.GetComponent<ImprovMgr>().TimeReceiver(LengthListToSend);
        RollManager.GetComponent<RollScript>().TimeReceiver(LengthListToSend);

        //also send the chord names!
        RollManager.GetComponent<RollScript>().NamesReceiver(ChordNamesToSend);
        RollManager.GetComponent<RollScript>().GetMode(improvmode);

        //we dont need to send time to RollScript because the offset manages the time information of the spawnsb 

        //our work is done 
    }//end ChordMapper

    //we use this to send to InputMgr 
    public List<List<int>> SendToInputMgr(List<List<int>> ChordListToSend)
    {
        //all this ever does is send the right sequence 
        return ChordListToSend;
    }

    //assigns the Y coords based on the length and sequence
    public void MapSpawnYCoords()
    {
        /*
         * General algorithm
         * fetch item i in ChordListToSend - we dont really need to we just pass it
         * fetch its i in LengthListToSend - ok 
         * store value 0 in i - this is where the y of the first spawn - ok 
         * then compute offset based on the i in LenghListToSend - ok 
         * formula is like i x 60, this is the offset
         * this means item i+1 will have y of ix60. store this value for i+2 - ok 
         * repeat until end 
         * **/

        //changed to 30 from 60
        int offset = 60; //the first elements will have no offset
        int previousOffset = 0; //there is no existing offset
        int newOffset = 0;


        //store the 0 offset for the 0th element immediately
        YListPlotter.Add(0);
        //we use for loop instead for for each so we control stuff 
        for (int i = 1; i < LengthListToSend.Count; i++) //yes begin at 1
        {
            //formula should be
            //previous one x offset then divide by 2
            // (LengthListToSend[i - 1] * offset)/2 = 240/2 = 120
            // then current one x offset then divide by 2
            // (LengthListToSend[i] * offset)/2 = 60
            //then add them together
            // ((LengthListToSend[i - 1] * offset)/2) + ((LengthListToSend[i] * offset)/2)

            // Debug.Log("Offset to multiply" + LengthListToSend[i-1]);
            newOffset = ((LengthListToSend[i - 1] * offset) / 2) + ((LengthListToSend[i] * offset) / 2) + previousOffset;
            YListPlotter.Add(newOffset); //should be of the previous one

            //store the previous one for the next round
            previousOffset = newOffset;
            //previousOffset = LengthListToSend[i - 1] * offset;
            //
            //Debug.Log("New y coord is " + newOffset);
        }//end for loop yplotter
        // we use < so we only care about these spawns

        //then send everything to RollMgr
        RollManager.GetComponent<RollScript>().ReceiveYPlots(YListPlotter);

        //then do the same but for onwait ylist

        //start fresh
        offset = 60; //the first elements will have no offset
        previousOffset = 0; //there is no existing offset
        newOffset = 0;
        OnWaitYListPlotter.Add(0);
        for (int i = 1; i < LengthListToSend.Count; i++) //yes begin at 1
        {

            // Debug.Log("Offset to multiply" + LengthListToSend[i-1]);
            newOffset = ((2 * offset) / 2) + ((2 * offset) / 2) + previousOffset;
            OnWaitYListPlotter.Add(newOffset); //should be of the previous one

            //store the previous one for the next round
            previousOffset = newOffset;
            //previousOffset = LengthListToSend[i - 1] * offset;
            //
            //Debug.Log("New y coord is " + newOffset);
        }//end for loop yplotter

        RollManager.GetComponent<RollScript>().ReceiveOnWaitYPlots(OnWaitYListPlotter);

    }//end MapSpawnYCoords

    //maps the notenumber to the equivalent musical key

    //identifies the press and returns the lick list 
    public void PressMapper(List<int> userPress, List<int> correctPress)
    {
        switch (RollManager.GetComponent<RollScript>().VizMode)
        {
            case 2: CheckIfAChord(userPress); break;
            case 3:
                {
                    RollManager.GetComponent<RollScript>().validpress = CheckIfCorrect(userPress, correctPress);
                    //if valid then remember it
                    if (CheckIfCorrect(userPress, correctPress))
                    {
                        tempPressed = userPress.ToList();
                    }

                    break;
                }
            case 4:
                {
                    RollManager.GetComponent<RollScript>().validpress = CheckIfCorrect(userPress, correctPress);
                    RollManager.GetComponent<RollScript>().isPressed = true;
                    //  RollManager.GetComponent<RollScript>().isReleased = true;

                    //if valid then remember it
                    if (CheckIfCorrect(userPress, correctPress))
                    {
                        tempPressed = userPress.ToList();
                    }

                    //then trigger next hihglight
                    //RollManager.GetComponent<RollScript>().highlightNow = true;
                    break;
                }
            default: break;
        }//end switch PressMapper

    } //end PressMapper

    //chord checker function
    public bool CheckIfCorrect(List<int> userinput, List<int> correctPress)
    {
        //print contents to be sure
        foreach (int element in userinput)
        {
            //Debug.Log("Pressed " + element);
        }

        foreach (int element in correctPress)
        {
            // Debug.Log("Should match with " + element);
            if (!userinput.Contains(element))
            {
                return false; // Exit and return false if an element is not found
            }
        }
        return true; // All elements are found
    }
    //end check if correct

    //check if released
    public bool CheckifCorrectReleased(List<int> userreleased, List<int> guidedpresslist)
    {
        int ctr = 0;

        //print contents to be sure
        foreach (int element in guidedpresslist)
        {
            Debug.Log("Checking with " + element);
        }

        foreach (int element in userreleased)
        {
            //Debug.Log("Should match with " + element);
            if (guidedpresslist.Contains(element))
            {
                Debug.Log("Released " + element);
                ctr++;
            }
        }

        if (ctr == 4)
        {
            RollManager.GetComponent<RollScript>().UserReleased.Clear();
            RollManager.GetComponent<RollScript>().isReleased = true;
            return true;

        }
        else
        {
            RollManager.GetComponent<RollScript>().UserReleased.Clear();
            RollManager.GetComponent<RollScript>().isReleased = false;
            return false;
            // All elements are found
        }

    }//end checkif correct released 

    //chord comparer function
    public void CheckIfAChord(List<int> userinput)
    {
        if (userinput.Contains(9) && userinput.Contains(13) && userinput.Contains(16) && userinput.Contains(19))
        {
            RollManager.GetComponent<RollScript>().OnPressLicks.Clear();
            copyList(A7ct, 1);
            copyList(A7st, 2);
            RollManager.GetComponent<RollScript>().validpress = true;
            RollManager.GetComponent<RollScript>().display_name.text = "A7 chord tones";
        }
        else if (userinput.Contains(12) && userinput.Contains(16) && userinput.Contains(19) && userinput.Contains(22))
        {
            RollManager.GetComponent<RollScript>().OnPressLicks.Clear();
            copyList(C7ct, 1);
            copyList(C7st, 2);
            RollManager.GetComponent<RollScript>().validpress = true;
            RollManager.GetComponent<RollScript>().display_name.text = "C7 chord tones";
        }
        else if (userinput.Contains(12) && userinput.Contains(15) && userinput.Contains(19) && userinput.Contains(22))
        {
            RollManager.GetComponent<RollScript>().OnPressLicks.Clear();
            copyList(Cm7ct, 1);
            copyList(Cm7st, 2);
            RollManager.GetComponent<RollScript>().validpress = true;
            RollManager.GetComponent<RollScript>().display_name.text = "Cm7 chord tones";
        }
        else if (userinput.Contains(12) && userinput.Contains(16) && userinput.Contains(19) && userinput.Contains(23))
        {
            RollManager.GetComponent<RollScript>().OnPressLicks.Clear();
            copyList(CM7ct, 1);
            copyList(C7st, 2);
            RollManager.GetComponent<RollScript>().validpress = true;
            RollManager.GetComponent<RollScript>().display_name.text = "CM7 chord tones";
        }
        else if (userinput.Contains(14) && userinput.Contains(17) && userinput.Contains(21) && userinput.Contains(24))
        {
            RollManager.GetComponent<RollScript>().OnPressLicks.Clear();
            copyList(Dm7ct, 1);
            copyList(Dm7st, 2);
            RollManager.GetComponent<RollScript>().validpress = true;
            RollManager.GetComponent<RollScript>().display_name.text = "Dm7 tones";
        }
        else if (userinput.Contains(14) && userinput.Contains(17) && userinput.Contains(19) && userinput.Contains(23))
        {
            RollManager.GetComponent<RollScript>().OnPressLicks.Clear();
            copyList(G7ct, 1);
            copyList(G7st, 2);
            RollManager.GetComponent<RollScript>().validpress = true;
            RollManager.GetComponent<RollScript>().display_name.text = "G second inv chord tones";
        }
        else if (userinput.Contains(16) && userinput.Contains(19) && userinput.Contains(23) && userinput.Contains(26))
        {
            //then this is a D-based chord tone
            RollManager.GetComponent<RollScript>().OnPressLicks.Clear();
            copyList(Em7ct, 1);
            copyList(Em7st, 2);
            RollManager.GetComponent<RollScript>().validpress = true;
            RollManager.GetComponent<RollScript>().display_name.text = "Em7 chord tones";
        }
        else if (userinput.Contains(17) && userinput.Contains(21) && userinput.Contains(24) && userinput.Contains(27))
        {
            RollManager.GetComponent<RollScript>().OnPressLicks.Clear();
            copyList(FM7ct, 1);
            copyList(FM7st, 2);
            RollManager.GetComponent<RollScript>().validpress = true;
            RollManager.GetComponent<RollScript>().display_name.text = "F7 chord tones";
        }
        else //this is the default case always put this in the lowest when adding more
        {
            RollManager.GetComponent<RollScript>().OnPressLicks.Clear();
            RollManager.GetComponent<RollScript>().validpress = false;
            RollManager.GetComponent<RollScript>().display_name.text = "Unrecognised chord";
        }

    }//end checkifa chord thank you chat gpt for helping me build this function

    //chord sender function
    public void copyList(List<int> cloneMe, int type)
    {
        if (type == 1)
        {
            foreach (var item in cloneMe)
            {
                // LickListToSend.Add(item);
                RollManager.GetComponent<RollScript>().OnPressLicks.Add(item);
            }
        }//end type 1
        if (type == 2)
        {
            foreach (var item in cloneMe)
            {
                // LickListToSend.Add(item);
                RollManager.GetComponent<RollScript>().OnPressBlues.Add(item);
            }
        }//end type 2 

    }//end copylist


    public List<int> GetSwingList(string rootKey)
    {

        List<int> SwingListAcquired = new List<int>();
        int startingnumber;
        int count = 0;
        int currentNumber = 0;

        //====general algorithm

        //get the rootkey in the sequence
        // get all the white keys from the rootkey until the next octave - use contains from blacklist
        //light each key from first to last +7  indices 
        //then from last to first
        //store this into a string
        //based on timing
        //Debug.Log("Getting swing mode of " + rootKey);


        //adding a method here that gives the return scale for easy changing 
        //check for rootkey
        switch (rootKey)
        {
            case "D":
                {
                    startingnumber = 26; //14 is root D
                    break;
                }
            case "G":
                {
                    startingnumber = 31; //19 is root G
                    break;
                }

            case "C":
                {
                    startingnumber = 24; //12 is root C
                    break;
                }

            default:
                {
                    startingnumber = 24;
                    break;
                } //assume C scale 

        }

        //store starting number so we can iterate
        currentNumber = startingnumber;

        //save it for Rollmanager to use
        RollManager.GetComponent<RollScript>().rootKeyIndex = startingnumber;

        //now compute the rest of the whitekeys while skipping stuff in blacklist
        while (count < 8) //cos it gets everything
        {
            if (!blacklist.Contains(currentNumber))
            {
                SwingListAcquired.Add(currentNumber);
                //increment count
                count++;
            }//endifblacklistCount

            //move to the next number
            currentNumber++;

        }//endwhile

        ////verify if we got the right numbers
        //foreach (int number in SwingListAcquired)
        //{
        //    Debug.Log("Swing mode are " + number);
        //}

        return SwingListAcquired;

    }//end getswinglist

    //send the motif list so RollMgr can call it 
    public List<int> GetMotifList() //removed parameter since its global
    {
        //put the jazzmotif in to the motiflist 
        return MotifList;

    }


    //here is a generic yplot plotter - the offset of the Y coordinates 
    public List<int> GenericYPlotter(List<int> List3, int size)
    {
        //important declaration
        RectTransform BarScale = BarPrefab.GetComponent<RectTransform>();

        //clear onwaitylistplotter to be safe
        OnWaitYListPlotter.Clear();

        int offset = 25; //the first elements will have no offset
        int previousOffset = 0; //there is no existing offset
        int newOffset = 0;

        //then do the same but for onwait ylist

        //start fresh
        offset = (int)BarScale.rect.height; //now it is more dynamic
                                            //   30; //the first elements will have no offset
        previousOffset = 0; //there is no existing offset
        newOffset = 0;
        OnWaitYListPlotter.Add(0);
        for (int i = 1; i < List3.Count; i++) //yes begin at 1
        {

            // Debug.Log("Offset to multiply" + LengthListToSend[i-1]);
            newOffset = ((size * offset) / 2) + ((size * offset) / 2) + previousOffset;
            OnWaitYListPlotter.Add(newOffset); //should be of the previous one

            //store the previous one for the next round
            previousOffset = newOffset;
            //previousOffset = LengthListToSend[i - 1] * offset;
            //
            //Debug.Log("New y coord is " + newOffset);
        }//end for loop yplotter

        return OnWaitYListPlotter; //pass to RollScript 

    }//end GenericYPlotter

    //specific yplotter for motifs since the size is different for each
    public List<int> MotifYPlotter(List<int> List3, List<int> List4) //par 1 - y plots, par 2 - sizes 
    {
        //important declaration
        RectTransform BarScale = BarPrefab.GetComponent<RectTransform>();

        //clear onwaitylistplotter to be safe
        MotifYPlots.Clear();

        int offset; //= 30; //the first elements will have no offset
        int previousOffset = 0; //there is no existing offset
        int newOffset = 0;

        //then do the same but for onwait ylist

        //start fresh
        offset = (int)BarScale.rect.height; //now it is more dynamic
                                            //   30; //the first elements will have no offset
        previousOffset = 0; //there is no existing offset
        newOffset = 0;
        MotifYPlots.Add(0);
        for (int i = 1; i < List3.Count; i++) //yes begin at 1
        {
            //size is replaced by the element of List4 which is the corresponding size
            // Debug.Log("Offset to multiply" + LengthListToSend[i-1]);
            newOffset = ((List4[i] * offset) / 2) + ((List4[i] * offset) / 2) + previousOffset;
            MotifYPlots.Add(newOffset); //should be of the previous one

            //store the previous one for the next round
            previousOffset = newOffset;

        }//end for loop yplotter

        return MotifYPlots; //pass to RollScript 

    }//end MotifYPlotter

    //lets have a MotifController Function here
    public void SetMotifDetails(List<(string, int)> LocalMotif)
    {
        //get the numbers for the sizes
        MotifSizes = GetSecondElements(LocalMotif).ToList();
        //now RollMgr should pickup MotifSize

        //get the names for the motiflist and map their piano key index for spawning
        KeyMapper(LocalMotif);

        //now we need to send their YPlots using the GenericYPlotter 

        //send details to rollscript

    }//end setmotifdetails

    //does the mapping and sends the right lists to responsible functions
    public void KeyMapper(List<(string, int)> ReceivedList)
    {
        // general algorithm
        // get chordname and assume it from the c4 octave onwards (since they are licks)
        //then store it in the MotifList
        //send MotifList for use of RolLScript 

        //iterate through each in the list and then assign or map
        foreach (var chord in ReceivedList)
        {

            string chordname = chord.Item1;
            //check chord name and add the corresponding improvs as well
            switch (chordname)
            {
                //these are the permanent harmony spawns 
                //these should be arranged musically  
                case "C3": //c3
                    {
                        MotifList.Add(12);
                        break;
                    }//end C
                case "D3": //d3
                    {
                        MotifList.Add(14);
                        break;
                    }//end d3
                case "E3": //e3
                    {
                        MotifList.Add(16);
                        break;
                    }//end e3
                case "F3": //f3
                    {
                        MotifList.Add(17);
                        break;
                    }//end f3
                case "G3": //g3
                    {
                        MotifList.Add(19);
                        break;
                    }//end g3
                case "A3": //A3
                    {
                        MotifList.Add(21);
                        break;
                    }//end A3
                case "B3": //c3
                    {
                        MotifList.Add(23);
                        break;
                    }//end b3

                // we begin here the motifs and licks - c4 is both  
                case "C4": //c4
                    {
                        MotifList.Add(24);
                        break;
                    }//end C
                case "D4": //d4
                    {
                        MotifList.Add(26);
                        break;
                    }//end
                case "E4": //e4
                    {
                        MotifList.Add(28);
                        break;
                    }//end
                case "F4": //f4
                    {
                        MotifList.Add(29);
                        break;
                    }//end
                case "G4": //g4
                    {
                        MotifList.Add(31);
                        break;
                    }//end
                case "A4": //a4
                    {
                        MotifList.Add(33);
                        break;
                    }//end
                case "B4": //b4
                    {
                        MotifList.Add(35);
                        break;
                    }//end
                case "C5": //c5
                    {
                        MotifList.Add(36);
                        break;
                    }//end C
                case "D5": //c5
                    {
                        MotifList.Add(38);
                        break;
                    }//end D5
                case "E5": //c5
                    {
                        MotifList.Add(40);
                        break;
                    }//end E5
                case "F5": //f5
                    {
                        MotifList.Add(41);
                        break;
                    }//end f
                case "Fs5": //fs5
                    {
                        MotifList.Add(42);
                        break;
                    }//end fs5
                case "A5": //a5
                    {
                        MotifList.Add(45);
                        break;
                    }//end A
                case "B5": //b5
                    {
                        MotifList.Add(47);
                        break;
                    }//end b5
                case "As5": //as5
                    {
                        MotifList.Add(46);
                        break;
                    }//end as5
                case "G5": //g5
                    {
                        MotifList.Add(43);
                        break;
                    }//end g5
                case "Gs5": //c5
                    {
                        MotifList.Add(44);
                        break;
                    }//end gs5
                case "rest":
                    {
                        MotifList.Add(0); //spawn at zero but make it black
                        break;
                    }//
                default:
                    {
                        //TODO: catch "rest" in the piano during spawn
                        MotifList.Add(24); //assume its a C 
                        break;
                    }//end default and rest case
            }//end switch

            //=== this should get the second elements instead 
            //then immediate add their length to lengthlist
            // LengthListToSend.Add(chord.Item2);

        }//end foreach scan of received list

        //work is done since MotifList is a globalvariable

    }//end KeyMapper

    //here is a sample to get the second elements thanks GPT for this
    // the second elements are the times that will be sent for motif sizes 
    public List<int> GetSecondElements(List<(string, int)> tupleList)
    {
        List<int> secondElements = new List<int>();

        foreach (var tuple in tupleList)
        {
            secondElements.Add(tuple.Item2);
        }

        return secondElements;
    }//end get second elements 

    //here is a sample to get the FIRST elements thanks GPT for this
    //the first elements are the keys for key mapping
    public List<string> GetFirst(List<(string, int)> tupleList)
    {
        List<string> firstElements = new List<string>();

        foreach (var tuple in tupleList)
        {
            firstElements.Add(tuple.Item1);
        }

        return firstElements;
    }// end get first elements

    //an equivalent of chord mapper but for s

    ////here is a motif yplot plotter
    //public List<int> MotifYPlotter(List<int> List3, int size)
    //{
    //    //important declaration
    //    RectTransform BarScale = BarPrefab.GetComponent<RectTransform>();

    //    //clear onwaitylistplotter to be safe
    //    OnWaitYListPlotter.Clear();

    //    int offset; //= 30; //the first elements will have no offset
    //    int previousOffset = 0; //there is no existing offset
    //    int newOffset = 0;

    //    //then do the same but for onwait ylist

    //    //start fresh
    //    offset = (int)BarScale.rect.height; //now it is more dynamic
    //                                        //   30; //the first elements will have no offset
    //    previousOffset = 0; //there is no existing offset
    //    newOffset = 0;
    //    OnWaitYListPlotter.Add(0);
    //    for (int i = 1; i < List3.Count; i++) //yes begin at 1
    //    {

    //        // Debug.Log("Offset to multiply" + LengthListToSend[i-1]);
    //        newOffset = ((size * offset) / 2) + ((size * offset) / 2) + previousOffset;
    //        OnWaitYListPlotter.Add(newOffset); //should be of the previous one

    //        //store the previous one for the next round
    //        previousOffset = newOffset;
    //        //previousOffset = LengthListToSend[i - 1] * offset;
    //        //
    //        //Debug.Log("New y coord is " + newOffset);
    //    }//end for loop yplotter

    //    return OnWaitYListPlotter; //pass to RollScript 

    //}//end GenericYPlotter


}//end ChordMgr
