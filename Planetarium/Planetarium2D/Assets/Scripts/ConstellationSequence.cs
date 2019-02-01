using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationSequence : MonoBehaviour
{
    public List<Constellation> ConstList;

    public float waitTime = 1.0f;

    [InspectorButton("RunSequence")] public bool DoRunSequence;

    void Start(){
        ConstList.ForEach((Constellation c)=>{
            c.gameObject.SetActive(true);
        });
    }

    public void RunSequence(){

        StartCoroutine(ConstellationSequenceRoutine());
    }

    IEnumerator ConstellationSequenceRoutine(){

        for (int i=0; i < ConstList.Count; i++){

            var c = ConstList[i];

            c.TransitionIn();
            yield return new WaitForSeconds(c.StarsInTime + 1.0f + (c.scrollEndPosition.magnitude > 0 ? c.scrollTime * 2.0f : 1.0f) + waitTime);
            c.TransitionOut();
            yield return new WaitForSeconds(c.StarsOutTime + 1.0f + waitTime);
        }


    }
}
