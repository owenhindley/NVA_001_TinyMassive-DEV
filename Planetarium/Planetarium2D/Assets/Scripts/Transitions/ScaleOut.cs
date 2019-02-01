using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleOut : TransitionOut
{
    
    public override void DoSetup(){
    }
    public override void DoTransition(){
        transform.DOScale(Vector3.zero, 1.0f).SetEase(Ease.InFlash);
    }
}
