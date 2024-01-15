using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class LoadButtonScript : MonoBehaviour
{
   
    [SerializeField] GameObject RollManager, ImprovManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadButtonPressed()
    {
        Debug.Log("Load button Pressed");
        //when pressed calls all the respective methods and loads everything
        ImprovManager.GetComponent<ImprovMgr>().ManageImprov();
        ImprovManager.GetComponent<ImprovMgr>().display_text.text = "Lesson ongoing.."; 
        //RollManager.GetComponent<RollMgr>().GeneratePianoRoll();
    }//end loadbutton pressed

}//end load button listener
