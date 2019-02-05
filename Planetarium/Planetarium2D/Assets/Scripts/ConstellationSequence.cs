using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationSequence : MonoBehaviour
{
    public enum Language{
        EN,
        IS
    }

    public Language language;


    public List<Constellation> ConstList;

    public TextScroller textScroller;

    public float startWaitTime = 3.0f;

    public float postTextWaitTime = 1.0f;
    public float preTextWaitTime = 2.0f;
    public float waitBetweenConstellations = 2.0f;

    [InspectorButton("RunSequence")] public bool DoRunSequence;

    void Start(){
        ConstList.ForEach((Constellation c)=>{
            c.gameObject.SetActive(true);
        });

        StartCoroutine(ConstellationSequenceRoutine(startWaitTime));

    }

    public void RunSequence(){

        StartCoroutine(ConstellationSequenceRoutine(0.0f));
    }

    IEnumerator ConstellationSequenceRoutine(float startDelay){

        yield return new WaitForSeconds(startDelay);

        for (int i=0; i < ConstList.Count; i++){

            var c = ConstList[i];

            yield return c.InRoutine();

            yield return new WaitForSeconds(preTextWaitTime);

            textScroller.ShowText(language == Language.EN ? c.titleEN : c.titleIS.ToUpper());

            yield return new WaitForSeconds(c.StarsInTime + postTextWaitTime);
            
            yield return c.OutRoutine();

            yield return new WaitForSeconds(waitBetweenConstellations);
            
        }


    }
}
