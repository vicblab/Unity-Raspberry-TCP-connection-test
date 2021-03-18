using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvas : MonoBehaviour
{
    public GameObject TCPListener;
    public Button connect;
    public Text display;
    public InputField input;
    public static string[] parts;
    public bool isoff;
    public bool isconn;
    
    // Start is called before the first frame update
    void Start()
    {
        //GameObject Listener = GameObject.Find("TCPListener");
        isoff = false;
        isconn = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
        //string newtext;
        //newtext = input.text;
        //parts = newtext.Split(null);
        //pressed = true;
        //Instantiate(TCPListener);
        Debug.Log("Button Clicked");
    }
    public void toggle()
    {
        string command = "on";

        if (isoff) command = "off";

        TCPListener.GetComponent<Listener>().writeSocket(command);

        isoff = !isoff;
    }
}
