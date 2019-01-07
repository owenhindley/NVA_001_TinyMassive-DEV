using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Net;
using artnet;

public class ArtnetTransmit : MonoBehaviour
{
    public List<ArtnetDmx> universePackets = new List<ArtnetDmx>();

    public int numUniverses = 6;

    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i < numUniverses; i++){
            universePackets.Add(new ArtnetDmx((byte)i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SocketThreadLoop(){

    }
}
