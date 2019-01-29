using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StarCollisionIn : Transition
{
    public GameObject sourceStar;
    public int numSourceStars;
    public float startingRadius;

    public float minSourceScale = 1.0f;
    public float maxSourceScale = 3.0f;

    public List<GameObject> sourceStars = new List<GameObject>();


    public override void DoSetup(){
        for (int i=0; i < numSourceStars; i++){
            var s = GameObject.Instantiate(sourceStar, Vector3.zero, Quaternion.identity, transform);
            
            var p = Random.insideUnitCircle * startingRadius;
            s.name = "sourceStar" + (i+1);
            s.transform.localPosition = new Vector3(p.x, p.y, 0.0f);
            s.transform.localScale = Vector3.zero;
            sourceStars.Add(s);
        }

        var r = GetComponent<SpriteRenderer>();
        r.enabled = false;
    }
    public override void DoTransition(){
        StartCoroutine(TransitionRoutine());
    }

    IEnumerator TransitionRoutine(){
        
        for (int i=0; i < numSourceStars; i++){             
            sourceStars[i].transform.DOScale(Vector3.one * Random.Range(minSourceScale, maxSourceScale), 1.0f).SetEase(Ease.InCubic);            
            sourceStars[i].transform.DOLocalMove(Vector3.zero, 2.0f).SetEase(Ease.InQuint);
        }

        yield return new WaitForSeconds(2.0f);
        
        sourceStars[0].transform.DOPunchScale(Vector3.one * maxSourceScale * 2.0f,0.5f);
        
        yield return new WaitForSeconds(0.5f);

        for (int i=0; i < numSourceStars; i++){      
            sourceStars[i].transform.DOScale(Vector3.zero, 0.5f);
        }
        var r = GetComponent<SpriteRenderer>();
        r.enabled = true;
        



    }
}
