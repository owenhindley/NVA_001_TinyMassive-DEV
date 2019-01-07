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
        public FixtureAddress(byte _uni, byte _add){
            universe = _uni;
            address = _add;
        }
    }

    [System.Serializable]
    public class ArtNetInterface{
        public string IP;
        public int[] universes;
        public ConcurrentQueue<byte[]> artnetQueue;
        public IPEndPoint endPoint;
    }

    public const int NUM_UNIVERSES = 6;

    public ArtnetDmx[] universePackets = new ArtnetDmx[NUM_UNIVERSES];

    public ArtNetInterface[] interfaceList;
    
    public bool enableSending = true;

    public bool HasError = false;

    private Thread worker;
    public FixtureAddress[] fixtures;

    public int texWidth = 77;
    public int texHeight = 13;

    // Start is called before the first frame update
    void Start()
    {
       

        var patchPath = Path.Combine(Path.GetFullPath(Application.streamingAssetsPath), TMConfig.Current.patchFilename);

        if (File.Exists(patchPath)){
            fixtures = parseCSV(File.ReadAllText(patchPath));
        }

        if (interfaceList.Length >= 2){

            interfaceList[0].IP = TMConfig.Current.interfaceA_IP;
            interfaceList[1].IP = TMConfig.Current.interfaceB_IP;

            for (int i=0; i < interfaceList.Length; i++){
                interfaceList[i].artnetQueue = new ConcurrentQueue<byte[]>();
                interfaceList[i].endPoint = new IPEndPoint(IPAddress.Parse(interfaceList[i].IP), 6454);
            }

            for (int i=0; i < NUM_UNIVERSES; i++){
                universePackets[i] = new ArtnetDmx((byte)i);
            }

            worker = new Thread(SocketThreadLoop);
            worker.Start();

        } else {
            Debug.LogError("ERROR : Incorrect number of interfaces");
        }

    }

    public FixtureAddress[] parseCSV(string text){
        var rows = text.Split('\n');
        var list = new List<FixtureAddress>();
        for (int i=0; i < rows.Length; i++){
            var cols = rows[i].Split(',');
            for (int j=0; j< cols.Length; j++){
                var patch = cols[j].Split(':');
                var uni = int.Parse(patch[0]);
                var addr = int.Parse(patch[1]);
                list.Add(new FixtureAddress((byte)uni, (byte)addr));
            }
        }
        return list.ToArray();
    }

    public void RenderColor32Array(Color32[] arr){
        for (int i=0; i < arr.Length; i++){
            var f = fixtures[i];
            universePackets[(int)f.universe].setChannel((ushort)f.address, arr[i].r);
            universePackets[(int)f.universe].setChannel((ushort)(f.address+1), arr[i].g);
            universePackets[(int)f.universe].setChannel((ushort)(f.address+2), arr[i].b);
        }
        
    }


    void SocketThreadLoop(){
        while(true){

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            while(enableSending){

                for (int i=0; i < interfaceList.Length; i++){
                    while(interfaceList[i].artnetQueue.Count > 0){
                        byte[] b;
                        if(interfaceList[i].artnetQueue.TryDequeue(out b)){
                            socket.SendTo(b, interfaceList[i].endPoint);
                        } else {
                            HasError = true;
                        }
                    }
                }

                
            }

        }
    }
}
