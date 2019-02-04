using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IdleScreensaver : MonoBehaviour
{
    public Material idleMaterial;
    public RenderTexture target;
    public Texture sourceTexture;

    public bool isRendering = false;

    private Tween opacityTween;


    [InspectorButton("ShowIdle")] public bool doShowIdle;
    [InspectorButton("EndIdle")] public bool doEndIdle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShowIdle(){
        if (!isRendering){
            isRendering = true;
            idleMaterial.SetFloat("_MasterOpacity", 0.0f);
            if (opacityTween != null){ opacityTween.Kill(); }
            opacityTween = DOVirtual.Float(0.0f,1.0f, 3.0f, (float val)=>{
                idleMaterial.SetFloat("_MasterOpacity", val);
            });
        }
       


    }

    public void EndIdle(){
        // isRendering = false;
        // idleMaterial.SetFloat("_MasterOpacity", 0.0f);
        if (isRendering){
            if (opacityTween != null){ opacityTween.Kill(); }
            opacityTween = DOVirtual.Float(1.0f,0.0f, 1.0f, (float val)=>{
                idleMaterial.SetFloat("_MasterOpacity", val);
            }).OnComplete(()=>{
                isRendering = false;
            });

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isRendering){
            Graphics.Blit(sourceTexture, target, idleMaterial);
        }
    }
}
