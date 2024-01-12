using UnityEngine;
using UnityEngine.UI;

public class Checkbox : MonoBehaviour
{
    private Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();

        // Find the CheckboxGroup script in the scene and register this toggle
        CheckboxToggle checkboxGroup = FindObjectOfType<CheckboxToggle>();

        if (checkboxGroup != null)
        {
            //checkboxGroup.RegisterToggle(toggle);
        }
    }
}
