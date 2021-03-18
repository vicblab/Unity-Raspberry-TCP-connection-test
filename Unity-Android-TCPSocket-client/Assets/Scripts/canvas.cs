
//-----------------------------------------------------------------------------------------------------------
// Script for controlling the simple canvas based UI needed to control the Raspberry LED and display its state
// 
// Victor Blanco Bataller
// 2021/03/18
// Turku University of Applied Sciences
//----------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class canvas : MonoBehaviour
{

    // This object is the one which will handle the TCP socket connection
    public GameObject TCPListener;

    // Button for toggling the connection (sometimes it is kinda buggy)
    public Button connect;

    // Here is where the message recieved from the server will be displayed (on or off)
    public Text display;

    //public InputField input;
    
    // boolean variables necessary for the toggling LED and connect button
    public bool isoff;
    public bool isconn;
    
    // Start is called before the first frame update
    void Start()
    {
        
        isoff = false;
        isconn = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //------------------------------
    // onclick()
    // called when the toggle connection button is pressed, it starts or closes the connection depending on the value of iscon
    //----------------------------
    public void onclick()
    {

        
        if (isconn)
        {
            //connect.GetComponentsInChildren<GameObject>()[0].GetComponent<Text>().text = "Disconnect";
            TCPListener.GetComponent<Listener>().closeSocket();
        }
        else
        {
            //connect.GetComponentsInChildren<GameObject>()[0].GetComponent<Text>().text = "Connect";
            TCPListener.GetComponent<Listener>().setupSocket();
        }
        isconn = !isconn;
        
        Debug.Log("Button Clicked");
    }

    //----------------------------------------
    // toggle()
    // Function called every time the Toglle led button is pressed. It sends the on or off command to the server using our TCPListener
    // Its message depends on the value of isoff
    //-----------------------------------------
    public void toggle()
    {
        string command = "on";

        if (isoff) command = "off";

        TCPListener.GetComponent<Listener>().writeSocket(command);

        isoff = !isoff;
    }
}
