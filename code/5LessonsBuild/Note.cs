//based from the tutorial of https://www.youtube.com/watch?v=ev0HsmgLScg&t=156s 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;
    void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
    }

    // Update is called once per frame
    void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));


        if (t > 1)
        {
            Destroy(gameObject);
            //then light the key

        }
        else
        {
            //we need to modify this to get the x of pianokeys in rollmanager
            //so transfer the note keys here. get the note details 
            transform.localPosition = Vector3.Lerp(Vector3.up * SongManager.Instance.noteSpawnY, Vector3.up * SongManager.Instance.noteDespawnY, t);
           // GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}