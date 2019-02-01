using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionDiffusion : MonoBehaviour
{
    private Color32 color;

    public int colorIndex = 1;

    public float dA = 0.8f;
    public float dB = 0.25f;
    public float feed = 0.055f;
    public float k = 0.062f;

    public Color32[] arrayCopy;

    void Start()
    {
        color = Main.Instance.availableColors[colorIndex];
        arrayCopy = (Color32[])Main.Instance.colorArray.Clone();
        for (int i=0; i < arrayCopy.Length; i++){
            // arrayCopy[i] = Main.Instance.colorArray[i];
            arrayCopy[i] = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Main.Instance.IsStep){

            for (int x = 1; x < Main.WIDTH-1; x++){
                for (int y = 1; y < Main.HEIGHT-1; y++){
                    var index = x + (y * Main.WIDTH);
                    var val = (dA * Laplace(x, y));
                   
                    arrayCopy[index] = Main.Instance.GetPixel(x, y) + new Color(val, val, val);
                }
            }
            // update pixel if value is greater
            for (int x = 1; x < Main.WIDTH-1; x++){
                for (int y = 1; y < Main.HEIGHT-1; y++){
                    var index = x + (y * Main.WIDTH);
                    var current = Main.Instance.GetPixel(x,y);
                    var newColor = (Color)arrayCopy[index];
                    if (newColor.maxColorComponent > current.maxColorComponent){
                        Main.Instance.SetPixel(x,y, arrayCopy[index]);
                    }
                }
            }

        }
        
    }


    float Laplace(int x, int y){

        /*
        |.05 |.2|.05|
        | .2 |-1| .2|
        |.05 |.2|.05|
        */

        float sum = 0.0f;
        float m = 0.2f;
        float r = 0.05f;

        sum += Main.Instance.GetPixel(x, y).maxColorComponent * -1.0f;
        sum += Main.Instance.GetPixel(x-1, y).maxColorComponent * m;
        sum += Main.Instance.GetPixel(x+1, y).maxColorComponent * m;
        sum += Main.Instance.GetPixel(x, y+1).maxColorComponent * m;
        sum += Main.Instance.GetPixel(x, y-1).maxColorComponent * m;
        sum += Main.Instance.GetPixel(x-1, y-1).maxColorComponent * r;
        sum += Main.Instance.GetPixel(x+1, y-1).maxColorComponent * r;
        sum += Main.Instance.GetPixel(x-1, y+1).maxColorComponent * r;
        sum += Main.Instance.GetPixel(x+1, y+1).maxColorComponent * r;

        return sum;
    }
}
