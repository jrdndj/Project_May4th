using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PressListener : MonoBehaviour
{
    //global variable for all presses
  //  UnityEvent press = new UnityEvent();

    //this receives all inputs fromthe midi devices
  //  private InputAction movement;

    //instance of the inputactions asset
   // public static Midiactions inputActions = new Midiactions();
   // public static event Action<InputActionMap> actionMapChange;

    // Start is called before the first frame update
    void Start()
    {
        //we send here the listener event from the MIDIDevice
        //ToggleActionMap(inputActions.Player);

       // movement = PlayerInputManager.
        
    }

    public static void ToggleActionMaps(InputActionMap actionMap)
    {
      //  if (actionMap.enabled)
            return;

     //   inputActions.Disable();
        //actionMap.Change?.Invoke(actionMap);
      //  actionMap.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
