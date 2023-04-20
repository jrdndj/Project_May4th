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
using System.Linq; //for ToList
using UnityEngine.UI; //added for colors

public class ImprovMgr : MonoBehaviour
{
    //======= important environment variables =======/

    [SerializeField] GameObject ImprovManager;
    [SerializeField] GameObject RollManager;

    //we need TimeManager controlling stuff
    [SerializeField] GameObject TimeManager;

    //==== time related variables 
    const int keysCount = 61; //or is it 68?
    public int ctr;  //an internal counter
    int ListStop;
    int TimeCount = 0; //this is for the different elements
    int TimeStart = 0; //for the accumulated time for time list
    int TimePassed = 0;

    //this helps the mapping of keys similar to that midi hardware
    [SerializeField] List<GameObject> pianoKeys = new List<GameObject>();
    //List<GameObject> pianoKeys = RollScript

    bool[] improvToHighlight = new bool[keysCount];
    bool[] melodyToHighlight = new bool[keysCount];
    bool[] isKeyHighLighted = new bool[keysCount]; //for error checking
    bool[] isKeyPressed = new bool[keysCount]; //for spawning
    bool highlightNow = false;

    //========== IMPROV RELATED VARIABLES ======/

    public List<List<int>> ChordList = new List<List<int>>();
    public List<List<int>> LickList = new List<List<int>>();
    public List<int> ElapsedList = new List<int>(); //for computing validity 
    List<int> TimeList = new List<int>(); // for the list of times to check for validity
    List<int> YListPlotter = new List<int>(); //this is for the Y positions of the spawns that RollMgr will need


    //=========== COLOR RELATED VARIABLES ==========/
    //these are the color related objects
    public ColorBlock theColor;
    public Button theButton;
    Color32 improvpink = new Color32(255, 150, 234, 255);
    Color32 yellow = Color.yellow;
    Color32 belowpink = new Color32(75, 0, 130, 255);  //this is indigo akshully
    Color32 blues = new Color32(65, 105, 225, 255); // this is for the blues blue


    //==== receiver functions ========/

    //a function that receives from ChordMgr
    public void ListReceiver(List<List<int>> List1, List<List<int>> List2)
    {
        //Debug.Log("We made it here ");
        //assign chordlist
        foreach (var item in List1)
        {
           // Debug.Log("Passing " + item );
            ChordList.Add(item);
        }//endchordlist

        //assign licklist
        foreach (var item in List2)
        {
            //Debug.Log("Passing " + item);
            LickList.Add(item);
        }//endlicklist 

        //return ListReceived;
    }//endListReceiver

    public void TimeReceiver(List<int> TimesReceived)
    {
        //we shouldnt transfer time but rather set the accumulated time for spawntimes
        //transfteer things to time
        //TimeList.AddRange(TimesReceived);
        //Debug.Log("TimesReceived of ImprovMgr has " + TimesReceived.Count);
        for (int i = 0; i <= TimesReceived.Count; i++)
        {
            TimeStart += i;
            TimeList.Add(TimeStart); //this is the sequence or pprogram counter
            ElapsedList.Add(i); //this is the lengths based on sequence num
           // Debug.Log("Mapped times: " + i);
        }
        //Debug.Log("ElapsedList after mapping has " + TimesReceived.Count);
        //foreach (var item in TimesReceived)
        //{
        //    TimeStart += item;
        //    TimeList.Add(TimeStart);
        //    ElapsedList.Add(item);
        //}

    }//endListReceiver

    /*
     * this function gets the chord sequences from **/
    public void PlotYCoords()
    {

    }

    public void SpawnTimes(List<int> TimesReceived)
    {

    }


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


    // Start is called before the first frame update
    void Start()
    {

        //init ctr
        ctr = 0;
        ListStop = ChordList.Count;

        //map data for sending 
        //MapSpawnYCoords();

        //routine cleanup
        CleanupKeyboard();

        // StartCoroutine(CheckValid());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //check validity 
        CheckTime();

   //i need  a way to move to the next ctr 
        
        CleanupKeyboard();
   
        if (highlightNow)
        {
            // CleanupKeyboard();
          //  HighlightLicks(ChordList[ctr], yellow, 1);
         //   HighlightLicks(LickList[ctr], improvpink, 2);

            //Debug.Log("licks highlighted at " + TimeManager.GetComponent<TimeMgr>().GetTime().TotalSeconds);
        }//end if check get time

        //it will be here - and the time is managed by TimeMgr
        ////=== general algorithm looks like this
        //if (highlightNow)
        //{
        //    // CleanupKeyboard();
        //    //we need to clear previous improvs so they dont stain the keyboard
        //    ClearMelodies();
        //    ClearImprovs();

        //    //then highlight the next batch
        //    //==== JAZZ IMPROV Variables ====
        //    // HighlightLicks(ChordList[ctr], yellow, 1);
        //    // HighlightLicks(LickList[ctr], improvpink, 2);

        //    //===== Jazz improv with Halfstep
        //    //HighlightLicks(HalfStepList[ctr], belowpink, 2);

        //    //==== Jazz improv with Scale above
        //    //HighlightLicks(StepAboveList[ctr], belowpink, 3);

        //    //===== BLUES IMPROV VARIABLES ====
        //    //HighlightLicks(EBluesScale[ctr], yellow, 1);
        //    //HighlightLicks(EBluesImprov[ctr], blues, 2);

        //    //highlightNow 
        //    highlightNow = false;
        //}//end if check hihglight now

    }

    //some function to pass them or return in a common object
    public List<int> PassPianoCoordinates()
    {
        List<int> KeyIndices = new List<int>();

        //store each index in a keyIndex list and be ready to pass 
        foreach (var i in pianoKeys)
        {
            KeyIndices.Add(pianoKeys.IndexOf(i));
        }
        return KeyIndices;
    }

    public void CheckTime()
    {
        var localtime = TimeManager.GetComponent<TimeMgr>().GetTime().TotalSeconds;
        //Debug.Log("ctr is " + ctr);
        //Debug.Log("timestart is " + TimeStart);
        if (localtime == TimeList[ctr])
        {
            highlightNow = true;
            TimeStart = (int) TimeManager.GetComponent<TimeMgr>().GetTime().TotalSeconds;
            //save the time
            //get the time
            // TimeStart = (int) TimeManager.GetComponent<TimeMgr>().GetTime().TotalSeconds;
            Debug.Log("IN " );
        }
        //return highlightNow;
    }//end checkTime;

    //this checks how much time has passed since then so we can unhighlight 
    public bool CheckValid()
    {
        var localtime = TimeManager.GetComponent<TimeMgr>().GetTime().TotalSeconds;
        //var timeToWait = TimeList[ctr];
        //Debug.Log("waiting for " + timeToWait);
        //yield return new WaitForSeconds(timeToWait);

        //highlightNow = false;
        //ctr++;


        //check if the difference to the time now is the same as in the elapsedlist 
        if ((localtime - TimeStart) < ElapsedList[ctr])
        {
            //if their difference is same in the list then it is expired thus not valid
            highlightNow = true;
            //then move to the next element in the counter
            // ctr++;
            // highlightNow = true;
            Debug.Log("OUT ");
        }//end check valid
        else
        {
            highlightNow = false;
        }

       return highlightNow; 

    }//end check valid

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
        for (int i = 1; i <= ElapsedList.Count; i++) //yes begin at 1
        {
            YListPlotter.Add(ElapsedList[i-1]*offset); //should be of the previous one
        }//end for loop yplotter

        //then send everything to RollMgr
        //RollManager.GetComponent<RollScript>().ChordList = RollManager.GetComponent<RollScript>().ReceiveLists(ChordList);
        //RollManager.GetComponent<RollScript>().ListReceiver(ChordList, LickList);
        RollManager.GetComponent<RollScript>().ReceiveYPlots(YListPlotter);

        // RollManager.GetComponent<RollScript>().LickList = RollManager.GetComponent<RollScript>().ReceiveLists(LickList);
        //RollManager.GetComponent<RollScript>().YPlotsReceived = RollManager.GetComponent<RollScript>().ReceiveYPlots(YListPlotter); 
        //RollManager.GetComponent<RollScript>().ReceiveYPlots(YListPlotter);

    }//end MapSpawnYCoords
}//endclass 
