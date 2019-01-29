using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StarExplosionOut : TransitionOut
{

    public GameObject sourceStar;
    public int numSourceStars;
    public float explosionRadius;

    public float minSourceScale = 1.0f;
    public float maxSourceScale = 3.0f;

    public List<GameObject> sourceStars = new List<GameObject>();


    public override void DoSetup(){
        for (int i=0; i < numSourceStars; i++){
            var s = GameObject.Instantiate(sourceStar, Vector3.zero, Quaternion.identity, transform);            
            s.name = "sourceStar" + (i+1);
            s.transform.localPosition = Vector3.zero;
            s.transform.localScale = Vector3.zero;
            sourceStars.Add(s);
        }
    }
    public override void DoTransition(){
       var r = GetComponent<SpriteRenderer>();
        r.enabled = false;
        
        for (int i=0; i < numSourceStars; i++){      
            sourceStars[i].transform.localScale =  Vector3.one * Random.Range(minSourceScale, maxSourceScale); 
            sourceStars[i].transform.DOScale(Vector3.zero, 1.0f).SetEase(Ease.InQuint);
            sourceStars[i].transform.DOLocalMove(Random.insideUnitCircle * explosionRadius, 1.0f).SetEase(Ease.InQuint);
        }
    }

}
