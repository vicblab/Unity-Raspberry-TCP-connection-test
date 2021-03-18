//-----------------------------------------------------------------------------------------------------------
// Script for controlling the simple TCP socket connection to the server needed to control the Raspberry LED 
// it also displays the LED's state
// 
// Victor Blanco Bataller
// 2021/03/18
// Turku University of Applied Sciences
//----------------------------------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.IO;
using System.Net.Sockets;
//using Newtonsoft.Json;
//using System.Threading;




public class Listener : MonoBehaviour
{
    public String host;
    public Int32 port ;

    internal Boolean socket_ready = false;
    internal String input_buffer = "";
    public UnityEngine.UI.Text test;
    TcpClient tcp_socket;
    NetworkStream net_stream;

    StreamWriter socket_writer;
    StreamReader socket_reader;

    private void Start()
    {
        test = GameObject.Find("Test").GetComponent<UnityEngine.UI.Text>();
        //host = GameObject.Find("InputField").GetComponentsInChildren<UnityEngine.UI.Text>()[0].text; ;
        //port = 50000;//Int32.Parse(canvas.parts[1]);

        // we start the connection
        setupSocket();
    }


    void Update()
    {

        // The display is refreshed every time a new message (retrieved by readSocket()) is recieved

        string received_data = readSocket();
        if (received_data != "")
        {
            Debug.Log(" received:" + received_data);
            //text = Encoding.UTF8.GetString(data);
            test.text = received_data;
        }
    }


    // The socket connection is closed if the user exits the app
    void OnApplicationQuit()
    {
        closeSocket();
    }

    // Helper methods for:
    //...setting up the communication
    public void setupSocket()
    {
        
        try
        {

            // we set up the tcp socket client, specifying the ip address and the port, and setting up the writer and reader streams
            tcp_socket = new TcpClient();
            
            IPAddress ipAddress = Dns.GetHostEntry(host).AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 50000);
            tcp_socket.Connect(ipEndPoint);
            
            net_stream = tcp_socket.GetStream();
            socket_writer = new StreamWriter(net_stream);
            socket_reader = new StreamReader(net_stream);

            // now the socket is ready
            socket_ready = true;
        }
        catch (Exception e)
        {
            // Something went wrong
            Debug.Log("Socket error: " + e);
        }
    }

    //... writing to a socket...
    public void writeSocket(string line)
    {

        // if the socket isn't ready do nothing
        if (!socket_ready)
            return;

        // if the socket is ready write the message on the socker writer stream
        line = line + "\r\n";
        socket_writer.Write(line);
        socket_writer.Flush();
    }

    //... reading from a socket...
    public string readSocket()
    {
        if (!socket_ready)
            return "";

        if (net_stream.DataAvailable)
            return socket_reader.ReadLine().ToString();

        return "";
    }

    //... closing a socket...
    public void closeSocket()
    {
        if (!socket_ready)
            return;

        socket_writer.Close();
        socket_reader.Close();
        tcp_socket.Close();
        socket_ready = false;
    }
}