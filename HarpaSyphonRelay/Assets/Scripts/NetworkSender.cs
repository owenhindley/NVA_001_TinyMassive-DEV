using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System;
using NetMQ;
using UnityEngine;
using NetMQ.Sockets;

public class NetworkSender : MonoBehaviour
{
    private PairSocket sock;

    public bool ConnectingToPartner = false;

    public byte[] msgBuffer = new byte[0];

    private NetMQ.Msg msg = new NetMQ.Msg();

    public RenderTexture rt;
    public Texture2D sourceTex;
    public Color[] textureColorArray;
    private const int CHANNEL_SIZE_BYTES = sizeof(int);
    private byte[] tempChannelBytes = new byte[CHANNEL_SIZE_BYTES];

    // Start is called before the first frame update
    void Start()
    {
        sock = new PairSocket();
        sourceTex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false, true);
        int byteBufferSize = rt.width * rt.height * 3 * sizeof(int);
        msgBuffer = new byte[byteBufferSize];
    }

    public void ConnectTo(string ip, string port){
        if (ConnectingToPartner){
            sock.Close();
        }

        ConnectingToPartner = true;
        sock.Connect("tcp://" + ip + ":" + port);

    }

   
    void OnDestroy()
    {
        NetMQConfig.Cleanup();
        sock.Close();
    }

    // Update is called once per frame
    void Update()
    {
        // grab byte data from render texture
        RenderTexture.active = rt;
        sourceTex.ReadPixels(new Rect(0,0,rt.width, rt.height), 0,0,false);
        sourceTex.Apply();

        textureColorArray = sourceTex.GetPixels();
        int byteArrayPointer = 0;
        for (int i=0; i < textureColorArray.Length; i++){
            tempChannelBytes = BitConverter.GetBytes((int)textureColorArray[i].r * 255);
            Buffer.BlockCopy(tempChannelBytes, 0, msgBuffer, byteArrayPointer, CHANNEL_SIZE_BYTES);
            byteArrayPointer += CHANNEL_SIZE_BYTES;

            tempChannelBytes = BitConverter.GetBytes((int)textureColorArray[i].g * 255);
            Buffer.BlockCopy(tempChannelBytes, 0, msgBuffer, byteArrayPointer, CHANNEL_SIZE_BYTES);
            byteArrayPointer += CHANNEL_SIZE_BYTES;

            tempChannelBytes = BitConverter.GetBytes((int)textureColorArray[i].b * 255);
            Buffer.BlockCopy(tempChannelBytes, 0, msgBuffer, byteArrayPointer, CHANNEL_SIZE_BYTES);
            byteArrayPointer += CHANNEL_SIZE_BYTES;
        }
    }


    void LateUpdate()
    {
        // transmit texture data
        if (msgBuffer.Length > 0){
            msg.InitGC(msgBuffer, msgBuffer.Length);
            try{
                sock.Send(ref msg, false);
            } catch(Exception e){
                UnityEngine.Debug.Log(e.Message);
            }
        }
    }
}
