using System.Diagnostics;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

public class SocketReceiver
{
    private Thread _listenerWorker;

    private bool _listenerCancelled;

    public System.Action<byte[]> OnBytes;

    private readonly Stopwatch _contactWatch;

    private const long ContactThreshold = 1000;

    public bool Connected;

    public string port = "1337";

    private bool _threadRunning = false;

    private void ListenerWork()
    {
        while(true){

            _threadRunning = true;
            _listenerCancelled = false;
            AsyncIO.ForceDotNet.Force();
            using (var server = new PairSocket())
            {
                server.Bind("tcp://*:" + port);

                while (!_listenerCancelled)
                {
                    Connected = _contactWatch.ElapsedMilliseconds < ContactThreshold;
                    byte[] frameBytes;
                    if (!server.TryReceiveFrameBytes(out frameBytes)) continue;
                    _contactWatch.Restart();
                    if (OnBytes != null && frameBytes != null){
                        OnBytes.Invoke(frameBytes);
                    }
                }

                server.Close();
            }
            NetMQConfig.Cleanup();
            _threadRunning = false;


        }
        
    }

    public SocketReceiver(string _port)
    {
        port = _port;
        _contactWatch = new Stopwatch();
        _contactWatch.Start();
        _listenerWorker = new Thread(ListenerWork);
        _listenerCancelled = false;
        _listenerWorker.Start();
    }

}
