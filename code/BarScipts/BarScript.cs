//this script was from Kate Sawada's tutorial
// more info https://github.com/KateSawada/midi_visualizer_tutorial_01/blob/vol01/Assets/Scripts/BarScript.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    //public GameObject newButton; 

    const int keysCount = 68; //changed from 88

    //adding a enum check for the blackkeys
    List<int> blacklist = new List<int>() {1, 3, 6, 8, 10, 13, 15, 18, 20, 22, 25, 27, 30, 32, 34, 37, 39, 42, 44, 46, 49, 51, 54, 56, 58 };
    List<int> whitelist = new List<int>() {0, 2, 4, 5, 7, 9, 11, 12, 14, 16, 17, 19, 21, 23, 24, 26, 28, 29, 31, 33, 35, 36, 38, 40, 41, 43, 45, 47, 48, 50, 52, 53, 55, 57, 59, 60};

    //so we can map each key in the virtual piano to the key
    [SerializeField] List<GameObject> whiteKeys = new List<GameObject>();
    [SerializeField] List<GameObject> blackKeys = new List<GameObject>();

    [SerializeField] GameObject barManager;
    GameObject[] barsPressed = new GameObject[keysCount]; // bars linked to the pressed key
    [SerializeField] List<GameObject> barsReleased = new List<GameObject>(); // bars linked to the released key
        
    bool[] isKeyPressed = new bool[keysCount];

    float barSpeed = (float)0.15; //from 0.05
    float upperPositionLimit = (float)1000; //changed from 100

    //these are the color related objects
    public ColorBlock theColor;
    public Button theButton;

    // Start is called before the first frame update
    void Start()
    {

        //for the midi related 
        for (int i = 0; i < 68; i++)
        {
            // initialize: keys are not pressed
            isKeyPressed[i] = false;
            barsPressed[i] = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // currently pressed keys
        for (int i = 0; i < 68; i++)
        {
            if (isKeyPressed[i] && barsPressed[i] != null)
            {
                Vector3 scale = barsPressed[i].transform.localScale;
                scale.y += barSpeed * 2; //changed from 2
                barsPressed[i].transform.localScale = scale;
                Vector3 pos = barsPressed[i].transform.position;
                pos.y += barSpeed;
                //pos.x += 20; //added this to change x
                barsPressed[i].transform.position = pos;
            }
        }

        // released keys
        for (int i = barsReleased.Count - 1; i >= 0; i--)
        {
            Vector3 pos = barsReleased[i].transform.position;

            // destroy bars when it reached upperPositionLimit
            if (pos.y+barsReleased[i].transform.localScale.y > upperPositionLimit)
            {
                //Destroy(barsReleased[i]); //commented for now
                Destroy(Instantiate(barsReleased[i])); //try lang
                barsReleased.RemoveAt(i);
            }
            else
            {
                pos.y += barSpeed * 2; //changed from 2
                barsReleased[i].transform.position = pos;
            }
        }
    }

    public void onNoteOn(int noteNumber, float velocity)
    {
        //color related blocks
        //higlightkey
        //get button information for color transformation
        theButton = GetComponent<Button>();
        theColor = GetComponent<Button>().colors;

        // clearfy that the key is pressed
        isKeyPressed[noteNumber] = true;

        //Vector3 wkpos = whiteKeys[noteNumber].transform.position;
       // Debug.Log("noteNumber is " + noteNumber);
        //looks like 0 or 2 so can be mapped

        //xcord should be x of whitekeys
        //barsPressed[noteNumber].transform.position = new Vector3(whiteKeys[noteNumber].transform.localPosition.x, 0, 0);

        // create bar object
        GameObject barPrefab;
        barPrefab = (GameObject)Resources.Load("Prefab/Bar");
        barsPressed[noteNumber] = Instantiate(barPrefab);
        //should instantiate it on the Xcoord of the gameobject

        //barsPressed[noteNumber].transform.position = new Vector3(noteNumber, 0, 0);
        //return this if it doesnt work

        //we are now spawning at the local position of the button in the scene
        if (blacklist.Contains(noteNumber))
        {
            //use blackKeys BUT get their index
            //blacklist.IndexOf(noteNumber); //and use this as blackKey index
            barsPressed[noteNumber].transform.position = new Vector3(blackKeys[blacklist.IndexOf(noteNumber)].transform.localPosition.x, 0, 0);
            barsPressed[noteNumber].transform.SetParent(barManager.transform, true);

            //color changing methods
            theColor.pressedColor = Color.green;
            blackKeys[blacklist.IndexOf(noteNumber)].GetComponent<Image>().color = Color.green;
            //blackKeys[blacklist.IndexOf(noteNumber)].gameObject.SendMessage("onClick", 1);


        }
        else
        {   //use whiteKeys
            //we shouldnt put -36
            barsPressed[noteNumber].transform.position = new Vector3(whiteKeys[whitelist.IndexOf(noteNumber)].transform.localPosition.x, 0, 0);
            barsPressed[noteNumber].transform.SetParent(barManager.transform, true);

            //color changing methods
            theColor.pressedColor = Color.green;
            whiteKeys[whitelist.IndexOf(noteNumber)].GetComponent<Image>().color = Color.green;
            //blackKeys[blacklist.IndexOf(noteNumber)].gameObject.SendMessage("onClick", 1);
        }

    }

    public void onNoteOff(int noteNumber)
    {
        barsReleased.Add(Clone(barsPressed[noteNumber]));
        DestroyImmediate(barsPressed[noteNumber]);

        isKeyPressed[noteNumber] = false;
    }

    GameObject Clone(GameObject obj)
    // reference: https://develop.hateblo.jp/entry/2018/06/30/142319
    {
        var clone = GameObject.Instantiate(obj) as GameObject;
        clone.transform.parent = obj.transform.parent;
        clone.transform.localPosition = obj.transform.localPosition;
        clone.transform.localScale = obj.transform.localScale;
        return clone;
    }

}