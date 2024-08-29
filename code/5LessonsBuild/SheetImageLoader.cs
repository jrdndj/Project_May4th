using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //needed for this setup

public class SheetImageLoader : MonoBehaviour
{
    public Image imageUI; // Reference to the UI Image component

    // Start is called before the first frame update
    void Start()
    {
        
    }//end start

    // Update is called once per frame
    void Update()
    {
        
    }//end update

    //a method to load and display images
    public void LoadSheetImage(string filename)
    {
        // Construct the full path based on the filename
        string fullPath = "Images/" + filename;

        // Load the sprite from the Resources folder
        Sprite loadedSprite = Resources.Load<Sprite>(fullPath);
     

        // Check if the image was successfully loaded
        if (loadedSprite != null)
        {
            // Assign the loaded sprite to the UI Image component
            imageUI.sprite = loadedSprite;
          //  Debug.Log("Image loaded successfully: " + filename);
        }
        else
        {
            Debug.LogError("Failed to load image: " + filename);
        }
    }//end loadsheetimage

    // Method to clear the image
    public void ClearImage()
    {
        // Set the sprite to null to clear the image
        imageUI.sprite = null;
       // Debug.Log("Image cleared.");
    }

}//endclass
