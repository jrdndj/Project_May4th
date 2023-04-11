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
    GameObject TimeBar;

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

    float barSpeed = (float)0.6666666667; //from 0.05 0.65 was ok //0.15 is still too fast

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

        //putting this here just for the monitoring. 
        //Debug.Log("Currenttime is " + time.ToString(@"mm\:ss"));
        //Debug.Log("next chordtime is " + ChordTimings[ctr]);

        //move the bar
        MoveBar();

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
        TimeBar = Instantiate(BarPrefab);

        //set parent as timemgr
        TimeBar.transform.SetParent(TimeManager.transform, true);

        //if we changing its scale then this is where do we it
        // change scale here
        TimeBar.transform.localScale = new Vector3(green_line.transform.localScale.x, 1, 1);

        //then change the color here
        //(0.5, 0.5, 0.5, 1)
        TimeBar.GetComponent<Image>().color = greyColor;

        //get the vector variable for position of the green line 
        Vector3 keypos = green_line.transform.localPosition;

        //then assign in initial position prior movement
        TimeBar.transform.localPosition = new Vector3(keypos.x, spawnpoint, keypos.z);
        //removed +50 from spawnpoint 

    }//endSpawnBar

    //this moves or rolls the bar every 3rd or 4th 
    public void MoveBar()
    {
        ///general algorithm
        // spawn them from SpawnBar()
        //move them down based on time (perhaps use LERP for this)
        // when it reaches target, teleport back
        //repeat

        float keyStartTime = currentTime;

        Vector3 pos = TimeBar.transform.position;
        Vector3 backpos = spawn_top.transform.position;

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

    /*
     * this is a sample LERP from ParseMIDI for reference
     * 
        //this is the target position vector of the LERP
        Vector3 Ypos = gameObject.transform.GetChild(68).position;
        var YCordGreenLine = this.gameObject.transform.GetChild(68).position.y;
       // float time = 0.01f;
        float time = 0; 

        //this is the start position of the object 
        Vector3 startPosition = Note1.transform.position;
        //the target interpolation is the y position plus half of its size 
        Vector3 targetPosition = new Vector3(Note1.transform.position.x, Ypos.y - (Note1.transform.localScale.y /2), -1);
        while (Note1.transform.position.y + (Note1.transform.localScale.y / 2) > YCordGreenLine)
        //changed rom >= to >. revert if it causes issues
            {
            Note1.transform.position = Vector3.Lerp(startPosition, targetPosition, time);
            // Debug.Log("startposition" + startPosition);
            //  Debug.Log("target" + targetPosition);
            time += Time.deltaTime;
            //original time

           
            //time = time * time * time * (time * (6f * time - 15f) + 10f);
            yield return null;            
        }        
      *
      *
      *
     */
}
