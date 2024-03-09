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

    //we need to declare the lessons here so we just pass them around
    public List<List<string>> lessonlist = new List<List<string>>();
    public List<List<string>> sheetlist = new List<List<string>>();

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

        //print it to check
        SetLessons();
        SetSheetFilenames();
        //it works! commenting it now
        //PrintAllLessons(lessonlist);

    }//end start


    //=====declare toggle listener functions here
    //Lesson 01
    public void Lesson01ToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            lesson = 1;
            Debug.Log("Selected Lesson 01: All Swing Modes");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Selected Lesson 01. Press Load to begin.";

            //now send the value to ImprovMgr and MusicSheetManager
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 1;
          //  MusicSheetMgr.GetComponent<MusicSheetManager>().lesson01 = true; 

        }//endif 
        else
        {
            //change to default lesson
            lesson = 9;
            Debug.Log("Deselected Lesson 01: All Swing Modes");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Select a lesson to continue.";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 9;
          //  MusicSheetMgr.GetComponent<MusicSheetManager>().lesson01 = false;

        }//endelse 
    }//end lesson01toggle

    //Lesson 02
    public void Lesson02ToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            lesson = 2;
            Debug.Log("Selected Lesson 02: Sequencing");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Selected Lesson 02. Press Load to begin.";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 2;
         //   MusicSheetMgr.GetComponent<MusicSheetManager>().lesson02 = true;

        }//endif 
        else
        {
            //change to default lesson
            lesson = 9;
            Debug.Log("Deselected Lesson 02: Sequencing");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Select a lesson to continue.";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 9;
         //   MusicSheetMgr.GetComponent<MusicSheetManager>().lesson02 = false;

        }//endelse 
    }//end lesson02toggle

    //Lesson 03
    public void Lesson03ToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            lesson = 3;
            Debug.Log("Selected Lesson 03: Motif Learning");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Selected Lesson 03. Press Load to begin.";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 3;
         //   MusicSheetMgr.GetComponent<MusicSheetManager>().lesson03 = true;

        }//endif 
        else
        {
            //change to default lesson
            lesson = 9;
            Debug.Log("Deselected Lesson 03: Motif Learning");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Select a lesson to continue.";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 9;
          //  MusicSheetMgr.GetComponent<MusicSheetManager>().lesson03 = false;

        }//endelse 
    }//end lesson03toggle

    //Lesson 04
    public void Lesson04ToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            lesson = 4;
            Debug.Log("Selected Lesson 04: Variations");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Selected Lesson 04. Press Load to begin.";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 4;
         //   MusicSheetMgr.GetComponent<MusicSheetManager>().lesson04 = true;

        }//endif 
        else
        {
            //change to default lesson
            lesson = 9;
            Debug.Log("Deselected Lesson 04: Variations");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Select a lesson to continue.";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 9;
        //    MusicSheetMgr.GetComponent<MusicSheetManager>().lesson04 = false;

        }//endelse 
    }//end lesson04toggle

    //Lesson 05
    public void Lesson05ToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            lesson = 5;
            Debug.Log("Selected Lesson 05: Questions Answers");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Selected Lesson 05. Press Load to begin.";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 5;
        //    MusicSheetMgr.GetComponent<MusicSheetManager>().lesson05 = true;

        }//endif 
        else
        {
            //change to default lesson
            lesson = 9;
            Debug.Log("Deselected Lesson 05: Questions Answers");
            ImprovMgr.GetComponent<ImprovMgr>().display_text.text = "Select a lesson to continue.";

            //now send the value to ImprovMgr
            ImprovMgr.GetComponent<ImprovMgr>().lessonValue = 9;
         //   MusicSheetMgr.GetComponent<MusicSheetManager>().lesson05 = false;

        }//endelse 
    }//end
     //

    //when updating the files, just update these lessons. 
    public void SetLessons()
    {
        //lesson 01 is more specific
        List<string> lesson1 = new List<string>(){
            "L01_01C_viz.mid",
            "L01_02D_viz.mid",
            "L01_03E_viz.mid",
            "L01_04F_viz.mid",
            "L01_05G_viz.mid",
            "L01_06A_viz.mid",
            "L01_07B_viz.mid"
        };

        lessonlist.Add(lesson1);

        //so is lesson 02 
        List<string> lesson2 = new List<string>(){
            "L02_01UU_viz.mid",
            "L02_02UUe_viz.mid",
            "L02_03DD_viz.mid",
            "L02_04DDe_viz.mid",
            "L02_05UD_viz.mid",
            "L02_06UDe_viz.mid",
            "L02_07DU_viz.mid",
            "L02_08DUe_viz.mid"
        };

        lessonlist.Add(lesson2);

        //3 to 8 is more straightforward so we use that
        // Lessons 3 to 5 with 8 sublessons each
        for (int i = 3; i <= 5; i++)
        {
            List<string> lesson = new List<string>();
            for (int j = 1; j <= 8; j++)
            {
                lesson.Add($"L{i:D2}_{j:D2}_viz.mid");
            }
            lessonlist.Add(lesson);
        }//end for loop

    }//end SetLessons

    ////when updating the files, just update these lessons. 
    public void SetSheetFilenames()
    {
        //=== something to take note here is that since this is a seralizable game object
        //we remove the .txt in the extension even though the real files are using .abc.txt 
        //lesson 01 is more specific
        List<string> lesson1 = new List<string>(){
            "L01_01C_viz.abc",
            "L01_02D_viz.abc",
            "L01_03E_viz.abc",
            "L01_04F_viz.abc",
            "L01_05G_viz.abc",
            "L01_06A_viz.abc",
            "L01_07B_viz.abc"
        };

        sheetlist.Add(lesson1);

        //so is lesson 02 
        List<string> lesson2 = new List<string>(){
            "L02_01UU_viz.abc",
            "L02_02UUe_viz.abc",
            "L02_03DD_viz.abc",
            "L02_04DDe_viz.abc",
            "L02_05UD_viz.abc",
            "L02_06UDe_viz.abc",
            "L02_07DU_viz.abc",
            "L02_08DUe_viz.abc"
        };

        sheetlist.Add(lesson2);

        //3 to 8 is more straightforward so we use that
        // Lessons 3 to 5 with 8 sublessons each
        for (int i = 3; i <= 5; i++)
        {
            List<string> lesson = new List<string>();
            for (int j = 1; j <= 8; j++)
            {
                lesson.Add($"L{i:D2}_{j:D2}_viz.abc");
            }
            sheetlist.Add(lesson);
        }//end for loop

    }//end SetLessons

    //for sanity's sake here's a print to check
    void PrintAllLessons(List<List<string>> lessons)
    {
        for (int i = 0; i < lessons.Count; i++)
        {
            Debug.Log($"Lesson {i + 1}:");
            foreach (var sublesson in lessons[i])
            {
                Debug.Log(sublesson);
            }
            Debug.Log("");
        }
    }//end printall lessons


}//end lessonmgr
