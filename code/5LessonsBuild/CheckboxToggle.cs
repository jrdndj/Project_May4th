//this code was written with the help of chat GPT 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckboxToggle : MonoBehaviour
{
    private List<Toggle> toggles = new List<Toggle>();

    void Start()
    {
        // Find all Toggle components in the scene
        Toggle[] allToggles = FindObjectsOfType<Toggle>();

        // Add each Toggle to the list and register a listener
        foreach (Toggle toggle in allToggles)//we need to modify this that only the accompaniement ones are added
        {
            toggles.Add(toggle);
            toggle.onValueChanged.AddListener((value) => ToggleValueChanged(toggle, value));
        }
    }

    private void ToggleValueChanged(Toggle changedToggle, bool newValue)
    {
        // If a toggle is turned on, turn off all other toggles
        if (newValue)
        {
            foreach (Toggle toggle in toggles)
            {
                if (toggle != changedToggle)
                {
                   // toggle.isOn = false;
                }
            }
        }
    }//end togglevaluechanged
}//end class
