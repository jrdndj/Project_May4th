//contains toggle listeners for modes which is either
//listen and learn or
// test yourself
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //for ToList
using UnityEngine.UI; //added for colors and toggles


//===== general algorithm for mode mgr is
// once values have been selected
// and are set in module
// then these values must be set to Improv Mgr
public class ModeMgr : MonoBehaviour
{
    [SerializeField] GameObject ImprovMgr;

    //=== declare toggle listeners here
    public Toggle listentoggle, trytoggle, testtoggle, composetoggle;

    //declare variable to send to ImprovMgr
    //  int module = 9; //default set it to zero
    // 1 if listen
    // 2 if test yourself
    // 3 if try yourself
    // 4 if compose (only for lessons 4 and 5) 

    // Start is called before the first frame update
    void Start()
    {

        //initialize toggle listeners here
        //watch mode
        listentoggle.GetComponent<Toggle>();
        listentoggle.onValueChanged.AddListener(delegate
        {
            ListenToggleValueChanged(listentoggle);
        });

        //try mode
        trytoggle.GetComponent<Toggle>();
        trytoggle.onValueChanged.AddListener(delegate
        {
            TryToggleValueChanged(trytoggle);
        });

        //test mode
        testtoggle.GetComponent<Toggle>();
        testtoggle.onValueChanged.AddListener(delegate
        {
            TestToggleValueChanged(testtoggle);
        });

        //compose mode
        composetoggle.GetComponent<Toggle>();
        composetoggle.onValueChanged.AddListener(delegate
        {
            TestToggleValueChanged(composetoggle);
        });

    }

    //=== some functions here that manage the values that will all be sent to the improv manager

    //watch toggle value changed
    public void ListenToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            //  module = 1; //watch and listen mode
            Debug.Log("Selected Listen and Learn mode");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Selected Listen and Learn mode, now select a lesson";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().modeValue = 1;

        }//endif 
        else
        {
            //change to default module
            //   module = 9;
            Debug.Log("Unselected Listen and Learn mode");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Select a mode to begin with.";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().modeValue = 9;

        }//endelse 
    }//end listentoggle

    //watch toggle value changed
    public void TryToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            //  module = 3; //try yourself
            Debug.Log("Selected Try Yourself mode");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Selected Try Yourself mode, now select a lesson";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().modeValue = 3;

        }//endif 
        else
        {
            //change to default module
            // module = 9;
            Debug.Log("Unselected Try Yourself mode");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Select a mode to begin with.";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().modeValue = 9;

        }//endelse 
    }//end listentoggle


    //test toggle
    public void TestToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            //   module = 2; //test mode
            Debug.Log("Selected Test Yourself mode");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Selected Test Yourself mode, now select a lesson";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().modeValue = 2;

        }//endif 
        else
        {
            //change to default module
            //   module = 9;
            Debug.Log("Unselected Listen and Learn mode.");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Select a mode to begin with.";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().modeValue = 9;

        }//endelse 
    }//end testtoggle

    //compose toggle
    public void ComposeToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            //   module = 4; //compose mode
            Debug.Log("Selected Compose mode");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Compose Mode valid only for L4 and L5";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().modeValue = 4;

        }//endif 
        else
        {
            //change to default module
            //   module = 9;
            Debug.Log("Unselected Compose mode.");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Select a mode to begin with.";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().modeValue = 9;

        }//endelse 
    }//end testtoggle

}//end ModeMgr

