using WebSocketSharp; 
using UnityEngine;

public class WS_Client : MonoBehaviour
{
    WebSocket ws;

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
