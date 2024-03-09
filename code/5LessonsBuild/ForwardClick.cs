using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForwardClick : MonoBehaviour
{
    [SerializeField] public GameObject ImprovManager;
 

    public GameObject forward;
    public int index = 0;
    public int max = 0; 


    // Start is called before the first frame update
    void Start()
    {
      //  index = ImprovManager.GetComponent<ImprovMgr>().display_lesson_ctr;
      //  max = ImprovManager.GetComponent<ImprovMgr>().display_lesson_max; 

    }

    // Update is called once per frame
    void Update()
    {
    

        // Check if the 'A' key is pressed
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Trigger the click event for the back button
            // OnNextButtonClick();
            forward.SendMessage("OnNextButtonClick", SendMessageOptions.RequireReceiver);
        }

    
    }

  

    //back button
    public void OnNextButtonClick()
    {
        index = ImprovManager.GetComponent<ImprovMgr>().display_lesson_ctr-1;
        max = ImprovManager.GetComponent<ImprovMgr>().display_lesson_max;

        if (index < max)
        {
            //free objects
            ImprovManager.GetComponent<ImprovMgr>().ClearSpawns();
            index++;
            //increase count
            ImprovManager.GetComponent<ImprovMgr>().display_lesson_ctr = index;

            //reload based on this new value
            ImprovManager.GetComponent<ImprovMgr>().LoadSequence(ImprovManager.GetComponent<ImprovMgr>().modeValue, ImprovManager.GetComponent<ImprovMgr>().lessonValue, ImprovManager.GetComponent<ImprovMgr>().guidanceValue, index);
        }
       else  if (index == max)
        {
            //free objects
            ImprovManager.GetComponent<ImprovMgr>().ClearSpawns();
            ImprovManager.GetComponent<ImprovMgr>().ManageSequence();
            //index = 1; 
            ////revert to zero so we dont overflow
            //ImprovManager.GetComponent<ImprovMgr>().display_lesson_ctr = index;

            ////reload based on this new value
           // ImprovManager.GetComponent<ImprovMgr>().LoadSequence(ImprovManager.GetComponent<ImprovMgr>().modeValue, ImprovManager.GetComponent<ImprovMgr>().lessonValue, ImprovManager.GetComponent<ImprovMgr>().guidanceValue, index);

        }
        //else if (index < max)
        //{
        //    //free objects
        //    ImprovManager.GetComponent<ImprovMgr>().ClearSpawns();
        //    index++; 
        //    //increase count
        //    ImprovManager.GetComponent<ImprovMgr>().display_lesson_ctr = index;

        //    //reload based on this new value
        //    ImprovManager.GetComponent<ImprovMgr>().LoadSequence(ImprovManager.GetComponent<ImprovMgr>().modeValue, ImprovManager.GetComponent<ImprovMgr>().lessonValue, ImprovManager.GetComponent<ImprovMgr>().guidanceValue, index);
        //}

    }//endNextButton();

    
    
}//end UIControl 
