//this manages the toggles for guidance such as harmony metronome and rhythm
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //for ToList
using UnityEngine.UI; //added for colors and toggles
public class VizMgr : MonoBehaviour
{

    [SerializeField] GameObject ImprovMgr;

    //toggle declarations here
    public Toggle noviztoggle;
    public bool noviz = false; 


    // Start is called before the first frame update
    void Start()
    {
        //rhythm toggle listener
        noviztoggle.GetComponent<Toggle>();
        noviztoggle.onValueChanged.AddListener(delegate
        {
            NoVizToggleValueChanged(noviztoggle);
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void NoVizToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            noviz = true;
            // guidancevalue = DetermineAccompaniement();

            Debug.Log("Condition 2 ON");
            ImprovMgr.GetComponent<ImprovMgr>().noviz = 1; 
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "No Viz will be generated";

    

        }//endif 
        else
        {
            //change to default lesson
            // lesson = 9;
            noviz = false;
   

            Debug.Log("Condition 2 OFF");
            ImprovMgr.GetComponent<ImprovMgr>().noviz = 0;
            // ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Removed rhythm.";
            // display_name.text = "select lesson";

            //now send the value to ImprovMgr
            //   ImprovMgr.GetComponent<ImprovMgr>().guidanceValue = 9;

        }//endelse 

    }//end rhythrmh toggle 

}
