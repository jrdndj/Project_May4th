//this script was from Kate Sawada's tutorial
// more info https://github.com/KateSawada/midi_visualizer_tutorial_01/blob/vol01/Assets/Scripts/BarScript.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    const int keysCount = 68; //changed from 88
    public int mode; //can be 1, 2, or 3

    //serialized field for the greenline
    [SerializeField] GameObject GreenLine;

    //so we can map each key in the virtual piano to the key
    [SerializeField] List<GameObject> pianoKeys = new List<GameObject>();

    [SerializeField] GameObject barManager;
    //[SerializeField] GameObject rollManager;

    //for the reverse piano roll
    GameObject[] barsPressed = new GameObject[keysCount]; // bars linked to the pressed key
    [SerializeField] List<GameObject> barsReleased = new List<GameObject>(); // bars linked to the released key

    bool[] isKeyPressed = new bool[keysCount];

    //float barSpeed = (float)0.65; //from 0.05 0.15 was ok 
    //float upperPositionLimit = (float)700; //changed from 100

    //these are the color related objects
    public ColorBlock theColor;
    public Color alpha;
    public Button theButton;

    // Start is called before the first frame update
    void Start()
    {
        //randomize function for next chord tone
        var rand = UnityEngine.Random.Range(0, 6); //there are 6 in the list

        //for the midi related 
        for (int i = 0; i < 61; i++)
        {
            // initialize: keys are not pressed
            isKeyPressed[i] = false;
            barsPressed[i] = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //randomize function for next chord tone
        var rand = UnityEngine.Random.Range(0, 6); //there are 6 in the list s

        //int allkeyscleared = 0;

        //from this line below  are the reverse piano roll commands  ========
        //for (int i = 0; i < 68; i++)
        //{
        //    if (isKeyPressed[i] && barsPressed[i] != null)
        //    {
        //        Vector3 scale = barsPressed[i].transform.localScale;
        //        scale.y += barSpeed * 2; //changed from *2 
        //        barsPressed[i].transform.localScale = scale;
        //        Vector3 pos = barsPressed[i].transform.position;
        //        pos.y += barSpeed;
        //        barsPressed[i].transform.position = pos;
        //    }
        //}//endforiskeyPressed

        ////released keys
        //for (int i = barsReleased.Count - 1; i >= 0; i--)
        //{
        //    Vector3 pos = barsReleased[i].transform.position;

        //    // destroy bars when it reached upperPositionLimit
        //    if (pos.y + (barsReleased[i].transform.localScale.y / 2) > upperPositionLimit)
        //    {
        //        Destroy(barsReleased[i]);
        //        barsReleased.RemoveAt(i);
        //    }//endifbarsreleased
        //    else
        //    {
        //        pos.y += barSpeed * 2; //changed from 2
        //        barsReleased[i].transform.position = pos;
        //    }//end else bars released
        //}//end checking of all bars

        //above this line are the reverse piano roll commands  ========

        //check all keys
        //  allkeyscleared = checkAllKeysIfWhite();

        //show different suggestive higlights here 
        //if (allkeyscleared == 0) //0 means all keys are white
        //{

        //    //then we can call another highlight
        //    //HighlightChords(ChordList[rand]);
        //    //HighlightLicks(LickList[rand]);
        //    if (mode == 1 && chordctr < ChordList.Count)
        //    {
        //        HighlightChords(ChordList[chordctr], yellow);
        //        chordctr++;

        //    }
        //    if (mode == 2 && chordctr < ChordList.Count && lickctr < LickList.Count)
        //    {
        //        HighlightChords(ChordList[chordctr], yellow);
        //        HighlightLicks(LickList[lickctr], improvpink);
        //        chordctr++;
        //        lickctr++;
        //    }
        //    if (mode == 3 && lickctr < LickList.Count)
        //    {
        //        HighlightLicks(LickList[lickctr], improvpink);
        //        lickctr++;
        //    }
        //    if (mode == 4)//no need for additional criteria since this will be unique
        //    {
        //        HighlightLicks(LickList[LickList.Count - 1], improvpink); //get the latest one 
        //    }//end of by press mode improv suggestion

        //    if (chordctr == 4) chordctr = 0;
        //    if (lickctr == 4) lickctr = 0;


        //}//endcheckkeyswhite
    }//end on note off

    public void onNoteOn(int noteNumber, float velocity)
    {
        //empty on purpose
    }//end onNoteOn

    public void onNoteOff(int noteNumber)
    {
        //this is where we put the clearing of all keys
        theButton = pianoKeys[noteNumber].GetComponent<Button>();
        theColor = pianoKeys[noteNumber].GetComponent<Button>().colors;
        theColor.pressedColor = Color.white;
        pianoKeys[noteNumber].GetComponent<Image>().color = Color.black;
    }//end bars pressed on note off 

    //lights up a group of keys based on the licks 
    public List<int> HighlightChords(List<int> chordset, Color32 color)
    {
        //show all 4 as a for loop
        for (int i = 0; i < chordset.Count; i++)
        {
            theButton = pianoKeys[chordset[i]].GetComponent<Button>();
            theColor = pianoKeys[chordset[i]].GetComponent<Button>().colors;
            theColor.highlightedColor = color;
            pianoKeys[chordset[i]].GetComponent<Image>().color = color;
        }//endfors
        return chordset;
    }//endHighlightMelodyChords

    //we need this later on 
    GameObject Clone(GameObject obj)
    // reference: https://develop.hateblo.jp/entry/2018/06/30/142319
    {
        var clone = GameObject.Instantiate(obj) as GameObject;
        clone.transform.parent = obj.transform.parent;
        clone.transform.position = obj.transform.position; //changed from LocalPosition
        clone.transform.localScale = obj.transform.localScale;
        return clone;
    }

    public int checkAllKeysIfWhite()
    {
        int clear = 0;
        //this is scanning through the white list and changing them to white 
        for (int i = 0; i < 61; i++)
        {
            theButton = pianoKeys[i].GetComponent<Button>();
            theColor = pianoKeys[i].GetComponent<Button>().colors;
            if (pianoKeys[i].GetComponent<Image>().color != Color.black)
            {
                clear++;
            }//endif
        }//endfor white keys

        return clear;
    }//endcheck all checks if white


}//endclass