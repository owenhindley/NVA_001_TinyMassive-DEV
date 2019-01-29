using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveIn : TransitionIn
{   
    private Vector3 initialLocation;

    public Vector3 offset;
    public override void DoSetup()
    {
        initialLocation = transform.localPosition;
        transform.localPosition += offset;
    }

    public override void DoTransition(){
        transform.DOLocalMove(initialLocation, 1.0f).SetEase(Ease.InQuad);
    }

}
