using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System;
using System.IO;
using NetMQ;
using UnityEngine;
using NetMQ.Sockets;
using UnityEngine.Rendering;
using Unity.Collections;

public class NetworkSender : MonoBehaviour
{

    public byte[] msgBuffer = new byte[0];

    private NetMQ.Msg msg = new NetMQ.Msg();

    public RenderTexture rt;
    public Texture2D sourceTex;
    public NativeArray<Color32> textureColorArray;
    public const int CHANNEL_SIZE_BYTES = 1;
    private byte[] tempChannelBytes = new byte[CHANNEL_SIZE_BYTES];

    public int framesEncoded = 0;
    public int framesSent = 0;
    private bool framePending = false;

    private Thread senderWorker;
    private bool senderCancelled;
    public bool threadRunning = false;
    private string ip = "127.0.0.1";
    private string port = "1337";

    private Queue<AsyncGPUReadbackRequest> requests = new Queue<AsyncGPUReadbackRequest>();

    // Start is called before the first frame update
    void Start()
    {

        ip = TMConfig.Current.receiverIP;
        port = TMConfig.Current.port;

        sourceTex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false, true);
        int byteBufferSize = rt.width * rt.height * 3 * CHANNEL_SIZE_BYTES;
        msgBuffer = new byte[byteBufferSize];

        senderWorker = new Thread(SocketThreadLoop);
        senderWorker.Start();
    }

    void SocketThreadLoop(){

        while(true){

            threadRunning = true;
            senderCancelled = false;
            AsyncIO.ForceDotNet.Force();
            using (var sock = new PairSocket())
            {
                sock.Connect("tcp://" + ip + ":" + port);

                while (!senderCancelled)
                {
                    if (!framePending) continue;
                    sock.SendFrame(msgBuffer);
                    framesSent++;
                    framePending = false;
                }

                sock.Close();
            }
            NetMQConfig.Cleanup();
            threadRunning = false;

        }
    }


    // Update is called once per frame
    void Update()
    {
        // grab byte data from render texture
        // RenderTexture.active = rt;
        // sourceTex.ReadPixels(new Rect(0,0,rt.width, rt.height), 0,0,false);
        // sourceTex.Apply();

        while (requests.Count > 0)
        {
            var req = requests.Peek();

            if (req.hasError)
            {
                UnityEngine.Debug.Log("GPU readback error detected.");
                requests.Dequeue();
            }
            else if (req.done)
            {
                textureColorArray = req.GetData<Color32>();
                requests.Dequeue();

                int byteArrayPointer = 0;
                for (int i=0; i < textureColorArray.Length; i++){
                    msgBuffer[byteArrayPointer++] = textureColorArray[i].r;
                    msgBuffer[byteArrayPointer++] = textureColorArray[i].g;
                    msgBuffer[byteArrayPointer++] = textureColorArray[i].b;
                    // Buffer.BlockCopy(tempChannelBytes, 0, msgBuffer, byteArrayPointer, CHANNEL_SIZE_BYTES);
                    // byteArrayPointer += CHANNEL_SIZE_BYTES;

                    
                    // Buffer.BlockCopy(tempChannelBytes, 0, msgBuffer, byteArrayPointer, CHANNEL_SIZE_BYTES);
                    // byteArrayPointer += CHANNEL_SIZE_BYTES;

                    
                    // Buffer.BlockCopy(tempChannelBytes, 0, msgBuffer, byteArrayPointer, CHANNEL_SIZE_BYTES);
                    // byteArrayPointer += CHANNEL_SIZE_BYTES;

                    // tempChannelBytes = BitConverter.GetBytes(textureColorArray[i].r);
                    // tempChannelBytes = BitConverter.GetBytes((short)UnityEngine.Random.Range(0.0f, 255.0f));
                    // Buffer.BlockCopy(tempChannelBytes, 0, msgBuffer, byteArrayPointer, CHANNEL_SIZE_BYTES);
                    // byteArrayPointer += CHANNEL_SIZE_BYTES;

                    // tempChannelBytes = BitConverter.GetBytes((short)UnityEngine.Random.Range(0.0f, 255.0f));
                    // Buffer.BlockCopy(tempChannelBytes, 0, msgBuffer, byteArrayPointer, CHANNEL_SIZE_BYTES);
                    // byteArrayPointer += CHANNEL_SIZE_BYTES;

                    // tempChannelBytes = BitConverter.GetBytes((short)UnityEngine.Random.Range(0.0f, 255.0f));
                    // Buffer.BlockCopy(tempChannelBytes, 0, msgBuffer, byteArrayPointer, CHANNEL_SIZE_BYTES);
                    // byteArrayPointer += CHANNEL_SIZE_BYTES;
                }

                framesEncoded++;
                framePending = true;

            }
            else
            {
                break;
            }
        }

        requests.Enqueue(AsyncGPUReadback.Request(rt));
        
    }


    
}
