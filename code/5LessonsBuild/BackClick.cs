using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class BackClick : MonoBehaviour
{
    [SerializeField] public GameObject ImprovManager;


    public GameObject back;

    public int index = 0;


    // Start is called before the first frame update
    void Start()
    {

        //index = ImprovManager.GetComponent<ImprovMgr>().display_lesson_ctr;
    }

    // Update is called once per frame
    void Update()
    {
      
        // Check if the 'A' key is pressed
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Trigger the click event for the back button
            // OnBackButtonClick();
            back.SendMessage("OnBackButtonClick", SendMessageOptions.RequireReceiver);

        }

     
    }//end update

    //back button
    public void OnBackButtonClick()
    {
        ImprovManager.GetComponent<ImprovMgr>().seqctr = 0;
        index = ImprovManager.GetComponent<ImprovMgr>().display_lesson_ctr-1;

        if (index==1)
        {
            //free objects
            ImprovManager.GetComponent<ImprovMgr>().ClearSpawns();
            index = 1;
            //no change in value
            ImprovManager.GetComponent<ImprovMgr>().ManageSequence();
            //reload based on this new value
           // ImprovManager.GetComponent<ImprovMgr>().LoadSequence(ImprovManager.GetComponent<ImprovMgr>().modeValue, ImprovManager.GetComponent<ImprovMgr>().lessonValue, ImprovManager.GetComponent<ImprovMgr>().guidanceValue, index);

        }
        else if (index >= 2)
        {
            //free objects
            ImprovManager.GetComponent<ImprovMgr>().ClearSpawns();
            index--;

            //decrease count
            ImprovManager.GetComponent<ImprovMgr>().display_lesson_ctr = index;
         //   ImprovManager.GetComponent<ImprovMgr>().harmonyindex--;
        //    Debug.Log("harmony to assign is " + ImprovManager.GetComponent<ImprovMgr>().harmonyindex);

            //reload based on this new value
            ImprovManager.GetComponent<ImprovMgr>().LoadSequence(ImprovManager.GetComponent<ImprovMgr>().modeValue, ImprovManager.GetComponent<ImprovMgr>().lessonValue, ImprovManager.GetComponent<ImprovMgr>().guidanceValue, index);
        }

    }//endOnBackButtonClick();

   
}//end UIControl 
