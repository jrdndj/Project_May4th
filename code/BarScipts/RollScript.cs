//some portions of this code was inspired from https://github.com/KateSawada/midi_visualizer_tutorial_01/blob/vol01/Assets/Scripts/BarScript.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //for ToList
using UnityEngine.UI; //added for colors

public class RollScript : MonoBehaviour
{
    //============ ENVIRONMENT RELATED VARIABLES =============/

    //this helps the mapping of keys similar to that midi hardware
    [SerializeField] List<GameObject> pianoKeys = new List<GameObject>();

    //an important element to manage all children of spawns
    [SerializeField] GameObject rollManager;

    //[SerializeField] GameObject piano; //should be the piano lets sees

    //this is to manage spawns
    GameObject[] spawnedBars = new GameObject[keysCount]; //this w as original
    //GameObject[] spawnedBars = new GameObject[1000];
    GameObject[] spawnedLines = new GameObject[keysCount];

    //for the lower position limit
    public GameObject green_line; //formerly 0 -85 0

    //for the spawn point
    public GameObject spawn_top; // 0 350 0

    //for the destory point
    public GameObject destroy_point;

    //for spawning the line
    public GameObject spawn_mid;

    //if all ready
    public bool ReadyToSpawn = false;

    //for the chord name to display
    //public GameObject chord_name; 
   [SerializeField] private Text display_name;

    //help us check for errors and control the number of spawns
    const int keysCount = 61; //or is it 68?
    public float lowerpositionlimit; //changed to float                              
    public int ctr;  //an internal counter
    public int programctr = 0;
    public float spawnpoint; //y coord of the spawnpoint
    public float spawnmid; //y coord for the spawndmid
    bool[] isKeyPressed = new bool[keysCount]; //for spawning
    bool[] isKeyHighLighted = new bool[keysCount]; //for error checking
    bool spawnNew = true; //flag to trigger next spawn or not
    public bool highlightNow = false; //flag to trigger higlighting
    //bool turnOfflights = false; 
    bool decreasing = false; //for the bars spawning
    bool destroyed = false;
    //some crucial variables
    public int spawnCount = 0;
    public int mode = 0; 

    //height of 120 means it runs for 2 seconds.
    //height of 60 means it runs for 1 second

    //to seperate melody and improv pressing
    bool[] melodyToHighlight = new bool[keysCount];
    bool[] improvToHighlight = new bool[keysCount];

    List<int> lineMapList = new List<int>();

    public List<int> YPlotsReceived = new List<int>();
    public List<int> KeyLengthsReceived = new List<int>();

    float barSpeed = (float)0.682; //from 0.05 0.65 was ok //0.15 is still too fast

    //for the co routines
    private IEnumerator spawn;
    private IEnumerator roll;

    //=========== COLOR RELATED VARIABLES ==========/
    //these are the color related objects
    public ColorBlock theColor;
    public Button theButton;
    Color32 improvpink = new Color32(255, 150, 234, 255);
    Color32 yellow = Color.yellow;
    Color32 belowpink = new Color32(75, 0, 130, 255);  //this is indigo akshully
    Color32 blues = new Color32(65, 105, 225, 255); // this is for the blues blue

    ////=========== CHORD RELATED VARIABLES ==========/

    ////still need my enum for black key spawning
    List<int> blacklist = new List<int>() { 1, 3, 6, 8, 10, 13, 15, 18, 20, 22, 25, 27, 30, 32, 34, 37, 39, 42, 44, 46, 49, 51, 54, 56, 58 };

    ////I need you but I will need to refactor you
    List<string> ChordNames = new List<string>();
    public List<List<int>> ChordList = new List<List<int>>();
    public List<List<int>> LickList = new List<List<int>>();
    public List<List<int>> HalfStepList = new List<List<int>>();
    public List<List<int>> StepAboveList = new List<List<int>>();
    public List<List<int>> BluesList = new List<List<int>>();
    //public List<int> YPlots = new List<int>();

    //a function that receives from ImprovMgr
    public void ListReceiver(List<List<int>> List1, List<List<int>> List2, List<List<int>> List3)
    {
        //assign chordlist
        foreach (var item in List1)
        {
            //  Debug.Log("Passing " + item);
            ChordList.Add(item);
        }//endchordlist

        //assign licklist
        foreach (var item in List2)
        {
            //   Debug.Log("Passing " + item);
            LickList.Add(item);
        }//endlicklist

        //assign licklist
        foreach (var item in List3)
        {
            //Debug.Log("Passing " + item);
            BluesList.Add(item);
        }//endlicklist 

        //return ListReceived;
    }//endListReceiver

    public void NamesReceiver(List<string> NamesReceived)
    {
       
        for (int i = 0; i < NamesReceived.Count; i++)
        {
            ChordNames.Add(NamesReceived[i]);
            //Debug.Log("Name added " + ChordNames[i]);
        }
        ChordNames.Add("rest");
    }//end name received

    //=============== This is the new spawn ========/

    public bool SpawnKeys()
    {
        /*
         * receive chordlist
         * init prefabs black and white
         * iterate thru ChordList, and access Length
         * the Y in length[i] is the y of all elements in the chordlist[i]
         * then iterate with new offset from length[i+1] and so on
         * **/

        //i can recycle spawnRoll and iterate thru it
        for (int ctr = 0; ctr < ChordList.Count; ctr++)
        {
            SpawnRoll(ChordList[ctr], yellow, 1, KeyLengthsReceived[ctr], YPlotsReceived[ctr]);
            // SpawnRoll(LickList[ctr], improvpink, 2, KeyLengthsReceived[ctr], YPlotsReceived[ctr]);
            //Debug.Log("Spawned chords" + ChordList[ctr]);
        }//end spawn

        //then end all spawns
        return false;

    }//end spawnKeys

    //THIS IS PART OF STEP 01
    //=== we need to change this - we spawn everything at once and it is lined up
    //there. this we dont have any exception and we just roll everything down
    //destroy them as well 
    //this method is to initialize important stuff for the piano roll
    public void SpawnRoll(List<int> indexList, Color32 spawncolor, int spawntype, int scale, int offset)
    {
        //for debugging purposes 
        int success = 0;
        bool isBlackPrefab = false;

        //set prefabs
        GameObject whitePrefab, blackPrefab;
        whitePrefab = (GameObject)Resources.Load("Prefab/whitekeyprefab");
        blackPrefab = (GameObject)Resources.Load("Prefab/blackkeyprefab");


        //scan through the list of keys to spawn based on type
        for (int i = 0; i < indexList.Count; i++)
        {

            //get the transform position of the elements
            //it shouldnt matter anyway
            Vector3 keypos = pianoKeys[indexList[i]].transform.localPosition;

            //add to list the index for line mapping later
            lineMapList.Add(indexList[i]);

            //get the position of the element in indexList
            //if index is in blacklist, then spawn blackPrefab, else whitePrefab
            if (blacklist.Contains(indexList[i])) //change this later on 
            {
                //spawn a blackprefab
                spawnedBars[spawnCount] = Instantiate(blackPrefab);
                //we need this for the colors later
                isBlackPrefab = true;
                //debugging purposes only
                success++;
            }//endif
            else
            {
                //spawn a whiteprefab
                spawnedBars[spawnCount] = Instantiate(whitePrefab);
                //debugging purposes only
                isBlackPrefab = false;
                success++;
            }//end whitePrefabs

            //set parent for proper positioning
            spawnedBars[spawnCount].transform.SetParent(rollManager.transform, true);


            //store information and spawn on location regardless of prefab
            // change size of spawned key
            spawnedBars[spawnCount].transform.localScale = new Vector3(pianoKeys[spawnCount].transform.localScale.x, scale, 1);
            // spawnedBars[spawnCount].GetComponent<RectTransform>().
            //set color to yellow or pink based on type
            spawnedBars[spawnCount].GetComponent<Image>().color = spawncolor;

            //make colors darker if dark prefab
            if (isBlackPrefab)
            {
                Color darkerColor = new Color();
                darkerColor = (Color)spawncolor * 0.75f;
                spawnedBars[spawnCount].GetComponent<Image>().color = darkerColor;
            }//endisBlackprefabcheck

            //get the actual size and then get the half of it
            // RectTransform SpawnScale = spawnedBars[spawnCount].GetComponent<RectTransform>();
            //  SpawnScale.sizeDelta = new Vector2(SpawnScale.sizeDelta.x, SpawnScale.sizeDelta.y - barSpeed * 5);


            //puts spawn in it proper position //add offset 
            spawnedBars[spawnCount].transform.localPosition = new Vector3(keypos.x, spawnpoint + offset, keypos.z);
            //spawnpoint has y 338
            //here, they must be positioned based on their sequence
            //upnext: a visual way to arrange them considering the time it takes
            // = how long does it take to arrive 

            //increase count of spawn cos of serializedfield
            spawnCount++;
            //if (spawntype == 1)
            //{
            //    //remember the melodybars that should be pressed
            //   // melodyToHighLight[indexList[i]] = true;
            //}
        }//endfor iterating loop list
        if (success == 4)
        {
            //Debug.Log("Chord spawned");
            spawnNew = false;
        }

    }//end spawnRoll

    //STEP 02
    //rolls the spawned keys to the greenline
    public void RollKeys()
    {
        //show their name
        display_name.text = ChordNames[ctr];
        

        //roll the objects spawns downward
        for (int i = 0; i < spawnedBars.Length; i++) //based on the current #
        {
            

            //if there are bars spawned keep rolling )
            if (spawnedBars[i] != null)
            {
                Vector3 pos = spawnedBars[i].transform.position;
                RectTransform SpawnScale = spawnedBars[i].GetComponent<RectTransform>();

                //changed to -= since we need them to go down

                pos.y -= barSpeed;

                //it affects the speed of all bars so this shouldnt be the case 
                //if (!decreasing)
                //{
                //    pos.y -= barSpeed;
                //}
                //else
                //{
                //    pos.y -= 0.43f; //0.43f
                //}

                spawnedBars[i].transform.position = pos;

               // ShowMapLines(lineMapList[i], spawnedBars[i].GetComponent<Image>().color);

                //STEP 03

                //show map lines when near 
                //if ((spawnedBars[i].GetComponent<RectTransform>().localPosition.y - 60) <= spawn_mid.GetComponent<RectTransform>().localPosition.y)
                //{
                //    //get color then pass it too
                //    //show lines
                //    //Debug.Log("Show mapping lines");
                //    ShowMapLines(lineMapList[i], spawnedBars[i].GetComponent<Image>().color);

                //    //clear up keyboard
                //    CleanupKeyboard();

                //}//endif

                //when they reach the green line                //there was a -30 here 
                if ((spawnedBars[i].GetComponent<RectTransform>().localPosition.y - (SpawnScale.rect.height+(SpawnScale.rect.height/2))) <= green_line.GetComponent<RectTransform>().localPosition.y)
                {
                    
                    //highlight here when they reach the green line
                    //hide MapLines
                    // Debug.Log("Hiding map lines");
                   // HideMapLines(lineMapList[i]);
                    highlightNow = true;

                    //==== this never work so never do this again
                    //spawnedBars[i].transform.localScale -= new Vector3(0, barSpeed, 0);

                    //==== this is using size delta approach ===== /
                    //start reducing in size                                                    // * 5
                   // SpawnScale.sizeDelta = new Vector2(SpawnScale.sizeDelta.x, SpawnScale.sizeDelta.y-5);
                    SpawnScale.sizeDelta = new Vector2(SpawnScale.sizeDelta.x, SpawnScale.sizeDelta.y - barSpeed);

                    //dont move me anymore
                    decreasing = true;

                    if (SpawnScale.rect.height <= 0)
                    {
                        //CleanupKeyboard();
                        highlightNow = false;
                        destroyed = true;
                       // ctr++;
                    }
                }//endif

                ////since we are 2D, we use RectTransform and get the localPosition since we are in real-time               //-120 here
                if ((spawnedBars[i].GetComponent<RectTransform>().localPosition.y - (SpawnScale.rect.height + (SpawnScale.rect.height))) <= destroy_point.GetComponent<RectTransform>().localPosition.y)
                {
                    //destroy then highlight 
                    Destroy(spawnedBars[i]);
                    //highlightNow = true;

                    //clear map lines too
                   //clearMapLines(lineMapList[i]);

                    //but we can spawn something new now
                    spawnNew = true;
                    decreasing = false;

                    //CleanupKeyboard();

                    spawnCount--;
                    //then add stuff on improvtoHighlight
                }//endif check contact green point

                //if it reaches the destroy point then destory and spawn 
                //if ((spawnedBars[i].GetComponent<RectTransform>().localPosition.y - 60) <= destroy_point.GetComponent<RectTransform>().localPosition.y)
                //{
                //    //Destroy(spawnedBars[i]);
                //    //reduce spawn count instead of resetting it

                //    //but we can spawn something new now
                //   // spawnNew = true;
                //    //add some timing elements here instead

                //   // spawnCount--;
                //}//destroy point
            }//endif movement
        }//end loop for to generate all spawns
    }//end roll keys


    //======= useful Improv-related functions ======/

    //these are error checking for improv
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
    }//endonNoteOn;

    //when user releases a pressed key as per MIDIScript 
    public void onNoteOff(int noteNumber)
    {
        //FIRST! - the key is no longer pressed so set it to false duh
        isKeyPressed[noteNumber] = false;

        ////THEN return to the appropriate color
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



    //general algorithm has now changed to
    // step 01: spawn chords
    // step 02: start rolling them down until they reach the green light
    // step 03: destroy object upon hitting green light
    // step 04: highlight chords and improvs based on color
    // step 04b: clear spawn and highlight lists 
    // step 05: proceed to next spawn, repeat

    ////need a cleanup function
    //public void CleanupKeyboard()
    //{
    //    //show all 4 as a for loops
    //    for (int i = 0; i < keysCount; i++)
    //    {
    //        pianoKeys[i].GetComponent<Image>().color = Color.black;
    //    }//endfor
    //    //return lickset;
    //}//endremovelicks

    //start is for initialization 
    void Start()
    {
        //set greenline pos
        //these values are true never change them to transform.Position
        //lowerpositionlimit = green_line.transform.localPosition.y;
        spawnpoint = spawn_top.transform.localPosition.y;
        //spawnmid = spawn_mid.transform.localPosition.y;
        ctr = 0;
        //belowpink = (Color)improvpink * 0.75f;
        //lets try purple

        //start with a clean slate
        CleanupKeyboard();
        ClearMelodies();
        ClearImprovs();

        display_name.text = "rest";

        //SPAWN
        //SpawnKeys();

        //some crucial initialisation
        highlightNow = false;
        // isNext = false;

        //STEP 01
        //spawn the first in the sequence
       // display_name.text = ChordNames[ctr];
        // SpawnRoll(ChordList[ctr], yellow, 1);
        //  SpawnRoll(LickList[ctr], improvpink, 2);
        //======== if we wanna spawn blues we use
        //display_name.text = BluesChordNames[ctr];
        //SpawnRoll(EBluesScale[ctr], yellow, 1);

        //this cleans up everything at start
        for (int i = 0; i < 61; i++)
        {
            isKeyPressed[i] = false;
            //dont put anything here anymore it fucks up the configurations
        }

        //SPAWN EVERYTHING NOW
        //SpawnRoll(ChordList[ctr], yellow, 1); //chord (and also licklist if selected)

    }//end start function

    // Update is called once per frame
    void Update()
    {
        // CleanupKeyboard();

        //RollKeys();
        if (highlightNow)
        {
            // CleanupKeyboard();
            //we need to clear previous improvs so they dont stain the keyboard
            ClearMelodies();
            ClearImprovs();

            if (mode==0) {
                JazzMode();
            }
            else
            {
                BluesMode();
            }
            //BluesMode();

            //then highlight the next batch
            //==== JAZZ IMPROV Variables ====
            // HighlightLicks(ChordList[ctr], yellow, 1);
            // HighlightLicks(LickList[ctr], improvpink, 2);

            //===== Jazz improv with Halfstep
            //HighlightLicks(HalfStepList[ctr], belowpink, 2);

            //==== Jazz improv with Scale above
            //HighlightLicks(StepAboveList[ctr], belowpink, 3);

            //===== BLUES IMPROV VARIABLES ====
            //HighlightLicks(EBluesScale[ctr], yellow, 1);
            //HighlightLicks(EBluesImprov[ctr], blues, 2);

            //highlightNow 
            highlightNow = false;
        }//end if check hihglight now

        ////when its time to trigger spawn and chordlist isnt empty
        if (spawnNew)
        {
            //clear lines when we spawn new
           // clearMapLines();

            //Some things to control the spawning
            //then increment
            //=====jazz 
            if (ctr < ChordList.Count - 1)
            //if (ctr < EBluesScale.Count - 1) //==== improv
            {
                ctr++;
            }
            //revert back to 0 when over this ensures a loop 
            //else
            //{
            //    ctr = 0;
            //}

            //======jazz variables
            //spawn new based on recent counter
            display_name.text = ChordNames[ctr];
            //    SpawnRoll(ChordList[ctr], yellow, 1);
            //    SpawnRoll(LickList[ctr], improvpink, 2);

            //=======blues variables
            //spawn new based on recent counter
            //display_name.text = BluesChordNames[ctr];
            //SpawnRoll(EBluesScale[ctr], yellow, 1);

            spawnNew = false;

        }//endidSpawnNew

    }//end update function

    private void FixedUpdate()
    {
        RollKeys();
        CleanupKeyboard();
       // display_name.text = "rest";
    }

    public void ClearImprovs()
    {
        for (int i = 0; i < 61; i++)
        {
            improvToHighlight[i] = false;
        }
    }//endclearImprovs

    public void ClearMelodies()
    {
        for (int i = 0; i < 61; i++)
        {
            melodyToHighlight[i] = false;
        }
    }//endclearImprovs

    //change algo. map lines get spawned but change in active color
    //also they shrink over time
    //they key is to guide the user for the NEXT
    public void ShowMapLines(int keyNumber, Color lineColor)
    {

        //get the index number, pass the darker color
        //trigger change color, have a lose color when you hit the green line
        Color darkerColor = new Color();
        darkerColor = (Color)lineColor * 0.75f;
        pianoKeys[keyNumber].transform.GetChild(1).GetComponent<Image>().color = darkerColor;
        //lineMaptoRemove.Add(keyNumber);
        lineMapList.Remove(keyNumber);
        // lineMap[keyNumber] = true;

    }//end show map lines

    public void HideMapLines(int keyNumber)
    {
        //when you hit the green line then call this method
        pianoKeys[keyNumber].transform.GetChild(1).GetComponent<Image>().color = Color.black;

        //when you show up, the highlights must disappear
    }//end hidemap lines

    public void clearMapLines()
    {
        // lineMapList.RemoveAt(keyNumber);
        lineMapList.Clear(); //Debug.Log("Map lines clear");
    }//end clearmaplines

    public void ReceiveYPlots(List<int> List3)
    {
        foreach (var item in List3)
        {
            //Debug.Log("Offset saved " + item );
            YPlotsReceived.Add(item);
        }//end foreach

        //return YPlotsReceived;
    }//end receive y plots

    public void TimeReceiver(List<int> TimesReceived)
    {
        //we need the length to set the scale of the spawns
        foreach (var item in TimesReceived)
        {
            //Debug.Log("times saved " + item);
            KeyLengthsReceived.Add(item);
        }

        ReadyToSpawn = true;

        SpawnKeys();

    }//endTimeReceiver

    public bool GetHighlightStatus()
    {
        return highlightNow;
    }//end gethighlightstatus

    public void SetHighlightStatus(bool value)
    {
        highlightNow = value;
    }//end gethighlightstatus

    public int GetProgramCounter()
    {
        return programctr;
    }//end get programcounter

    public void SetProgramCounter(int num)
    {
        programctr = num;
    }//end get programcounter

    public void JazzMode()
    {
        if (GetHighlightStatus())
        {
            CleanupKeyboard();
            HighlightLicks(ChordList[ctr], yellow, 1);
            HighlightLicks(LickList[ctr], improvpink, 2);
            // HighlightLicks(BluesList[ctr], blues, 2);
            //Debug.Log("licks highlighted at " + TimeManager.GetComponent<TimeMgr>().GetTime().TotalSeconds);
        }//end if check get time
    }//end jazz mode

    public void BluesMode()
    {
        if (GetHighlightStatus())
        {
            CleanupKeyboard();
            HighlightLicks(ChordList[ctr], yellow, 1);
            //HighlightLicks(LickList[ctr], yellow, 2);
            HighlightLicks(BluesList[ctr], blues, 2);
            //Debug.Log("licks highlighted at " + TimeManager.GetComponent<TimeMgr>().GetTime().TotalSeconds);
        }//end if check get time
    }//end blues mode 

    public void GetMode(int modereceived)
    {
        mode = modereceived;
    }

}//endclass
