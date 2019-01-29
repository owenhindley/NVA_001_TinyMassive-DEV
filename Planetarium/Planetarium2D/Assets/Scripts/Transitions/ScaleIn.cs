using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleIn : TransitionIn
{
    private Vector3 startingScale;
    public override void DoSetup(){
        startingScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }
    public override void DoTransition(){
        transform.DOScale(startingScale, 1.0f).SetEase(Ease.OutElastic);
    }
}
