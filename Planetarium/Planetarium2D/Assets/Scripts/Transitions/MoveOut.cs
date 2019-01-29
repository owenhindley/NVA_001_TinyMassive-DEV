using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveOut : TransitionOut
{   

    public Vector3 offset;
    public override void DoSetup()
    {       
    }

    public override void DoTransition(){
        transform.DOLocalMove(transform.localPosition + offset, 1.0f).SetEase(Ease.InQuad);
    }

}
