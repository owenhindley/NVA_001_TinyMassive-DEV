using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Transition : MonoBehaviour
{
    [InspectorButton("DoTransition")] public bool doDoTransition;


    void Start(){
        DoSetup();
    }

    public virtual void DoSetup(){

    }


    public virtual void DoTransition(){
    }

}

public class TransitionIn : Transition{

}

public class TransitionOut : Transition{
    
}
