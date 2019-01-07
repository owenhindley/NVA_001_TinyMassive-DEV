using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class GUITools : MonoBehaviour
{
   public static int GUIItemHeight = 25;
   public static int GUIItemSpacing = 10;
   public static int buttonWidth = 150;

   public static void Button(ref Vector2 pos, string buttonText, Action onClick){
      if (GUI.Button(new Rect(pos.x, pos.y, buttonWidth, GUIItemHeight), buttonText)){
         if (onClick != null){
            onClick.Invoke();
         }
      }
      pos += Vector2.up * (GUIItemHeight + GUIItemSpacing);
   }

   public static void Label(ref Vector2 pos, string LabelText){
      GUI.Label(new Rect(pos.x, pos.y, buttonWidth, GUIItemHeight), LabelText);
      pos += Vector2.up * (GUIItemHeight + GUIItemSpacing);
   }

   public static string TextField(ref Vector2 pos, string Value){
      var outValue = GUI.TextField(new Rect(pos.x, pos.y, buttonWidth, GUIItemHeight), Value);
      pos += Vector2.up * (GUIItemHeight + GUIItemSpacing);
      return outValue;
   }

   public static int IntSlider(ref Vector2 pos, int value, int min, int max){
      int outValue = (int)GUI.HorizontalSlider(new Rect(pos.x, pos.y, buttonWidth, GUIItemHeight), value, min, max);
      pos += Vector2.up * (GUIItemHeight + GUIItemSpacing);
      return outValue;
   }
}
