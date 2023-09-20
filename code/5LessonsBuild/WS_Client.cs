// this was developed using the tutorial from https://www.youtube.com/watch?v=13HnJPstnDM 
using WebSocketSharp; 
using UnityEngine;

public class WS_Client : MonoBehaviour
{
    WebSocket ws;

    //==== general algorithm would be
    // start collecting keypress for logs on try yourself mode
    // compile logs and call ParseMgr to generate midi files
    // mid files are now sent and picked up to the server
    // server drops them dynamically 

    // Start is called before the first frame update
    void Start()
    {
        ws = new WebSocket("ws://localhost:8080");
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
        };

        //donnt forget to start connection
        ws.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        if(ws == null)
        {
            return; 
        }

        //these are the lines where you send the keypresses or the created mid files so the s
        //server drpops it somewhere for the app to pick up
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ws.Send("Hello");
        }
    }
}
