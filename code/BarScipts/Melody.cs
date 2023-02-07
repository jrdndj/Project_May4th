using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* THIS CODE IS OBSOLETE REFER TO PARSEMIDI.CS */
public class Melody : MonoBehaviour {

    // Use this for initialization
    //this is the canvas to instanciate the notes

    //so far notes are created manually see this sample code

 //melodyNotes = new string[] { "FA3", "FA3", "FA3", "FA3", "MI3", "FA3", "FA3", ... };
   // melodyDuration = new float[] {  0.5f , 0.25f, 0.5f , 0.5f , 0.5f ,  1.0f, ...};
   //we can modify this using musicXML 

public Canvas canv;
    public GameObject note_prefab;

   
    public Transform DO2, RE2, MI2, FA2, SOL2, LA2, SI2, DO3, RE3, MI3, FA3,SOL3, LA3, SI3, DO4, RE4, MI4, FA4, SOL4, LA4, SI4, DO5, RE5, MI5, FA5, SOL5, LA5, SI5, DO6, RE6, MI6, FA6, SOL6, LA6, SI6, DO7;
    public Transform DOs2, REs2, FAs2, SOLs2, LAs2, DOs3, REs3, FAs3, SOLs3, LAs3, DOs4, REs4, FAs4, SOLs4, LAs4, DOs5, REs5, FAs5, SOLs5, LAs5, DOs6, REs6, FAs6, SOLs6, LAs6;

    // these are the two variables that create the notes and duration
    string[] melodyNotes;
    float[] melodyDuration;

    //this is the partiture speed
    public float partitureSpeed=25;

    void Start ()
    {
        // define the melody with the notes and durations
        // feed song as part of melody notes and as a sequence 
        //add a function here where we load a song, then use musicXML and then feed it to melodynotes 
        //melodyNotes =    new string[] { "FA3", "FA3", "FA3", "FA3", "MI3", "FA3","FA3","MI3","FA3","SOL3","LA3","SOL3", "FA4", "FA4", "FA4", "FA4", "MI4", "FA4", "FA4", "MI4", "FA4", "SOL4", "LA4", "SOL4" };
        
        //adding this to test all notes if they are working including grey notes 
        melodyNotes = new string[] { "DOs2", "REs2", "FAs2", "SOLs2", "LAs2", "DOs3", "REs3", "FAs3", "SOLs3", "LAs3", "DOs4", "REs4", "FAs4", "SOLs4", "LAs4", "DOs5", "REs5", "FAs5", "SOLs5", "LAs5", "DOs6", "REs6", "FAs6", "SOLs6", "LAs6" };
        melodyDuration = new float[] { 0.5f, 0.25f, 0.5f, 0.5f, 0.5f,  1.0f, 0.5f, 0.5f, 1.0f, 0.5f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f}; 
        //for example: 0.5f means FA3 note has 0.5 duration. the standard partiture speed is 25. 


        //this is used to know were to place the notes
        float cumulativeTime =0.0f;

        //create the notes
        for (int i=0; i<melodyNotes.Length;i++)
        {
            GameObject keyRef=GameObject.Find(melodyNotes[i]);

            //instanciate notes
            GameObject note= GameObject.Instantiate(note_prefab,keyRef.transform.position+new Vector3(0,500+cumulativeTime*partitureSpeed,0),Quaternion.Euler(0,0,0));
            //melody notes here is processed as an array and is called in the loop 
            note.GetComponent<Note>()._name = melodyNotes[i];
            note.transform.SetParent(canv.transform);

            //set new time for the notes
            cumulativeTime += (float)melodyDuration[i];
            //Debug.Log(cumulativeTime);
        }


    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
		
	}

    public void instanciateNote()
    {

    }

}
