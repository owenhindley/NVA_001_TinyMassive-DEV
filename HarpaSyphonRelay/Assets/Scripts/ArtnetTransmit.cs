using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using artnet;
using System.IO;

public class ArtnetTransmit : MonoBehaviour
{
    [System.Serializable]
    public struct FixtureAddress{
        public byte universe;
        public int address;
        public FixtureAddress(byte _uni, int _add){
            universe = _uni;
            address = _add;
        }
    }

    public const int NUM_UNIVERSES = 6;

    [System.Serializable]
    public class ArtNetInterface{
        public string IP;
        public byte universe;
        public ArtnetDmx packet;
        public ConcurrentQueue<byte[]> artnetQueue;
        public IPEndPoint endPoint;
    }

    public ArtNetInterface[] interfaceList;
    
    public bool enableSending = true;

    public bool HasError = false;

    private bool hasInitialised = false;

    private Thread worker;
    public FixtureAddress[] fixtures;

    public int texWidth = 77;
    public int texHeight = 13;

    public int packetsSent = 0;

    public int dummyChannel = 40;
    public int dummyValue = 255;

    public int debugPixelX = 39;
    public int debugPixelY = 12;
    public byte debugPixelFlashBrightness = 255;

    [InspectorButton("DebugSendDummyData")] public bool doSendDummyData;
    [InspectorButton("FlashDebugPixel")] public bool doFlashDebugPixel;

    // Start is called before the first frame update
    void Start()
    {
       

        var patchPath = Path.Combine(Path.GetFullPath(Application.streamingAssetsPath), TMConfig.Current.patchFilename);

        if (File.Exists(patchPath)){
            fixtures = parseCSV(File.ReadAllText(patchPath));
        }

        interfaceList = new ArtNetInterface[NUM_UNIVERSES];
        
        for (int i=0; i < NUM_UNIVERSES; i++){
            interfaceList[i] = new ArtNetInterface();
            interfaceList[i].universe = (byte)(i);
            interfaceList[i].IP = TMConfig.Current.artnet_IP;
            interfaceList[i].packet = new ArtnetDmx((byte)i);
            interfaceList[i].artnetQueue = new ConcurrentQueue<byte[]>();
            interfaceList[i].endPoint = new IPEndPoint(IPAddress.Parse(interfaceList[i].IP), 6454);
        }

        if (interfaceList.Length >= 2){


            worker = new Thread(SocketThreadLoop);
            worker.Start();

        } else {
            Debug.LogError("ERROR : Incorrect number of interfaces");
        }

        hasInitialised = true;
    }

    public FixtureAddress[] parseCSV(string text){
        var rows = text.Split('\n');
        var list = new List<FixtureAddress>();
        for (int i=rows.Length-1; i > -1; i--){
            var cols = rows[i].Split(',');
            for (int j=0; j< cols.Length; j++){
                var patch = cols[j].Split(':');
                var uni = int.Parse(patch[0]);
                var addr = int.Parse(patch[1]);
                list.Add(new FixtureAddress((byte)uni, (int)addr));
            }
        }
        return list.ToArray();
    }

    public void RenderColor32Array(Color32[] arr){
        

        if (!hasInitialised) { return; }

        // reset packets
        // for (int i=0; i < NUM_UNIVERSES; i++){
        //     interfaceList[i].packet = new ArtnetDmx((byte)i);
        // }

        // write array
        for (int i=0; i < arr.Length; i++){
            var f = fixtures[i];

                // adjust by 1 because.. I don't know
            interfaceList[(int)f.universe-1].packet.setChannel((ushort)(f.address-1), arr[i].r);
            interfaceList[(int)f.universe-1].packet.setChannel((ushort)(f.address+1-1), arr[i].g);
            interfaceList[(int)f.universe-1].packet.setChannel((ushort)(f.address+2-1), arr[i].b);
           
        }

        // queue packets
        for (int i=0; i < NUM_UNIVERSES; i++){
            interfaceList[i].artnetQueue.Enqueue(interfaceList[i].packet.toBytes());
        }
        
    }

    void DebugSendDummyData(){

        interfaceList[0].packet.setChannel((ushort)(dummyChannel-1), (byte)dummyValue);
        interfaceList[0].artnetQueue.Enqueue(interfaceList[0].packet.toBytes());

    }

    void FlashDebugPixel(){

        StartCoroutine(DebugPixelFlash());
    }

    IEnumerator DebugPixelFlash(){

        var arr = new Color32[texWidth * texHeight];
        var pixelIndex = (debugPixelY * texWidth) + debugPixelX;

        Debug.Log("Pixel index : " + pixelIndex);
        Debug.Log("Pixel universe : " + fixtures[pixelIndex].universe);
        Debug.Log("Pixel address : " + fixtures[pixelIndex].address);

        bool flip = true;
        for (int i=0; i < 8; i++){
            arr[pixelIndex] = flip ? new Color32(debugPixelFlashBrightness,debugPixelFlashBrightness,debugPixelFlashBrightness,255) : new Color32(0,0,0,255);
            flip = !flip;
            RenderColor32Array(arr);
            yield return new WaitForSeconds(0.01f);
            
        }

    }


    void SocketThreadLoop(){
        while(true){

            // var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            var udpClient = new UdpClient();
            udpClient.EnableBroadcast = true;

            while(enableSending){

                for (int i=0; i < interfaceList.Length; i++){
                    while(interfaceList[i].artnetQueue.Count > 0){
                        byte[] b;
                        if(interfaceList[i].artnetQueue.TryDequeue(out b)){
                            udpClient.Send(b, b.Length, interfaceList[i].IP, 6454);
                            packetsSent++;
                        } else {
                            HasError = true;
                        }
                    }
                }

                
            }

        }
    }
}
