/*
 * This class manages all time-related components in the piano such as:
 * - TimeBar
 * - Tempo Control
 * - MoveBar
 * - sokme other time control features 
 * 
 * dependent of: RollScript, ChordMgr, ImprovMgr
 * thus all times must be public so they are sent in correct time
 * 
 * depedent to: no one 
 * */
using System; //for time 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //for the color 

public class TimeMgr : MonoBehaviour
{
    [SerializeField] GameObject TimeManager;
    [SerializeField] GameObject RollManager;

    //this will be the bar. we need an instantiate in SpawnBar and a prefab clone
    //GameObject TimeBar;
    public GameObject[] TimeBar = new GameObject[4]; //max 4 at a given time

    //important counters
    public int timebarctr = 0; //counts the current number of timebars spawned
    public int movebarctr = 0; //iterator counter so it doesnt affect current spawns
    public int secondctr = 0; //for knowing when to spawn 

    //for the lower position limit
    public GameObject green_line;

    //for the spawn point
    public GameObject spawn_top; //game object
    public float spawnpoint; //y coord of the spawnpoint
    public GameObject destroy_point; //for the lerp destination

    Color32 greyColor;


    //========== TIME RELATED VARIABLES ==============/
    //thread timer related variables
    private float startWatch;
    public float keyStartTime;
    public TimeSpan songTime;
    public float currentTime;
    public int startMinutes;
    public TimeSpan start;
    public TimeSpan time;
    public bool Started = false;

    float barSpeed = (float)0.682; //from 0.05 0.65 was ok //0.15 is still too fast
    //0.6666666667;

    // Start is called before the first frame update
    void Start()
    {
        // have some time-related declarations here
        //time related inits
        currentTime = 0.000f; //current time is stored in seconds, start at 00
        keyStartTime = currentTime;
        start = TimeSpan.Zero;
        startWatch = Time.time;

        //and here is where we move time
        time = TimeSpan.FromSeconds(currentTime);

        //color related init
        greyColor = new Color32(128, 128, 128, 255);

        //set the spawn point
        spawnpoint = spawn_top.transform.localPosition.y;

        //create the bar
        SpawnBar();
    }

    // we use fixed update on this so it is constant
    void FixedUpdate()
    {
        //this where the time should update (on a fixed rate)
        time = TimeSpan.FromSeconds(currentTime);

        ////move the bars
        //foreach(GameObject i in TimeBar){
        //    MoveBar(i);
        //}
        MoveBar(TimeBar[0]);

        if (TimeBar[1] != null)
        {
            MoveBar(TimeBar[1]);
        }
       
        //checktime here

        checkTime();

        //check spawnCounter
        checkSpawnCount();

        //and this moves the timer so never touch this
        currentTime += Time.deltaTime;
    }

    //I think its best to just spawn the bar once and move them and teleport them
    public void SpawnBar()
    {
        //set prefab to load -- update with actual prefab
        GameObject BarPrefab;
        BarPrefab = (GameObject)Resources.Load("Prefab/barprefab");

        //instantiate first
        TimeBar[timebarctr] = Instantiate(BarPrefab);

        //set parent as timemgr
        TimeBar[timebarctr].transform.SetParent(TimeManager.transform, true);

        //if we changing its scale then this is where do we it
        // change scale here
        TimeBar[timebarctr].transform.localScale = new Vector3(green_line.transform.localScale.x, 0.5f, 1);

        //then change the color herei
        //(0.5, 0.5, 0.5, 1)
        TimeBar[timebarctr].GetComponent<Image>().color = greyColor;

        //get the vector variable for position of the green line 
        Vector3 keypos = green_line.transform.localPosition;

        //then assign in initial position prior movement
        TimeBar[timebarctr].transform.localPosition = new Vector3(keypos.x, spawnpoint, keypos.z);
        //removed +50 from spawnpoint

        //increment spawnbar
        timebarctr++;

    }//endSpawnBar

    //this should have its own counter  
    public void MoveBar(GameObject TimeBar)
    {
        ///general algorithm
        // spawn them from SpawnBar()
        //move them down based on time (perhaps use LERP for this)
        // when it reaches target, teleport back
        //repeat

        float keyStartTime = currentTime;

        // Vector3 pos = TimeBar[movebarctr].transform.position;
        Vector3 backpos = spawn_top.transform.position;

        Vector3 pos = TimeBar.transform.position;
        //Vector3 backpos = spawn_top.transform.position;

        //===== this is the transform position approach ========/
        //this is the rate of movement
        pos.y -= barSpeed;

        //this is the movement action
        TimeBar.transform.position = pos;

        //go invisible when it reaches the lines
        if ((TimeBar.GetComponent<RectTransform>().localPosition.y) <= green_line.GetComponent<RectTransform>().localPosition.y)
        {
            //consider reducing alpha along the way
            Color reducedAlpha = TimeBar.GetComponent<Image>().color;
            reducedAlpha.a = 0;
            TimeBar.GetComponent<Image>().color = reducedAlpha;

        }//end if

        //if it reaches the "destroy point" then teleport back to top
        if ((TimeBar.GetComponent<RectTransform>().localPosition.y) <= destroy_point.GetComponent<RectTransform>().localPosition.y)
        {
            Debug.Log("time reached " + time.ToString(@"mm\:ss\:fff"));

            //revert alpha then teleport back
            Color reducedAlpha = TimeBar.GetComponent<Image>().color;
            reducedAlpha.a = 255;
            TimeBar.GetComponent<Image>().color = reducedAlpha;
            //then teleport back to back position
            TimeBar.transform.position = backpos;
        }//end if

        //====== end of transform position approach =====
    }//end MoveBar

    public void checkTime()
    {
        //if it is divisible by 4 then spawn
        if ((time.TotalSeconds)==2)
        {
            //Debug.Log("seconds is" + time.TotalSeconds);
            SpawnBar();
        }

    }//enmd check time

    //ensures that oiur timebars are upto 4 only and dont overload the array
    public void checkSpawnCount()
    {
        if (timebarctr == 4)
        {
            //reset
            timebarctr = 0;
        }
    }//endcheckTime


}//end TimeMgr
