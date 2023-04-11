/*
 * This class manages the following features:
 * - pulling chord-key info from ChordMgr
 * - providing the right onNoteOn and onNoteOff response
 * - scoring mechanisms 
 * - detecting chord from pressed keys
 * - identifying the right improv based on detected chord
 * - manages all visualizations related to improvisation (and keys)
 * 
 * dependent to: ChordMgr, RollManager
 * dependent of: InputMgr
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //added for colors

public class ImprovMgr : MonoBehaviour
{
    //======= important environment variables =======/

    [SerializeField] GameObject ImprovManager;
    [SerializeField] GameObject RollManager;

    //public GameObject RollManager;

    const int keysCount = 61; //or is it 68?

    //this helps the mapping of keys similar to that midi hardware
    [SerializeField] List<GameObject> pianoKeys = new List<GameObject>();
    //List<GameObject> pianoKeys = RollScript

    bool[] improvToHighlight = new bool[keysCount];
    bool[] melodyToHighlight = new bool[keysCount];
    bool[] isKeyHighLighted = new bool[keysCount]; //for error checking
    bool[] isKeyPressed = new bool[keysCount]; //for spawning


    //=========== COLOR RELATED VARIABLES ==========/
    //these are the color related objects
    public ColorBlock theColor;
    public Button theButton;
    Color32 improvpink = new Color32(255, 150, 234, 255);
    Color32 yellow = Color.yellow;
    Color32 belowpink = new Color32(75, 0, 130, 255);  //this is indigo akshully
    Color32 blues = new Color32(65, 105, 225, 255); // this is for the blues blue

    //======= useful Improv-related functions ======/

    //when user presses a key on the clavinova (from MIDIScript)
    public void onNoteOn(int noteNumber, float velocity)
    {
        //do we even need this?? 
        //isKeyPressed[noteNumber] = true;

        //that's it - red if not highlighted 
        //highlights red if key pressed is not a lick or not in the roll
        //if ((!isKeyHighLighted[noteNumber] && !improvToPress[noteNumber]) || !melodyToPress[noteNumber])
        //i just flipped the logic just in case 
        if (isKeyHighLighted[noteNumber] && (melodyToHighlight[noteNumber] || improvToHighlight[noteNumber]))
        {
            // pianoKeys[noteNumber].GetComponent<Image>().color = Color.white;
        }//endif
        else
        {
            pianoKeys[noteNumber].GetComponent<Image>().color = Color.red;
        }//endif
         //you need to forget the melodies until a new one comes

        //totally something else
        //if (melodyToHighlight[noteNumber]==true)
        //{
        //    melodyToHighlight[noteNumber] = false;
        //}//end forget melody
        //else it's wrong that simple

    }//endonNoteOn;

    //when user releases a pressed key as per MIDIScript 
    public void onNoteOff(int noteNumber)
    {
        //FIRST! - the key is no longer pressed so set it to false duh
        isKeyPressed[noteNumber] = false;

        ////THEN return to the appropriate color

        ////if key was in lick and was pressed revert back to pink
        ////if (improvToPress[noteNumber] && improvToHighlight[noteNumber])
        ////if key released is in liscklist? how to say this? 
        ////if(LickList[ctr].Contains(noteNumber) && !spawnNew)
        ////if (improvToHighlight[noteNumber] && !melodyToPress[noteNumber] && !spawnNew)
        // if (!isKeyHighLighted[noteNumber])
        // {
        //pianoKeys[noteNumber].GetComponent<Image>().color = Color.black;
        // improvToHighlight[noteNumber] = false; //change to false 
        //}
        if (improvToHighlight[noteNumber] == true)
        {
            pianoKeys[noteNumber].GetComponent<Image>().color = improvpink;
            //==========for blues
            //pianoKeys[noteNumber].GetComponent<Image>().color = blues;
        }
        else if (melodyToHighlight[noteNumber] == true)
        {
            pianoKeys[noteNumber].GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            pianoKeys[noteNumber].GetComponent<Image>().color = Color.black;
        }

    }//end bars pressed on note off 


    //lights up a group of keys based on the licks 
    public void HighlightLicks(List<int> lickset, Color32 highlightcolor, int spawntype)
    {
        //show all 4 as a for loops
        for (int i = 0; i < lickset.Count; i++)
        {
            //HIGHLIGHT PINK WHAT SHOULD BE PINK NOTHING MORE

            //highlight piano key based on color and spawntype
            pianoKeys[lickset[i]].GetComponent<Image>().color = highlightcolor;

            //store information for onNoteOff
            if (spawntype == 2) //2 if improv
            {
                improvToHighlight[lickset[i]] = true;
            }
            else //1 if melody
            {
                melodyToHighlight[lickset[i]] = true;
            }

            //we update flag for error checking
            isKeyHighLighted[lickset[i]] = true; //this works never remove this

        }//endfor
        //checkHighlights++;
        //return lickset;
    }//endHighlightLicks

    //need a cleanup function
    public void CleanupKeyboard()
    {
        //show all 4 as a for loops
        for (int i = 0; i < keysCount; i++)
        {
            pianoKeys[i].GetComponent<Image>().color = Color.black;
        }//endfor
        //return lickset;
    }//endremovelicks


    // Start is called before the first frame update
    void Start()
    {
        //routine cleanup
        CleanupKeyboard();

        //we should call some functions that passes the y coordinates to RollScript 
    }

    // Update is called once per frame
    void Update()
    {
        CleanupKeyboard();
        
    }

    //some function to pass them or return in a common object
    public List<int> PassPianoCoordinates()
    {
        List<int> KeyIndices = new List<int>();

        //store each index in a keyIndex list and be ready to pass 
        foreach(var i in pianoKeys)
        {
            KeyIndices.Add(pianoKeys.IndexOf(i));
        }
        return KeyIndices;
    }
}
