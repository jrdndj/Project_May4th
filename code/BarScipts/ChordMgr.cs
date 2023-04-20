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

    [SerializeField] GameObject InputManager;
    [SerializeField] GameObject ImprovManager;
    [SerializeField] GameObject RollManager;
    //[SerializeField] GameObject CollectionManager; //improv controls

    //=========== ENVIRONMENT RELATED VARIABLES ==========/

    //where the chords are mapped
    //doesnt have to change, we send their key mappings
    List<List<int>> ChordListToSend = new List<List<int>>();
    List<List<int>> JazzListToSend = new List<List<int>>();
    List<List<int>> BluesListToSend = new List<List<int>>();
    List<int> LengthListToSend = new List<int>();
    List<int> YListPlotter = new List<int>(); //this is for the Y positions of the spawns that RollMgr will need


    //=========== CHORD RELATED VARIABLES ==========/

    //I think this should be moved to InputMgr just to be sure. 
    List<int> blacklist = new List<int>() { 1, 3, 6, 8, 10, 13, 15, 18, 20, 22, 25, 27, 30, 32, 34, 37, 39, 42, 44, 46, 49, 51, 54, 56, 58 };

    //list of some white only chords and their chord licks
    //D3 F3 A3 C4 --- D4 F4 A4 C5 - ok mapped! - higher D5 F5 A5 C6
    static List<int> Dm7 = new List<int>() { 14, 17, 21, 24 };
    static List<int> Dm7ct = new List<int>() { 26, 29, 33, 36, 38, 41, 45, 48 };
    static List<int> Dm7hs = new List<int>() { 25, 28, 32, 35 };
    static List<int> Dm7sa = new List<int>() { 28, 31, 35, 38 };
    static List<int> Dm7st = new List<int>() { 26, 29, 33, 36, 38, 41, 45, 48 }; //fix this! 

    //C3 E3 G3 B3 --- C4 E4 G4 B4 - ok mapped! - C5 E5 G5 B5
    static List<int> CM7 = new List<int>() { 12, 16, 19, 23 };
    static List<int> CM7ct = new List<int>() { 24, 28, 31, 35, 36, 40, 43, 47 };
    static List<int> CM7hs = new List<int>() { 23, 27, 30, 34 };
    static List<int> CM7sa = new List<int>() { 26, 29, 33, 36 };

    //we decommission this since we use G43-second inversion instead for better crabbing of G7
    ////G3 B3 D4 F4 --- G4 B4 D5 F5 - ok mapped! 
    //static List<int> G7 = new List<int>() { 19, 23, 26, 29 };
    //static List<int> G7ct = new List<int>() { 31, 35, 38, 41 };

    //D3 F3 G3 B3 --- D4 Fs4 G4 B4 - ok mapped! - D5 Fs5 G5 B5
    static List<int> G7 = new List<int>() { 14, 17, 19, 23 };
    static List<int> G7ct = new List<int>() { 26, 29, 31, 35, 38, 41, 43, 47 };
    static List<int> G7hs = new List<int>() { 25, 30, 34 }; //removed 29 here cos of overlap
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
    static List<int> A7hs = new List<int>() { 20, 24, 27, 30 };
    static List<int> A7sa = new List<int>() { 23, 26, 29, 33 };
    static List<int> A7st = new List<int>() { 21, 25, 28, 31, 33, 37, 40, 43 }; //fix this

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
            Debug.Log("Mapping " + chord );
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
                        break;
                    }//end Cm7
                case "Cm7":
                    {
                        ChordListToSend.Add(Cm7);
                        JazzListToSend.Add(Cm7ct);
                        BluesListToSend.Add(C7st);
                        break;
                    }//end Cm7
                case "CM7":
                    {
                        ChordListToSend.Add(CM7);
                        JazzListToSend.Add(CM7ct);
                        BluesListToSend.Add(C7st);
                        break;
                    }//end CM7
                case "Dm7":
                    {
                        ChordListToSend.Add(Dm7);
                        JazzListToSend.Add(Dm7ct);
                        BluesListToSend.Add(Dm7st);
                        break;
                    }//end Dm7
                case "Em7":
                    {
                        ChordListToSend.Add(Em7);
                        JazzListToSend.Add(Em7ct);
                        BluesListToSend.Add(E7st);
                        break;
                    }//end Em7
                case "FM7":
                    {
                        ChordListToSend.Add(FM7);
                        JazzListToSend.Add(FM7ct);
                        BluesListToSend.Add(F7st);
                        break;
                    }//end FM7
                case "G7":
                    {
                        ChordListToSend.Add(G7);
                        JazzListToSend.Add(G7ct);
                        BluesListToSend.Add(G7st);
                        break;
                    }//end G7
                case "A7":
                    {
                        ChordListToSend.Add(A7);
                        JazzListToSend.Add(A7ct);
                        BluesListToSend.Add(A7st);
                        break;
                    }//end A7 
                case "Am7":
                    {
                        ChordListToSend.Add(Am7);
                        JazzListToSend.Add(Am7ct);
                        BluesListToSend.Add(Am7st);
                        break;
                    }//end Am7

                    //this is for both
                case "rest":
                default: 
                    {
                        //TODO: catch "rest" in the piano during spawn
                        ChordListToSend.Add(Am7);
                        JazzListToSend.Add(Am7ct);
                        BluesListToSend.Add(Am7st);
                        break;
                    }//end default and rest case
            }//end switch
            
            //then immediate add their length to lengthlist
            LengthListToSend.Add(chord.Item2);
            
        }//end foreach scan of received list

        //now that we have the details we need to send them to InputMgr and ImprovMgr
        //for final sequencing
        //InputManager.GetComponent<InputMgr>().ListReceiver(ChordListToSend);

        //pass things to ImprovManager -we did this cos we received two things
        // ImprovManager.GetComponent<ImprovMgr>().ChordList = ChordListToSend;
        //   ImprovManager.GetComponent<ImprovMgr>().LickList = JazzListToSend;

        //we also need to send the time details!
        //   ImprovManager.GetComponent<ImprovMgr>().TimeReceiver(LengthListToSend);

        //we send to ImprovMgr for the keys to highlight
        ImprovManager.GetComponent<ImprovMgr>().ListReceiver(ChordListToSend, JazzListToSend);
        //also send it straight to RollManager for the keys to spawn 
        RollManager.GetComponent<RollScript>().ListReceiver(ChordListToSend, JazzListToSend);


        //map ycoords of spawn and pass to RollMgr too
        MapSpawnYCoords();

        //ImprovManager still needs lengthlist for the validity 
        ImprovManager.GetComponent<ImprovMgr>().TimeReceiver(LengthListToSend);

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

        //LenghListToSend is received as ElapsedList
        //ChordListToSend as ChordList

        int offset = 60; //the first elements will have no offset
        //int multiplier = 60; //for the scale based on their length

        //store the 0 offset for the 0th element immediately
        YListPlotter.Add(offset);
        //we use for loop instead for for each so we control stuff
        for (int i = 1; i <= LengthListToSend.Count; i++) //yes begin at 1
        {
            YListPlotter.Add(LengthListToSend[i - 1] * offset); //should be of the previous one
        }//end for loop yplotter

        //then send everything to RollMgr
        RollManager.GetComponent<RollScript>().ReceiveYPlots(YListPlotter);
    }//end MapSpawnYCoords

    //// The main goal is to pass and make vital chord info public or accessible
    //void Start()
    //{
      
      
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    //there is really nothing to update there. 
    //}
}//end ChordMgr
