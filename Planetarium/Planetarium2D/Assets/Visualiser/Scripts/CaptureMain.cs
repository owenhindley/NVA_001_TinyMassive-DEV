using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;
using TMPro;
using System.Runtime.InteropServices;
using System;

[ExecuteInEditMode]
public class CaptureMain : MonoBehaviour {

	public RenderTexture sourceTexture;
	public RenderTexture targetTexture;

	[Range(0.0f, 1.0f)]
	public float Width = 1.0f;
	[Range(0.0f, 1.0f)]
	public float Height = 1.0f;

	[Range(0,1024)]
	public int yOffset = 0;

	public int frameRate = 30;
	private int updatedFrameRate = 30;

	public bool networkSendEnabled;

	public Material croppedOutputMaterial;
	public Shader cropShader;

	public float debugNSourceTexX = 0.0f;
	public float debugNSourceTexY = 0.0f;

	public bool showUI = true;
	public bool showSyphonSources = false;
	public string[] syphonServerList = new string[0];
	public string[] syphonAppList = new string[0];

	public string renderServerIP = "127.0.0.1";
	public string renderServerPort = "1337";

	public GameObject harpaModel;
	
	// Use this for initialization
	void Start () {
		harpaModel = GameObject.Find("HarpaModel");
	
		croppedOutputMaterial = new Material(cropShader);
	
	}

	void LateUpdate () {

		croppedOutputMaterial.SetFloat("nSourceTexX", 0.0f);
		debugNSourceTexY = ((float)yOffset / sourceTexture.height);
		croppedOutputMaterial.SetFloat("nSourceTexY", debugNSourceTexY);

		
		croppedOutputMaterial.SetFloat("nSourceTexWidth", Width);
		croppedOutputMaterial.SetFloat("nSourceTexHeight", Height);


		Graphics.Blit(sourceTexture, targetTexture, croppedOutputMaterial);
		// Graphics.CopyTexture(sourceTexture, 0, 0, 0, sourceTexture.height - (Height + yOffset), Width, Height, targetTexture, 0, 0, 0, targetTexture.height-Height);

	}


}
