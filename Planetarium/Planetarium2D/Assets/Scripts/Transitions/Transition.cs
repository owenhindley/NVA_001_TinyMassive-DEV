using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Transition : MonoBehaviour
{
    public enum TransitionDirection{
        In,
        Out
    }

    [InspectorButton("DoTransition")] public bool doDoTransition;

    public TransitionDirection direction {
        get { return TransitionDirection.In; }
    }

    void Start(){
        DoSetup();
    }

    public virtual void DoSetup(){

    }


    public virtual void DoTransition(){
    }

}
