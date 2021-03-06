﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    public List<ConstellationDottedLine> lines;

    public string noteSequence = "";
    private string[] notes;

    public Vector3 scrollEndPosition = Vector3.zero;
    
    public float scrollTime = 4.0f;

    public string titleEN = "Constellation";
    public string titleIS = "Constellation";

    void Start()
    {
        GetStars();
        
        notes = noteSequence.Split(' ');

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

    public IEnumerator InRoutine(){
        int noteIndex= -1;
        for (int i=0; i < transitionsIn.Count; i++){
            transitionsIn[i].DoTransition();
            noteIndex = i % notes.Length;
            if (noteIndex >= 0){
                AudioManager.Instance.PlayStarNote(notes[noteIndex]);
            }
            yield return new WaitForSeconds(StarsInTime / (float)transitionsIn.Count);
        }
        yield return new WaitForSeconds(1.0f);
        lines.ForEach((ConstellationDottedLine l)=>{
            l.TransitionIn();
        });

        yield return new WaitForSeconds(1.0f);

        // yield return new WaitForSeconds(1.0f);
        if (scrollEndPosition.sqrMagnitude > 0.0f){
            transform.DOLocalMove(transform.localPosition + scrollEndPosition, scrollTime).SetEase(Ease.InOutFlash).SetLoops(2, LoopType.Yoyo);
            yield return new WaitForSeconds(scrollTime * 2.0f);
        }

        

    }

    public void TransitionOut(){
        StartCoroutine(OutRoutine());
    }

    public IEnumerator OutRoutine(){
        
        // transform.DOBlendableMoveBy(Vector3.right * 77.0f, 2.0f, true).SetEase(Ease.InFlash);
        
        if (Random.value > 0.5){
            transform.DOBlendableScaleBy(Vector3.one * 4.0f, 4.0f).SetEase(Ease.InOutFlash);
        } else {
            transform.DOScale(Vector3.zero * 4.0f, 4.0f).SetEase(Ease.InOutFlash);
        }
        
        
        yield return new WaitForSeconds(1.0f);


        for (int i=0; i < transitionsOut.Count; i++){
            transitionsOut[i].DoTransition();
            // yield return new WaitForSeconds(StarsOutTime / (float)transitionsOut.Count);
        }

        yield return new WaitForSeconds(1.0f);

        lines.ForEach((ConstellationDottedLine l)=>{
            l.TransitionOut();
        });
        
        yield return null;

    }

    void Update()
    {
        
    }
}
