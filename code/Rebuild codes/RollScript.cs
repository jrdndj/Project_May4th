//some portions of this code was inspired from https://github.com/KateSawada/midi_visualizer_tutorial_01/blob/vol01/Assets/Scripts/BarScript.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //for ToList
using UnityEngine.UI; //added for colors

public class RollScript : MonoBehaviour
{
    //============ ENVIRONMENT RELATED VARIABLES =============/

    [SerializeField] GameObject ChordManager;

    //this helps the mapping of keys similar to that midi hardware
    [SerializeField] List<GameObject> pianoKeys = new List<GameObject>();

    //an important element to manage all children of spawns
    [SerializeField] GameObject rollManager;

    //viz modes 1 to 4 should be in this script 
    public Toggle rollmodeListener, onpressmodelistener, guidedlistener, onwaitlistener;
    //jazzlistener, blueslistener, HSlistener, SAlistener, Whitelistener, RedListener;
    //public Toggle chordnamelistener, endresolvelistener, maplinelistener, keynamelistener;

    //this is to manage spawns
    public GameObject[] spawnedBars = new GameObject[keysCount]; //this w as original
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

    public bool shiftedTypes = false;

    public bool improvdiscovered = false;

    //for the chord name to display
    //public GameObject chord_name; 
    [SerializeField] public Text display_name;

    //help us check for errors and control the number of spawns
    const int keysCount = 61; //or is it 68?
    public float lowerpositionlimit; //changed to float                              
    public int ctr;  //an internal counter
    public int programctr = 0;
    public float spawnpoint; //y coord of the spawnpoint
    public float spawnmid; //y coord for the spawndmid
    public bool[] isKeyPressed = new bool[keysCount]; //for spawning
    bool[] isKeyHighLighted = new bool[keysCount]; //for error checking
    bool spawnNew = true; //flag to trigger next spawn or not
    public bool highlightNow = false; //flag to trigger higlighting
    //bool turnOfflights = false; 
    bool decreasing = false; //for the bars spawning
    bool destroyed = false;
    //some crucial variables
    public int spawnCount = 0;
    public int genre = 0;
    public int VizMode = 9; //1 is roll, 2 if expert press, 3 if guided, 4 if on-wait
    public bool showLickCount = false;
    public bool enablehalfstep = false;
    public bool enablestepabove = false;
    public bool movementEnabled = true; //set default to true
    public bool isReleased = false; // if true it means it can keep moving
    public bool isPressed = false;
    private bool isTouched = false;
    public bool isPaused = true;

    //list needed for the guided press mode
    public List<int> guidedPressList = new List<int>();


    //height of 120 means it runs for 2 seconds.
    //height of 60 means it runs for 1 second

    //to seperate melody and improv pressing
    bool[] melodyToHighlight = new bool[keysCount];
    bool[] improvToHighlight = new bool[keysCount];

    public int melodyKeyreleased = 0;

    List<int> lineMapList = new List<int>();

    public List<int> YPlotsReceived = new List<int>();
    public List<int> KeyLengthsReceived = new List<int>();
    public List<int> OnWaitKeyLengths = new List<int>();
    public List<int> OnWaitYPlots = new List<int>();

    float barSpeed = (float)0.682; //0.682

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

    //blues sequence numbering
    public static List<int> bluessequence = new List<int>() { 1, 3, 2, 1, 1, 3, 2, 3, 2, 1, 1, 3, 2 };
    //public enum bluessequence {3, 2, 1, 1, 3, 2, 3, 2, 1, 1, 3, 2, };

    ////I need you but I will need to refactor you
    List<string> ChordNames = new List<string>();
    public List<List<int>> ChordList = new List<List<int>>();
    public List<List<int>> LickList = new List<List<int>>();
    public List<List<int>> HalfStepList = new List<List<int>>();
    public List<List<int>> StepAboveList = new List<List<int>>();
    public List<List<int>> BluesList = new List<List<int>>();
    //public List<int> YPlots = new List<int>();

    //this remembers what the user pressed0
    public List<int> UserPress = new List<int>();
    public List<int> UserReleased = new List<int>();
    public bool validpress = false;
    public List<int> OnPressLicks = new List<int>();
    public List<int> OnPressBlues = new List<int>();

    //a function that receives from ImprovMgr
    public void ListReceiver(List<List<int>> List1, List<List<int>> List2, List<List<int>> List3, List<List<int>> List4, List<List<int>> List5)
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

        foreach (var item in List4)
        {
            //Debug.Log("Passing " + item);
            HalfStepList.Add(item);
        }//endlicklist

        foreach (var item in List5)
        {
            //Debug.Log("Passing " + item);
            StepAboveList.Add(item);
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

        //regular sequence if proper roll
        //i can recycle spawnRoll and iterate thru it
        if (VizMode == 1)
        {
            for (int ctr = 0; ctr < ChordList.Count; ctr++)
            {
                SpawnRoll(ChordList[ctr], yellow, 1, KeyLengthsReceived[ctr], YPlotsReceived[ctr]);
            }

        }
        //start at 1th to the nth if vizmode 4
        if (VizMode == 4)
        {
            //set onwait keylengths
            SetOnWaitKeyLenghts();

            for (int ctr = 1; ctr < ChordList.Count; ctr++)
            {
                SpawnRoll(ChordList[ctr], yellow, 1, OnWaitKeyLengths[ctr], OnWaitYPlots[ctr]);
                SpawnRoll(LickList[ctr], improvpink, 2, OnWaitKeyLengths[ctr], OnWaitYPlots[ctr]);

            }
        }

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
        if (ChordNames.Count != 0)
        {
            display_name.text = ChordNames[ctr];
        }

        //roll the objects spawns downward
        for (int i = 0; i < spawnedBars.Length; i++) //based on the current #
        {

            //if there are bars spawned keep rolling )
            if (spawnedBars[i] != null)
            {
                Vector3 pos = spawnedBars[i].transform.position;
                RectTransform SpawnScale = spawnedBars[i].GetComponent<RectTransform>();

                //changed to -= since we need them to go down

                // pos.y -= barSpeed;

                //it affects the speed of all bars so this shouldnt be the case 
                //if (decreasing)
                //{
                //    //dont move them then shrink them until they disappear
                //    pos.y -= 0;
                //    //ShrinkBars(spawnedBars[i]);
                //}

                pos.y -= barSpeed;

                //    else
                //   {
                //ideally they should stop at the bar when they reach the green line
                //   pos.y -= 0.43f; //0.43f         
                //  }

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

                //========= CHECK IF IT TOUCHES GREEN LINE =======/
                //when they reach the green line                //there was a -30 here 
                //if ((spawnedBars[i].GetComponent<RectTransform>().localPosition.y - 60) <= green_line.GetComponent<RectTransform>().localPosition.y)
                //if ((spawnedBars[i].GetComponent<RectTransform>().localPosition.y - (SpawnScale.rect.height + (SpawnScale.rect.height / 2))) <= green_line.GetComponent<RectTransform>().localPosition.y)
                if ((spawnedBars[i].GetComponent<RectTransform>().localPosition.y - (SpawnScale.rect.height + (SpawnScale.rect.height))) <= green_line.GetComponent<RectTransform>().localPosition.y)
                {
                    //Debug.Log("Bong");

                    //indicate touch first 
                    isTouched = true;

                    //// movementEnabled = false;
                    //if (!isReleased)
                    //{
                    //    movementEnabled = false;
                    //}
                    //else movementEnabled = true;

                    highlightNow = true;

                    //==== this never work so never do this again
                    //spawnedBars[i].transform.localScale -= new Vector3(0, barSpeed, 0);

                    //dont move me anymore
                    //decreasing = true;

                    //ShrinkBars(spawnedBars[i]);

                    //===== we are not shrinking anymore so we dont need this 
                    //if (SpawnScale.rect.height <= 0)
                    //{
                    //    CleanupKeyboard();
                    //    highlightNow = false;
                    //    destroyed = true;

                    //    //spawnNew = true;
                    //    // ctr++;

                    //    //remove all of these when it fails
                    //    //  Destroy(spawnedBars[i]);
                    //    // spawnNew = true;
                    //    //   decreasing = false;
                    //    // spawnCount--;
                    //}
                }//endif

                //============= CHECK IF IT REACHES DESTROY POSITION =====/
                ////since we are 2D, we use RectTransform and get the localPosition since we are in real-time               // /2 here
                if ((spawnedBars[i].GetComponent<RectTransform>().localPosition.y + (SpawnScale.rect.height + (SpawnScale.rect.height / 2))) <= destroy_point.GetComponent<RectTransform>().localPosition.y - 20)
                {
                    //destroy then highlight 
                    Destroy(spawnedBars[i]);


                    //enable movement of the rest until it touches 
                    // movementEnabled = true;

                    //set to false everything else 
                    isTouched = false;
                    isReleased = false;
                    isPressed = false;

                    //spawnedBars[i].SetActive(false);
                    highlightNow = false;


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
        //do we even need this?? Yes we need it now
        isKeyPressed[noteNumber] = true;

        //that's it - red if not highlighted 
        if (VizMode == 1)
        //if (VizMode == 1 || VizMode == 3) //this condition should be if error check is on
        {
            if (isKeyHighLighted[noteNumber] && (melodyToHighlight[noteNumber] || improvToHighlight[noteNumber]))
            {
                //if correct toggle is on show this
                pianoKeys[noteNumber].GetComponent<Image>().color = Color.white;
            }//endif
            else
            {
                pianoKeys[noteNumber].GetComponent<Image>().color = Color.red;
            }//endif

            ////=== some checking mechanism here
            ////clear validpress to be sure
            //UserPress.Add(noteNumber);
            //if (UserPress.Count >= 3) //if there are at least three presses then go
            //{
            //    ChordManager.GetComponent<ChordMgr>().PressMapper(UserPress, guidedPressList);
            //    //then immediately clear user press
            //    UserPress.Clear();
            //    // highlightNow = true; 

            //}//end if userpress
            //else validpress = false;
            //UserPress.Clear();
        }//endviz mode 1

        else if (VizMode == 2) //vizmode is expert press 
        {
            //if validation mode is off then this one
            if (!improvdiscovered)
            {
                pianoKeys[noteNumber].GetComponent<Image>().color = Color.white;
            }

            //if validation mode is on this one
            else if (improvdiscovered && isKeyHighLighted[noteNumber] && improvToHighlight[noteNumber])
            {
                //  pianoKeys[noteNumber].GetComponent<Image>().color = Color.white;
            }
            else if (improvdiscovered && !isKeyHighLighted[noteNumber] || !improvToHighlight[noteNumber])
            {
                //  pianoKeys[noteNumber].GetComponent<Image>().color = Color.red;
            }
            // else pianoKeys[noteNumber].GetComponent<Image>().color = Color.red;

            //clear validpress to be sure
            UserPress.Add(noteNumber);
            if (UserPress.Count == 4) //if there are at least three presses then go
            {
                ChordManager.GetComponent<ChordMgr>().PressMapper(UserPress, guidedPressList);
                //then immediately clear user press
                UserPress.Clear();
                // highlightNow = true; 

            }//end if userpress
            else validpress = false;
            //UserPress.Clear();

        }//end viz mode 2
        else if (VizMode == 4) //onwait 
        {
            if (isKeyHighLighted[noteNumber] && melodyToHighlight[noteNumber] && isKeyPressed[noteNumber])
            {
                //if correct toggle is on show this
                //pianoKeys[noteNumber].GetComponent<Image>().color = Color.white;

                //add to list of presses to check
                //  UserPress.Add(noteNumber);

            }//endif
            else
            {
                pianoKeys[noteNumber].GetComponent<Image>().color = Color.red;
            }//endif

         
        }//end vizmode 4
   
    }//endonNoteOn;

    //when user releases a pressed key as per MIDIScript 
    public void onNoteOff(int noteNumber)
    {

        ////THEN return to the appropriate color
        ///this is true for any mode 
        if (improvToHighlight[noteNumber] == true)
        {
            pianoKeys[noteNumber].GetComponent<Image>().color = improvpink;
            //==========for blues
            //pianoKeys[noteNumber].GetComponent<Image>().color = blues;
        }
        //else if (melodyToHighlight[noteNumber] == true)
        //{
        //    pianoKeys[noteNumber].GetComponent<Image>().color = Color.yellow;
        //}0
        else
        {
            pianoKeys[noteNumber].GetComponent<Image>().color = Color.black;
        }

        if (VizMode == 2)
        {
            //clear user press too
            UserPress.Clear();
            display_name.text = "Press any chord to continue";
            //remove any content and be ready to be populated again
            if (validpress)
            {
                //OnPressLicks.Clear();
                ClearImprovs();
                // improvdiscovered = false;
                CleanupKeyboard();
            }//ednvalid press
             // CleanupKeyboard();
        }//endvismode2

        if (VizMode == 3)
        {
            //check if key pressed is same in the melody to press
            if (isKeyPressed[noteNumber] == melodyToHighlight[noteNumber])
            {
                //if they are the same then release it
                melodyToHighlight[noteNumber] = false;
                //melodyKeyreleased++;
                // display_name.text = "Release the next chord to release";
            }//end check key compared and melody to release

            ////if all melody is released then move to next highlight
            //if (melodyKeyreleased == 4)
            //{
            //    //then we move to the next by triggering spawnNew
            //    spawnNew = true;

            //    //cleanupKeyboard
            //    CleanupKeyboard();
            //    ClearImprovs();

            //    //then refresh melodyKeyreleased
            //    melodyKeyreleased = 0;
            //    isReleased = true;
            //}//end check melody key count 


        }//end vizmode 3
        //still keep moving on release
        if (VizMode == 4) //onwait 
        {

            //add every press to UserRelease
            UserReleased.Add(noteNumber);

            //check if we have four
            if (UserReleased.Count >= 3)
            {
                //  Debug.Log("Here");
                //then check chordmapper
                //  ChordManager.GetComponent<ChordMgr>().CheckifCorrectReleased(UserReleased, guidedPressList);
                isReleased = true;
                //clear when don
                                //  UserReleased.Clear();

            }
        }//end viz mode

        //PUTTING THIS TO LAST SO WE DONT FORGET - the key is no longer pressed so set it to false duh
        isKeyPressed[noteNumber] = false;

    }//end OnNoteOff

    //lights up a group of keys based on the licks 
    public void HighlightLicks(List<int> lickset, Color32 highlightcolor, int spawntype)
    {
        int numHighlighted = -2;
        int bluesorder = 0;
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
                numHighlighted++;
                if (showLickCount && genre == 0 && !enablestepabove && !enablehalfstep) // works only for jazz
                {
                    int locallickcount = numHighlighted;

                    locallickcount += 2;
                    pianoKeys[lickset[i]].GetComponentInChildren<Text>().text = locallickcount.ToString();
                    numHighlighted++;
                }//end show lick count
                //if blues we use semitones count
                if (showLickCount && genre == 1 && !enablestepabove && !enablehalfstep) // blues
                {
                    //enum is 3 2 1 1 3 2 3 2 1 1 3 2
                    //start with 0
                    // int locallickcount = 0;
                    //getBluesSequence(locallickcount);

                    pianoKeys[lickset[i]].GetComponentInChildren<Text>().text = getBluesSequence(bluesorder).ToString();
                    bluesorder++;
                    //numHighlighted++;
                }//end show lick count


            }//if improv lick
            else //1 if melody
            {
                melodyToHighlight[lickset[i]] = true;
                //show names of chord progression
                pianoKeys[lickset[i]].GetComponentInChildren<Text>().text = ChordNames[ctr];
                //also show the name

            }//if melody higlight

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
            pianoKeys[i].GetComponentInChildren<Text>().text = "";
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
        movementEnabled = true;
        Debug.Log("initial viz mode is " + VizMode);
        //set greenline pos
        //these values are true never change them to transform.Position
        //lowerpositionlimit = green_line.transform.localPosition.y;
        spawnpoint = spawn_top.transform.localPosition.y;
        //spawnmid = spawn_mid.transform.localPosition.y;
        ctr = 0;
        //belowpink = (Color)improvpink * 0.75f;
        //lets try purple


        //rollmode toggle listener viz mode 1 
        rollmodeListener.GetComponent<Toggle>();
        rollmodeListener.onValueChanged.AddListener(delegate
        {
            RollModeValueChanged(rollmodeListener);
        });

        //on press mode toggle listener viz mode 2 
        onpressmodelistener.GetComponent<Toggle>();
        onpressmodelistener.onValueChanged.AddListener(delegate
        {
            OnPressValueChanged(onpressmodelistener);
        });

        //guided press mode toggle listener viz mode 3
        guidedlistener.GetComponent<Toggle>();
        guidedlistener.onValueChanged.AddListener(delegate
        {
            GuidedValueChanged(guidedlistener);
        });

        //for on wait viz mode 3
        onwaitlistener.GetComponent<Toggle>();
        onwaitlistener.onValueChanged.AddListener(delegate
        {
            OnWaitValueChanged(onwaitlistener);
        });

        ////jazz improv listener
        //jazzlistener.GetComponent<Toggle>();
        //jazzlistener.onValueChanged.AddListener(delegate
        //{
        //    JazzValueChanged(jazzlistener);
        //});

        ////blues improv listener
        //blueslistener.GetComponent<Toggle>();
        //blueslistener.onValueChanged.AddListener(delegate
        //{
        //    BluesValueChanged(blueslistener);
        //});

        ////bkeyname lick count listener
        //keynamelistener.GetComponent<Toggle>();
        //keynamelistener.onValueChanged.AddListener(delegate
        //{
        //    KeyNameValueChanged(keynamelistener);
        //});

        ////halfstep listener
        //HSlistener.GetComponent<Toggle>();
        //HSlistener.onValueChanged.AddListener(delegate
        //{
        //    HSValueChanged(HSlistener);
        //});

        ////halfstep listener
        //SAlistener.GetComponent<Toggle>();
        //SAlistener.onValueChanged.AddListener(delegate
        //{
        //    SAValuechanged(SAlistener);
        //});

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

    // for the higlighting of the licks
    void Update()
    {
        // CleanupKeyboard();
        //if (VizMode == 4)
        //{

        //    OnWaitRollKeys();
        //    CleanupKeyboard();


        //}//endvizmode4

        //exclusive to vizmode 4 now, merge later 
        if (VizMode == 4)
        {

            OnWaitRollKeys();
            CleanupKeyboard();

            //  Debug.Log("chord list has " + ChordList.Count);

            //add some conditions here when we are on the last thing
            if (ctr == ChordList.Count - 1)
            {

                //await a spacebar press here then
                //triggering the spacebar would restart everything
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ctr = 0; //set zero
                             //set spawnNew
                             //clear spawns
                    spawnCount = 0;

                    //then spawnew
                    SpawnKeys();

                    //then false
                    spawnNew = false;

                    //disable movement
                    isPaused = true;
                    movementEnabled = false;

                }//end check spacebar
            }//end check ctr say if its a 3

            if (highlightNow)
            {
                // CleanupKeyboard();
                //we need to clear previous improvs so they dont stain the keyboard
                ClearMelodies();
                ClearImprovs();

                if (genre == 0)
                {
                    JazzMode();
                }
                else
                {
                    BluesMode();
                }

            }//end if check hihglight now

            //trigger spawn when a spacebar is pressed and there are no spawns
            if (spawnNew)
            {

                if (ctr <= ChordList.Count) //removed -1 here 
                {
                    ctr++;
                }
                //revert back to 0 when over this ensures a loop 
                else
                {
                    //still 0 
                    ctr = 0;
                    //then spawnkeys now for viz 4
                    SpawnKeys();
                }

                if (ChordNames.Count != 0)
                {
                    display_name.text = ChordNames[ctr];
                }
                //crucial stuff
                spawnNew = false;

            }//endidSpawnNew
        }//endvizmode 4


        //this will now be for viz mode 1 and 3 only. duplicate revise for 4
        if (VizMode == 1 || VizMode == 3)
        {
            //spawn
            // SpawnKeys();

            //RollKeys();
            if (highlightNow)
            {
                // CleanupKeyboard();
                //we need to clear previous improvs so they dont stain the keyboard
                ClearMelodies();
                ClearImprovs();

                if (genre == 0)
                {
                    JazzMode();
                }
                else
                {
                    BluesMode();
                }

            }//end if check hihglight now

            ////when its time to trigger spawn and chordlist isnt empty
            if (spawnNew)
            {
                if (ctr < ChordList.Count - 1)
                {
                    ctr++;
                }
                //revert back to 0 when over this ensures a loop 
                else
                {
                    ctr = 0;
                    if (VizMode == 1) //removed || VizMode == 4, revert if fcked up
                    {
                        SpawnKeys();
                    }

                }
                if (ChordNames.Count != 0)
                {
                    display_name.text = ChordNames[ctr];
                }
                //crucial stuff
                spawnNew = false;
            }//endidSpawnNew
        }//endvizmode 1 and 3

        if (VizMode == 2 && validpress)
        {
            CleanupKeyboard();
            ClearMelodies();
            ClearImprovs();
            //change the pressed ones to white 
            //recolorHighlights();
            HighlightLicks(UserPress, Color.white, 1);
            HighlightLicks(OnPressLicks, improvpink, 2);
            //improvdiscovered = true; 
            if (UserPress.Count >= 4)
            {
                UserPress.Clear();
                // recolorHighlights();
            }//enduserpress count

        }//ifvizmode2

        if (VizMode == 3)
        {
            highlightNow = true;

            //triggered by space now
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // highlightNow = true;
                spawnNew = true; 
            }

          }//end viz mode 3

        if (VizMode == 4)
        {
            highlightNow = true;

            if (spawnCount == 0)
            {
                spawnNew = true;
                // SpawnKeys();
            }

        }//end vizmode 4

        //CleanupKeyboard();
    }//end update function

    //for the continuous rolling of the keys 
    private void FixedUpdate()
    {
        switch (VizMode)
        {
            case 1:
                {
                    RollKeys();
                    CleanupKeyboard();
                    break;
                }//end case 1

            default: break;
        }
    }//endfixupdate

    //==== some housekeeping functions 
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

    public void ReceiveOnWaitYPlots(List<int> List3)
    {
        foreach (var item in List3)
        {
            //Debug.Log("Offset saved " + item );
            OnWaitYPlots.Add(item);
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

        //ReadyToSpawn = true;

        //we receive it but we wait for the signal by the user
        //SpawnKeys();

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

            if (enablehalfstep)
            {
                HighlightLicks(HalfStepList[ctr], belowpink, 2);
            }
            if (enablestepabove)
            {
                HighlightLicks(StepAboveList[ctr], belowpink, 2);
            }

        }//end if check get time
    }//end jazz mode

    public void BluesMode()
    {
        if (GetHighlightStatus())
        {
            CleanupKeyboard();
            HighlightLicks(ChordList[ctr], yellow, 1);
            HighlightLicks(BluesList[ctr], blues, 2);
            //Debug.Log("licks highlighted at " + TimeManager.GetComponent<TimeMgr>().GetTime().TotalSeconds);
        }//end if check get time
    }//end blues mode 

    public void GetMode(int modereceived)
    {
        genre = modereceived;
    }//endgetmode

    public void ShrinkBars(GameObject spawnedBars)
    {
        RectTransform SpawnScale = spawnedBars.GetComponent<RectTransform>();
        //==== this is using size delta approach ===== /
        //start reducing in size                                                    // * 5
        SpawnScale.sizeDelta = new Vector2(SpawnScale.sizeDelta.x, SpawnScale.sizeDelta.y - (barSpeed / 2));

        //dont move me anymore
        decreasing = true;

    }//endshrinkbars

    public void ResizeBars(GameObject spawnedBars)
    {
        RectTransform SpawnScale = spawnedBars.GetComponent<RectTransform>();

        if (!decreasing)
        {
            SpawnScale.sizeDelta = new Vector2(SpawnScale.sizeDelta.x, SpawnScale.sizeDelta.y / 2);
            //dont move me anymore
            decreasing = true;
        }
    }//endshrinkbars

    public void DestroySpawns()
    {
        //for(int i = 0; i< spawnedBars.Length; i++)
        foreach (GameObject item in spawnedBars)
        {
            Destroy(item);
            //   item.SetActive(false);
            //spawnedBars[i].SetActive(false);
        }
    }//end destroy spawns

    public void ReactiveSpawns()
    {
        for (int i = 0; i < spawnedBars.Length; i++)
        //   foreach (GameObject item in spawnedBars)
        {
            // item.SetActive(true);
            spawnedBars[i].SetActive(true);
        }
    }

    public void RollModeValueChanged(Toggle change)
    {
        //things to do when RollMode is changed
        if (change.isOn)
        {
            //set ctr to 0 to have a good start
            ctr = 0;
            spawnCount = 0;

            Debug.Log("Selected Roll Mode.");
            //start with a clean slate
            CleanupKeyboard();
            ClearMelodies();
            ClearImprovs();

            display_name.text = "rest";

            //set to one to trigger other roll update events
            VizMode = 1;


            //set new spawnpoint
            spawnpoint = spawn_top.transform.localPosition.y;

            //SPAWN
            //  if (!shiftedTypes) {
            SpawnKeys();
            // }
            //  else
            //  {
            //      ReactiveSpawns();
            //  }
            //some crucial initialisation
            highlightNow = false;
            // isNext = false;


        }//end if
        else
        {
            //change to default vizmode then destroy everything else
            VizMode = 9;
            shiftedTypes = true;
            DestroySpawns();
            Debug.Log("Deactivated rolling. Spawns destroyed. ");
            display_name.text = "select viz mode";

            ClearImprovs();
            ClearMelodies();
            CleanupKeyboard();
            //clear user press too
            UserPress.Clear();

            //set ctr to 0
            ctr = 0;
        }//endelse
    }//end rollvaluemode changed

    public void OnPressValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            VizMode = 2;
            Debug.Log("Selected On Press Mode.");
            display_name.text = "Press a Chord to show licks";
            CleanupKeyboard();
        }//endif 
        else
        {
            //change to default vizmode
            VizMode = 9;
            Debug.Log("Deactivated On press mode.");
            display_name.text = "select viz mode";

            ClearMelodies();
            ClearImprovs();
            CleanupKeyboard();

            //clear user press too
            UserPress.Clear();

            //set ctr to 0 to have a good start
            ctr = 0;
        }//endelse 

    }//end onpressvaluechanged

    public void GuidedValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            VizMode = 3;
            Debug.Log("Guided Press Mode");
            display_name.text = "press a chord";

            //restart it to 0 just to be sure
            ctr = 0;
        }
        else
        {
            //change to default vizmode
            VizMode = 9;
            Debug.Log("Deactivated Guided mode.");
            display_name.text = "select viz mode";

            ClearMelodies();
            ClearImprovs();
            CleanupKeyboard();

            //clear user press too
            UserPress.Clear();

            //set counter to 0 for roll purpose
            ctr = 0;
        }
    }//end onpressvaluechanged

    public void OnWaitValueChanged(Toggle change)
    {
        //its like RollMode but wait for press before Rollings
        if (change.isOn)
        {
            //set ctr to 0 to have a good start
            ctr = 0;
            spawnCount = 0;

            Debug.Log("OnWait Press Mode.");
            //start with a clean slate
            CleanupKeyboard();
            ClearMelodies();
            ClearImprovs();

            display_name.text = "OnWait Press Mode";

            //set new spawnpoint
            // spawnpoint = green_line.GetComponent<RectTransform>().localPosition.y;
            spawnpoint = destroy_point.GetComponent<RectTransform>().localPosition.y - 60;

            //4 is onwait 
            VizMode = 4;

            //disable movement by default until event changes is
            movementEnabled = false;

            //SPAWN
            //  if (!shiftedTypes) {
            SpawnKeys();
            // }
            //  else
            //  {
            //      ReactiveSpawns();
            //  }
            //some crucial initialisation
            highlightNow = false;
            // isNext = false;


        }//end if
        else
        {
            //change to default vizmode then destroy everything else
            VizMode = 9;
            shiftedTypes = true;
            DestroySpawns();
            //Debug.Log("Deactivated rolling. Spawns destroyed. ");
            display_name.text = "select viz mode";
            ClearImprovs();
            ClearMelodies();
            CleanupKeyboard();
            //clear user press too
            UserPress.Clear();

            //set ctr to 0
            ctr = 0;
        }//endelse
    }//end onpressvaluechanged

    //public void JazzValueChanged(Toggle change)
    //{
    //    // ctr = 0; 
    //    if (change.isOn)
    //    {
    //        genre = 0;
    //        Debug.Log("Jazz Improvs will be shown");

    //    }
    //    else
    //    {
    //        //change to default vizmode
    //        genre = 1;
    //        Debug.Log("Blues Improvs will be shown");
    //        //
    //        //ChordManager.GetComponent<ChordMgr>().ChordMapper(Blues001);
    //    }
    //}//end blues values toggle

    //public void BluesValueChanged(Toggle change)
    //{
    //    //ctr = 0; 
    //    if (change.isOn)
    //    {
    //        genre = 1;
    //        Debug.Log("Blues Improvs will be shown");


    //    }
    //    else
    //    {

    //        genre = 0;
    //        Debug.Log("Jazz Improvs will be shown");

    //    }
    //}//end bluesvalues toggle

    //public void KeyNameValueChanged(Toggle change)
    //{
    //    if (change.isOn)
    //    {
    //        showLickCount = true;
    //    }//end
    //    else
    //    {
    //        showLickCount = false;
    //    }
    //}//end keynamevaluechanged

    public int getBluesSequence(int order)
    {
        return bluessequence[order];
    }//end getBlues sequence number

    //public void HSValueChanged(Toggle change)
    //{
    //    if (change.isOn)
    //    {
    //        enablehalfstep = true;
    //    }//end
    //    else
    //    {
    //        enablehalfstep = false;
    //    }
    //}//end keynamevaluechanged

    //public void SAValuechanged(Toggle change)
    //{
    //    if (change.isOn)
    //    {
    //        enablestepabove = true;
    //    }//end
    //    else
    //    {
    //        enablestepabove = false;
    //    }
    //}//end keynamevaluechanged

    //a function to recolor every key
    public void recolorHighlights()
    {
        foreach (var item in UserPress)
        {
            if (pianoKeys[item])
            {
                pianoKeys[item].GetComponent<Image>().color = Color.white;
            }
        }
    }//end recolorHighlights

    public bool monitorTouch()
    {
        if (isTouched && (isReleased || isPressed))
        //if (isTouched || isReleased)
        {
            return true;
        }

        else
        {
            return false;
        }

    }//end monitorTouch

    //this function gets the current list of guided press and adds it for error checking
    public void getGuidedPressList()
    {
        for (int i = 0; i < melodyToHighlight.Count(); i++)
        {
            if (melodyToHighlight[i])
            {
                guidedPressList.Add(i);
            }
        }
    }//end getguidedpresslist

    //meant specifically for  on wait rollkeys
    public void OnWaitRollKeys()
    {
        //show their name
        if (ChordNames.Count != 0)
        {
            display_name.text = ChordNames[ctr];
        }//displaying of chordnames 

        //this is good already
        if (isPaused || !movementEnabled)
        {
            // Debug.Log("Looking for the spacebar press now");
            //if (Input.anyKeyDown)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isPaused = false;  // Resume movement if spacebar is pressed
                isPressed = true; //and tell us its been pressed 
                                  //  Debug.Log("spacebar pressed!");
                movementEnabled = true;
            }//end if keycode space
        }//end if isPaused 

        //else keep moving
        else
        {
            //roll the objects spawns downward
            for (int i = 0; i < spawnedBars.Length; i++)
            {// Move the objects downward
                if (spawnedBars[i] != null)
                {
                    Vector3 pos = spawnedBars[i].transform.position;
                    RectTransform SpawnScale = spawnedBars[i].GetComponent<RectTransform>();
                    pos.y -= barSpeed;
                    spawnedBars[i].transform.position = pos;

                    // Check if objects have reached the green line
                    //if (((spawnedBars[i].GetComponent<RectTransform>().localPosition.y - (SpawnScale.rect.height + (SpawnScale.rect.height))) <= green_line.GetComponent<RectTransform>().localPosition.y))
                    if (spawnedBars[i].GetComponent<RectTransform>().localPosition.y < green_line.GetComponent<RectTransform>().localPosition.y + (SpawnScale.rect.height))
                    {
                        if (isPressed)
                        {
                            isPaused = false;
                        }
                        else
                        {
                            //  Debug.Log("TOUCHED! Press space to continue ");
                            isPaused = true;  // Pause objects at the green line

                        }
                        highlightNow = true;
                        //should i disable movement here? answer is no
                        //  movementEnabled = false;
                    }//end if touch green line

                    //now check if it touches the destroy point
                    //============= CHECK IF IT REACHES DESTROY POSITION =====/
                    ////since we are 2D, we use RectTransform and get the localPosition since we are in real-time               // /2 here
                    if ((spawnedBars[i].GetComponent<RectTransform>().localPosition.y + (SpawnScale.rect.height + (SpawnScale.rect.height / 2))) <= destroy_point.GetComponent<RectTransform>().localPosition.y - 20)
                    {
                        //destroy then highlight 
                        Destroy(spawnedBars[i]);

                        //enable movement of the rest until it touches 
                        movementEnabled = false;

                        //set to false everything else 
                        isTouched = false;

                        //spawnedBars[i].SetActive(false);
                        highlightNow = false;

                        //but we can spawn something new now
                        spawnNew = true;



                        //commented this 
                        //spawnCount--;

                    }//endif check contact green point

                }//end if spawnedbars != null 
            }//end else if not ispaused 
        }//end loop for all spawns
    }//end onwait roll keys

    //set onwait keylengths
    public void SetOnWaitKeyLenghts()
    {
        foreach (var item in ChordList)
        {
            OnWaitKeyLengths.Add(2);
            //OnWaitYPlots.Add(120);
        }

    }//end set wait on keylenghts


}//endclass
