using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //for the toggles to work
using System.Linq; //for ToList

public class VizMgr : MonoBehaviour
{

    //an important element to manage all children of spawns
    [SerializeField] GameObject RollManager;

    //putting all toggles here
    public Toggle jazzlistener, blueslistener, HSlistener, SAlistener, Whitelistener, RedListener;
    //public Toggle rollmodeListener, onpressmodelistener, guidedlistener, jazzlistener, blueslistener, HSlistener, SAlistener, Whitelistener, RedListener;
    public Toggle chordnamelistener, endresolvelistener, maplinelistener, keynamelistener;

    // Start is called before the first frame update
    void Start()
    {

        //initialize all toggles here

        ////rollmode toggle listener

        //====== these things stay in RollScript 
        //rollmodeListener.GetComponent<Toggle>();
        //rollmodeListener.onValueChanged.AddListener(delegate
        //{
        //    RollModeValueChanged(rollmodeListener);
        //});

        ////on press mode toggle listener
        //onpressmodelistener.GetComponent<Toggle>();
        //onpressmodelistener.onValueChanged.AddListener(delegate
        //{
        //    OnPressValueChanged(onpressmodelistener);
        //});

        ////guided press mode toggle listener
        //guidedlistener.GetComponent<Toggle>();
        //guidedlistener.onValueChanged.AddListener(delegate
        //{
        //    GuidedValueChanged(guidedlistener);
        //});

        //jazz improv listener
        jazzlistener.GetComponent<Toggle>();
        jazzlistener.onValueChanged.AddListener(delegate
        {
            JazzValueChanged(jazzlistener);
        });

        //blues improv listener
        blueslistener.GetComponent<Toggle>();
        blueslistener.onValueChanged.AddListener(delegate
        {
            BluesValueChanged(blueslistener);
        });

        //bkeyname lick count listener
        keynamelistener.GetComponent<Toggle>();
        keynamelistener.onValueChanged.AddListener(delegate
        {
            KeyNameValueChanged(keynamelistener);
        });

        //halfstep listener
        HSlistener.GetComponent<Toggle>();
        HSlistener.onValueChanged.AddListener(delegate
        {
            HSValueChanged(HSlistener);
        });

        //halfstep listener
        SAlistener.GetComponent<Toggle>();
        SAlistener.onValueChanged.AddListener(delegate
        {
            SAValuechanged(SAlistener);
        });

    }//end Start

    // Update is called once per frame
    void Update()
    {
        
    }//end update

    //the ValueChanged functions here

   

    public void JazzValueChanged(Toggle change)
    {
        // ctr = 0; 
        if (change.isOn)
        {
            RollManager.GetComponent<RollScript>().genre = 0; 
            //genre = 0;
            Debug.Log("Jazz Improvs will be shown");

        }
        else
        {
            //change to default vizmode
            RollManager.GetComponent<RollScript>().genre = 1;
            Debug.Log("Blues Improvs will be shown");
            //
            //ChordManager.GetComponent<ChordMgr>().ChordMapper(Blues001);
        }
    }//end blues values toggle

    public void BluesValueChanged(Toggle change)
    {
        //ctr = 0; 
        if (change.isOn)
        {
            RollManager.GetComponent<RollScript>().genre = 1;
            Debug.Log("Blues Improvs will be shown");


        }
        else
        {

            RollManager.GetComponent<RollScript>().genre = 0;
            Debug.Log("Jazz Improvs will be shown");

        }
    }//end bluesvalues toggle

    public void KeyNameValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            RollManager.GetComponent<RollScript>().showLickCount = true;
        }//end
        else
        {
            RollManager.GetComponent<RollScript>().showLickCount = false;
        }
    }//end keynamevaluechanged

    //public int getBluesSequence(int order)
    //{
    //    return bluessequence[order];
    //}//end getBlues sequence number

    public void HSValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            RollManager.GetComponent<RollScript>().enablehalfstep = true;
        }//end
        else
        {
            RollManager.GetComponent<RollScript>().enablehalfstep = false;
        }
    }//end keynamevaluechanged

    public void SAValuechanged(Toggle change)
    {
        if (change.isOn)
        {
            RollManager.GetComponent<RollScript>().enablestepabove = true;
        }//end
        else
        {
            RollManager.GetComponent<RollScript>().enablestepabove = false;
        }
    }//end keynamevaluechanged
}
