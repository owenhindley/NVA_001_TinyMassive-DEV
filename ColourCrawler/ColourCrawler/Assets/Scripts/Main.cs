using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Instance { get { return _instance; }}
    private static Main _instance;

    public Texture2D mainTex;
   
    public const int WIDTH = 77;
    public const int HEIGHT = 13;

    public List<Color32> availableColors;

    public Color32[] colorArray = new Color32[WIDTH * HEIGHT];

    public float stepInterval = 0.5f;
    private bool _isStep = false;
    public bool IsStep {
        get { return _isStep; }
    }

    void Awake(){
        _instance = this;

        mainTex = new Texture2D(WIDTH, HEIGHT, TextureFormat.RGBA32, false, true);
        mainTex.filterMode = FilterMode.Point;
        for (int i=0; i < colorArray.Length; i++){
            
            var c = availableColors[Random.Range(0, availableColors.Count)];
            c = c * Color.gray;
            colorArray[i] = c;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
        StartCoroutine(StepRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        mainTex.SetPixels32(colorArray);
        mainTex.Apply();
       
    }

    public void SetPixel(int x, int y, Color32 col){
        var index = x + (y * WIDTH);
        if (index < colorArray.Length){
            colorArray[index] = col;
        } else {
            Debug.LogError("Error, index " + index + " for x:" + x + " y: " + y + " is out of range");
        }        
    }

    public Color GetPixel(int x, int y){
        var index = x + (y * WIDTH);
        return colorArray[index];
    }

    IEnumerator StepRoutine(){
        while(true){
            _isStep = false;
            yield return new WaitForSeconds(stepInterval);
            _isStep = true;
            yield return null;
        }

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
