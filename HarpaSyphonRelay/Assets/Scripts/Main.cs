using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;
using TMPro;
using Klak;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Main : MonoBehaviour {

	public RenderTexture sourceTexture;
	public RenderTexture targetTexture;

	[Range(0.0f, 1.0f)]
	public float Width = 1.0f;
	[Range(0.0f, 1.0f)]
	public float Height = 1.0f;

	[Range(0,1024)]
	public int yOffset = 0;

	public int frameRate = 30;

	public Material croppedOutputMaterial;
	public Shader cropShader;
	public Klak.Syphon.SyphonClient syphonClient;

	public float debugNSourceTexX = 0.0f;
	public float debugNSourceTexY = 0.0f;

	public bool showUI = true;
	public bool showSyphonSources = false;
	public string[] syphonServerList = new string[0];
	public string[] syphonAppList = new string[0];

	public string renderServerIP = "127.0.0.1";
	public string renderServerPort = "1337";
	
	// Use this for initialization
	IEnumerator Start () {
		Application.targetFrameRate = frameRate;
		croppedOutputMaterial = new Material(cropShader);
		yield return new WaitForSeconds(1.0f);
		UpdateSettings();
	}

	void OnGUI(){
		Vector2 pos = new Vector2(10,70);
		if (showUI){
			
			GUITools.Button(ref pos, "Hide UI", ()=>{
				showUI = false;
			});

			GUITools.Button(ref pos, "Update All", ()=>{
				GetSyphonServerList();
				UpdateSettings();
			});

			if (showSyphonSources){

				GUITools.Button(ref pos, "Hide Syphon Sources", ()=>{
					showSyphonSources = false;
				});

				pos += Vector2.right * 20;
				
				if (syphonServerList.Length == 0){
					GUITools.Label(ref pos, "No Syphon Servers found");
				} else {
					for (int i=0; i < syphonServerList.Length; i++){
						GUITools.Button(ref pos, syphonServerList[i] + "/" + syphonAppList[i], ()=>{
							Debug.Log("Switching Syphon input over to " + syphonAppList[i]);
							syphonClient.appName = syphonAppList[i];
						});
					}
				}
				
				pos -= Vector2.right * 20;

			} else {

				GUITools.Button(ref pos, "Show Syphon Sources", ()=>{
					showSyphonSources = true;
				});
			}

			GUITools.Label(ref pos, "Render server IP");
			renderServerIP = GUITools.TextField(ref pos, renderServerIP);
			renderServerPort = GUITools.TextField(ref pos, renderServerPort);

			

		} else {

			GUITools.Button(ref pos, "Show UI", ()=>{
				showUI = true;
			});

		}

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

	public void GetSyphonServerList(){
		var list = Plugin_CreateServerList();
		var count = Plugin_GetServerListCount(list);

		if (count == 0){
			syphonServerList = new string[0];
			syphonAppList = new string[0];
			Debug.Log("No Syphon Servers Found");
		}
		else{
			syphonServerList = new string[count];
			syphonAppList = new string[count];
			Debug.Log("Found " + count + " syphon servers");
			for (var i = 0; i < count; i++)
			{
				var pName = Plugin_GetNameFromServerList(list, i);
				var pAppName = Plugin_GetAppNameFromServerList(list, i);

				var name = (pName != IntPtr.Zero) ? Marshal.PtrToStringAnsi(pName) : "(no name)";
				var appName = (pAppName != IntPtr.Zero) ? Marshal.PtrToStringAnsi(pAppName) : "(no app name)";

				syphonServerList[i] = name;
				syphonAppList[i] = appName;

				Debug.Log("Found " + name + " app : " + appName);
			}
		}
		
		Plugin_DestroyServerList(list);

	}

	public void UpdateSettings(){

		Debug.Log("updating settings");
		
	}

	#region Native plugin entry points

        [DllImport("KlakSyphon")]
        static extern IntPtr Plugin_CreateServerList();

        [DllImport("KlakSyphon")]
        static extern void Plugin_DestroyServerList(IntPtr list);

        [DllImport("KlakSyphon")]
        static extern int Plugin_GetServerListCount(IntPtr list);

        [DllImport("KlakSyphon")]
        static extern IntPtr Plugin_GetNameFromServerList(IntPtr list, int index);

        [DllImport("KlakSyphon")]
        static extern IntPtr Plugin_GetAppNameFromServerList(IntPtr list, int index);

        #endregion
}
