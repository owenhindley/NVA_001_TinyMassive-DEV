using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Instance { get { return _instance; }}
    private static Main _instance;

    public Texture2D mainTex;
   
    const int RenderWidth = 77;
    const int RenderHeight = 13;

    public Color32[] colorArray = new Color32[RenderWidth * RenderHeight];

    void Awake(){
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainTex = new Texture2D(RenderWidth, RenderHeight, TextureFormat.RGBA32, false, true);
        for (int i=0; i < colorArray.Length; i++){
            colorArray[i] = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        mainTex.SetPixels32(colorArray);
        mainTex.Apply();
       
    }

    /// <summary>
    /// OnRenderImage is called after all rendering is complete to render image.
    /// </summary>
    /// <param name="src">The source RenderTexture.</param>
    /// <param name="dest">The destination RenderTexture.</param>
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(mainTex, dest);
    }
   
    
}
