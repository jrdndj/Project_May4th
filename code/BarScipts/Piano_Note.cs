using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//color block belongs to unityengine ui
using UnityEngine.UI;

public class Piano_Note : MonoBehaviour {

    // Use this for initialization

    //this is the defaut audio source attached to the object
    //AudioSource audioS;
    //this is used to delay or advance the sound of the key/note
    public float delay=0.1f;
    //this is used to get the initialposition of the object
    Vector3 posInit;
    // movement of the key distance
    public float distKey = 50;
    //name of the note
    public string _name;

    //THIS IS THE REFERENCE FOR THE SCORING, and the scoring distance d;
    public Score sc;

    //button color change related variables
    //variable declaration for color when wrong 
    public ColorBlock theColor;
    public Button theButton;

    //adding this variable as flag to stop continue the prototype
    public bool moveFlag = false;

    //variable container for current mouse location
   // Vector3 mousePos = Input.mousePosition;

    //array for storage
   // Vector3[] clickPos = new[] { new Vector3 { x=mousePos.x, y=mousePos.y }, 
  //                               new Vector3 { x=0, y=0 } };
    //this stores in a list of clickPos so further processing later

	void Start ()
    {
        //inicialization of variables
       // audioS = transform.GetComponent<AudioSource>();
        posInit = transform.localPosition;
        _name = transform.gameObject.name;
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    //this function plays the tone that is instead the note or key when it is pressed
    public void playTone()
    {
        //we set the time to the delay and we play the sound
       // audioS.time =delay;
       // audioS.Play();
        //we move the key to simulate keypress
       // StartCoroutine(moveKey());

        // we check which note corresponds to to this value of note
        checkNotePosition();

        //put here code to get the current mouse position (spatio component) 
        //the temporal component is defined in the melody script 
    }

    // this simulates the user pressing the key
    //need to replace this with the MIDI piano press behavior
    //we dont need to function anymore this is annoying
   /* IEnumerator moveKey()
    {
        for (float teta=0;teta<=Mathf.PI; teta+=Mathf.PI/10)
        {
            //this is the movement of the key
            transform.localPosition = new Vector3(posInit[0], posInit[1]- distKey*Mathf.Sin(teta), posInit[2]);

            //yield return new WaitForSeconds(.01f);
            yield return null;
        }

        transform.localPosition = posInit;
    } */

    //this get the position of the note and destroys it when the key is touched
    void checkNotePosition()
    {
       //get button information for color transformation
        theButton = GetComponent<Button>();
        theColor = GetComponent<Button>().colors;

        //this destroys the object when the correct note is pressed
        GameObject go = GameObject.FindGameObjectWithTag("note");
        if (go != null)
        {
            if (go.GetComponent<Note>()._name == _name)
            {

                //this manages the scoring
                //dont really need this function but im keeping it for debugging purposes
                int scr = calculateScore(go.transform.position[1], sc.scoringLine.position[1]);
                sc.score += scr;
                //update moveFlag to true 
                moveFlag = true;


                //destroy note
                //this creates the star effect when a note is "pressed". 
                //for debugging: to check keypress
                go.GetComponent<Note>().particles();
                Destroy(go);

                //putting the code here so it only happens when it is destroyed (when correct)
                //change color to green when pressed correctly
                //everything else is red because it is not the proper press
                theColor.pressedColor = Color.green;
                theButton.colors = theColor;
                //@1445 i hope this works
                //@1523 it works! but only when pressed on the super correct position
            }

            else
            {
                //assume wrong and enforce color red
                theColor.pressedColor = Color.red;
                theButton.colors = theColor;
                //changeFlag to false and pause the entire thing
                moveFlag = false; 
            }
            
        }
    }

    int calculateScore(float x, float r)
    {
        int scr = 0;

        if(x-r>0 && x-r<=sc.d)
        {
            scr = (int) (-10 / sc.d *x +10+10 / sc.d *r);

        }
        else if (x - r < 0 && x-r >= -sc.d)
        {
            scr = (int)(10 / sc.d * x +10- 10 / sc.d * r);
        }


            return scr;
    }


}
