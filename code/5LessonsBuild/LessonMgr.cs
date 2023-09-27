//this contains all the toggle listeners for the lessons 01 to 05 and passess
// it to the content manager to decide which content will be produced.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //for ToList
using UnityEngine.UI; //added for colors and toggles

public class LessonMgr : MonoBehaviour
{
    [SerializeField] GameObject ImprovMgr;

    //declare listener toggles here
    public Toggle lesson01_modeswing, lesson02_sequencing, lesson03_motifs, lesson04_variations, lesson05_quesans;

    //some variables for tracking
    public int lesson = 9; //default is 9
    // 1 modeswing
    // 2 sequencing
    // 3 motifs
    // 4 variations
    // 5 quesans

    // Start is called before the first frame update
    void Start()
    {

        //initialize listener toggles here

        //lesson 01 toggle listener
        lesson01_modeswing.GetComponent<Toggle>();
        lesson01_modeswing.onValueChanged.AddListener(delegate
        {
            Lesson01ToggleValueChanged(lesson01_modeswing);
        });

        //lesson 02
        lesson02_sequencing.GetComponent<Toggle>();
        lesson02_sequencing.onValueChanged.AddListener(delegate
        {
            Lesson02ToggleValueChanged(lesson02_sequencing);
        });

        //lesson 03
        lesson03_motifs.GetComponent<Toggle>();
        lesson03_motifs.onValueChanged.AddListener(delegate
        {
            Lesson03ToggleValueChanged(lesson03_motifs);
        });

        //lesson 04
        lesson04_variations.GetComponent<Toggle>();
        lesson04_variations.onValueChanged.AddListener(delegate
        {
            Lesson04ToggleValueChanged(lesson04_variations);
        });

        //lesson 05
        lesson05_quesans.GetComponent<Toggle>();
        lesson05_quesans.onValueChanged.AddListener(delegate
        {
            Lesson05ToggleValueChanged(lesson05_quesans);
        });

    }

    //we dont need you yet
    //// Update is called once per frame
    //void Update()
    //{

    //}//end update

    //=====declare toggle listener functions here
    //Lesson 01
    public void Lesson01ToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            lesson = 1; 
            Debug.Log("Selected Lesson 01: All Swing Modes");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Selected Lesson 01. Choose guidance or you may Press Load";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 1;

        }//endif 
        else
        {
            //change to default lesson
            lesson = 9;
            Debug.Log("Deselected Lesson 01: All Swing Modes");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Select a lesson to continue.";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 9;

        }//endelse 
    }//end lesson01toggle

    //Lesson 02
    public void Lesson02ToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            lesson = 2;
            Debug.Log("Selected Lesson 02: Sequencing");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Selected Lesson 02. Choose guidance or you may Press Load";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 2;

        }//endif 
        else
        {
            //change to default lesson
            lesson = 9;
            Debug.Log("Deselected Lesson 02: Sequencing");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Select a lesson to continue.";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 9;

        }//endelse 
    }//end lesson02toggle

    //Lesson 03
    public void Lesson03ToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            lesson = 3;
            Debug.Log("Selected Lesson 03: Motif Learning");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Selected Lesson 03. Choose guidance or you may Press Load";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 3;

        }//endif 
        else
        {
            //change to default lesson
            lesson = 9;
            Debug.Log("Deselected Lesson 03: Motif Learning");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Select a lesson to continue.";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 9;

        }//endelse 
    }//end lesson03toggle

    //Lesson 04
    public void Lesson04ToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            lesson = 4;
            Debug.Log("Selected Lesson 04: Variations");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Selected Lesson 04. Choose guidance or you may Press Load";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 4;

        }//endif 
        else
        {
            //change to default lesson
            lesson = 9;
            Debug.Log("Deselected Lesson 04: Variations");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Select a lesson to continue.";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 9;

        }//endelse 
    }//end lesson04toggle

    //Lesson 05
    public void Lesson05ToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            lesson = 5;
            Debug.Log("Selected Lesson 05: Questions Answers");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Selected Lesson 05. Choose guidance or you may Press Load";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 5;

        }//endif 
        else
        {
            //change to default lesson
            lesson = 9;
            Debug.Log("Deselected Lesson 05: Questions Answers");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Select a lesson to continue.";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 9;

        }//endelse 
    }//end lesson05toggle


}//end lessonmgr
