using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PixelPerfectRTGrabber : MonoBehaviour
{
   public RenderTexture grabToTexture;
   void OnRenderImage(RenderTexture src, RenderTexture dest)
   {
       Graphics.Blit(src, grabToTexture);
       Graphics.Blit(src, dest);
   }

}
