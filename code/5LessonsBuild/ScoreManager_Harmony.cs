//based from the tutorial of https://www.youtube.com/watch?v=ev0HsmgLScg&t=156s 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is for logging of hits and miss on the harmony keys 
public class ScoreManager_Harmony : MonoBehaviour
{
    public static ScoreManager_Harmony Instance;
    //these are for sound effects we dont need this for now 
    public AudioSource hitSFX;
    public AudioSource missSFX;
    //i dont really need this since i dont wanna use this 
    public TMPro.TextMeshPro scoreText;
    static int comboScore;
    void Start()
    {
        Instance = this;
        comboScore = 0;
    }
    public static void Hit()
    {
        comboScore += 1;
     //   Instance.hitSFX.Play();
    }
    public static void Miss()
    {
        comboScore = 0;
      //  Instance.missSFX.Play();
    }
    private void Update()
    {
       // scoreText.text = comboScore.ToString();
    }
}//ebd ScoreManager_Harmony