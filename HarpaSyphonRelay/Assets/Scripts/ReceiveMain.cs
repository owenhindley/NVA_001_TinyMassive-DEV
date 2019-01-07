using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ReceiveMain : MonoBehaviour
{

    public SocketReceiver receiver;

    public int frameRate = 30;
    private int updatedFrameRate = 30;

    private bool receivedNewFrame = false;

    public int framesReceived = 0;

    public bool showUI = true;

    public const int texWidth = 77;
    public const int texHeight = 13;

    public Texture2D receiveTex;
    public Material receiveMaterial;
    public Color32[] receiveColor32Array = new Color32[0];
    public Color[] receiveColorArray = new Color[0];

    public GameObject harpaModel;

    void OnGUI(){
		Vector2 pos = new Vector2(200,70);
		if (showUI){
			
			GUITools.Button(ref pos, "Hide UI", ()=>{
				showUI = false;
			});

            GUITools.Button(ref pos, "Toggle Harpa model", ()=>{

				if (harpaModel != null){
					harpaModel.SetActive(!harpaModel.activeSelf);
				}
				
			});

            GUITools.Label(ref pos, "Frames received : " + framesReceived);
			

			GUITools.Button(ref pos, "Update All", ()=>{
				UpdateSettings();
			});

			if (frameRate == updatedFrameRate){
				GUITools.Label(ref pos, "Framerate : " + updatedFrameRate);
			} else {
				GUITools.Label(ref pos, "Framerate : " + frameRate + " : press update to change to " + updatedFrameRate);
			}
			updatedFrameRate = GUITools.IntSlider(ref pos, updatedFrameRate, 15, 50);

		} else {

			GUITools.Button(ref pos, "Show UI", ()=>{
				showUI = true;
			});

		}

	}

    // Start is called before the first frame update
    void Start()
    {
        frameRate = TMConfig.Current.defaultFrameRate;
        Application.targetFrameRate = frameRate;

        receiver = new SocketReceiver(TMConfig.Current.port);
        receiver.OnBytes += OnBytes;

        receiveColor32Array = new Color32[texWidth * texHeight * 3];
        receiveColorArray = new Color[texWidth * texHeight * 3];
       

        receiveTex = new Texture2D(texWidth, texHeight, TextureFormat.RGB24, false, true);
        receiveTex.filterMode = FilterMode.Point;
        receiveMaterial.SetTexture("_MainTex", receiveTex);

        harpaModel = GameObject.Find("HarpaModel");
    }

    // Update is called once per frame
    void Update()
    {
        if (receivedNewFrame){
            receiveTex.SetPixels(receiveColorArray, 0);
            receiveTex.Apply();
            framesReceived++;
            receivedNewFrame = false;
        }
    }

    void OnBytes(byte[] bytes){

        int colorArrayIndex = 0;
        short tempR;
        short tempG;
        short tempB;
        byte[] tempBytes = new byte[NetworkSender.CHANNEL_SIZE_BYTES];
        int index = 0;
        while(index < bytes.Length){
            receiveColor32Array[colorArrayIndex] = new Color32(bytes[index++], bytes[index++], bytes[index++], 255);
            receiveColorArray[colorArrayIndex] = receiveColor32Array[colorArrayIndex];
            colorArrayIndex++;
        }

        receivedNewFrame = true;

    }

    void UpdateSettings(){
        StopAllCoroutines();
        frameRate = updatedFrameRate;
        Application.targetFrameRate = frameRate;
        framesReceived = 0;
    }


}
