using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using artnet;
using System.IO;

public class ArtnetTransmitTest : MonoBehaviour
{

    public bool enableSending = true;

    public bool pendingMessage = false;
    byte[] msgBytes = new byte[0];

    public string ip = "2.255.255.255";
    public int port =  6454;

    public ushort channel = 40;
    public byte value = 255;

    public int messageCount = 0;

    private Thread worker;

    private IPEndPoint endPoint;

    [InspectorButton("SendDummyData")] public bool doSendDummyData;

    // Start is called before the first frame update
    void Start()
    {
        endPoint = new IPEndPoint(IPAddress.Broadcast, port);

        worker = new Thread(SocketThreadLoop);
        worker.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SendDummyData(){

        var packet = new ArtnetDmx(0);
        packet.setChannel(channel, value);
        msgBytes = packet.toBytes();
        pendingMessage = true;

    }

    void SocketThreadLoop(){

        while(true){

            // var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var udpClient = new UdpClient();
            udpClient.EnableBroadcast = true;

            while(enableSending){

                if (pendingMessage){
                    // socket.SendTo( msgBytes, endPoint);
                    udpClient.Send(msgBytes, msgBytes.Length, ip, port);
                    messageCount++;
                    pendingMessage = false;
                }


            }

        }


    }
}
