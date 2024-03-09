using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//adding namespace o ABCUnity to access basiclayout componment 
using ABCUnity; 
using ABCUnity.Example;

public class MusicSheetManager : MonoBehaviour
{
    //to directly modify the resourcename of ABCNotationHandler object
    [SerializeField] GameObject ABCNotationHandler, ABCLayout;

    //we need to get the values from lesson manager
    //[SerializeField] GameObject LessonMgr;
    //actually it should be LessonMgr who is sending messages directly to MusicSheetMgr
    public bool lesson01, lesson02, lesson03, lesson04, lesson05;
    public bool watch, trymode;
    //public List<string> sheetnamelist = new List<string>();

    //a function to show the correct abc notation based on user selection

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //receive the music sheet file names from improv manager
    public void SetSheetFilename(string sheetfilename)
    {
        ABCNotationHandler.GetComponent<BasicLayout>().resourceName = sheetfilename;
     //   Debug.Log( sheetfilename + " staff has been generated.");

    }//end receive sheetfilenames

    public void ClearSheets()
    {
        ABCNotationHandler.GetComponent<BasicLayout>().resourceName = " ";
        ABCLayout.GetComponent<Layout>().Clear();
    }//clear sheets

    


}//end music sheet manager
