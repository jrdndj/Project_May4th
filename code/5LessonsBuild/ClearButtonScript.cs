using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearButtonScript : MonoBehaviour
{
    [SerializeField] GameObject ImprovManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearButtonPressed()
    {
        Debug.Log("Cleared objects");
        //when pressed calls all the respective methods and loads everything
        ImprovManager.GetComponent<ImprovMgr>().ResetAllValues();

  
    }//end loadbutton pressed
}//end class
