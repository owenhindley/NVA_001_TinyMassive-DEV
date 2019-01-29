using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Constellation : MonoBehaviour
{

    public float StarsInTime = 5.0f;
    public float StarsOutTime = 5.0f;
    public List<GameObject> starTransforms = new List<GameObject>();
    public List<Transition> transitionsIn = new List<Transition>();
    public List<Transition> transitionsOut = new List<Transition>();

    [InspectorButton("GetStars")] public bool doGetStars;
    [InspectorButton("TransitionIn")] public bool doTransitionIn;
    [InspectorButton("TransitionOut")] public bool doTransitionOut;
    
    // public ConstellationLine line;
    public ConstellationDottedLine line;

    void Start()
    {
        GetStars();
        // line.lineWidth = 0.0f;
    }

    void GetStars(){

        starTransforms = new List<GameObject>();
        transitionsIn = new List<Transition>();
        transitionsOut = new List<Transition>();
        var stars = GetComponentsInChildren<SpriteRenderer>();
        for (int i=0; i < stars.Length; i++){
            starTransforms.Add(stars[i].gameObject);
            var t = stars[i].gameObject.GetComponents<Transition>();
            for (int j=0; j < t.Length; j++){
                if (t[j] is TransitionIn) {
                    transitionsIn.Add(t[j]);
                } else {
                    transitionsOut.Add(t[j]);
                }
            }
        }
    }

    public void TransitionIn(){
        StartCoroutine(InRoutine());
    }

    IEnumerator InRoutine(){
        for (int i=0; i < transitionsIn.Count; i++){
            transitionsIn[i].DoTransition();
            yield return new WaitForSeconds(StarsInTime / (float)transitionsIn.Count);
        }
        yield return new WaitForSeconds(1.0f);
        line.TransitionIn();
    }

    public void TransitionOut(){
        StartCoroutine(OutRoutine());
    }

    IEnumerator OutRoutine(){
        line.TransitionOut();
        // yield return new WaitForSeconds(1.0f);
        for (int i=0; i < transitionsOut.Count; i++){
            transitionsOut[i].DoTransition();
            yield return new WaitForSeconds(StarsOutTime / (float)transitionsOut.Count);
        }
    }

    void Update()
    {
        
    }
}
