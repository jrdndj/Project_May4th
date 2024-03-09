using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseClic : MonoBehaviour
{
    [SerializeField] public GameObject ImprovManager;
    public bool isStopped = false;
    public Sprite StopSprite, PlaySprite;
    private SpriteRenderer buttonImage;

    public GameObject stop;

    public int index = 0; 


    // Start is called before the first frame update
    void Start()
    {
        buttonImage = GetComponent<SpriteRenderer>();
       // index = ImprovManager.GetComponent<ImprovMgr>().display_lesson_ctr;

    }

    // Update is called once per frame
    void Update()
    {

     //   index = ImprovManager.GetComponent<ImprovMgr>().display_lesson_ctr;
        // Check if the 'A' key is pressed
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Trigger the click event for the back button
            stop.SendMessage("OnStopButtonClick", SendMessageOptions.RequireReceiver);
        }
    }


    //stop button
    public void OnStopButtonClick()
    {
        index = ImprovManager.GetComponent<ImprovMgr>().display_lesson_ctr-1;
        isStopped = true;
        //simply destroy all spawns
        ImprovManager.GetComponent<ImprovMgr>().ClearSpawns();

        ImprovManager.GetComponent<ImprovMgr>().ManageSequence();
        //then generate based on current count
        //ImprovManager.GetComponent<ImprovMgr>().LoadSequence(ImprovManager.GetComponent<ImprovMgr>().modeValue, ImprovManager.GetComponent<ImprovMgr>().lessonValue, ImprovManager.GetComponent<ImprovMgr>().guidanceValue, index);
    }//end onstopbutton click

    public void TogglePlayButton()
    {
        if (isStopped)
        {
            buttonImage.sprite = PlaySprite;
        }
        else buttonImage.sprite = StopSprite;
    }//end toggle play button
}//end UIControl 
