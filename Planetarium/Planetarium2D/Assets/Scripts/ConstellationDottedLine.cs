using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ConstellationDottedLine : MonoBehaviour
{

    public GameObject dotPrefab;

    public List<Transform> points;

    public float dotsPerUnit = 10.0f;
    public float dotTimeInterval = 0.01f;

    private List<GameObject> dots = new List<GameObject>();

    [InspectorButton("DrawDots")] public bool doDrawDots;

    void Start()
    {
        

    }

    public void DrawDots(){
        dots.ForEach((GameObject d)=>{
            Destroy(d);
        });
        dots.Clear();

        var pointList = new List<Vector3>();
        points.ForEach((Transform t)=>{
            pointList.Add(t.position);
        });

        StartCoroutine(DrawDotsRoutine(pointList));
    }

    IEnumerator DrawDotsRoutine(List<Vector3> pointList){
        Vector3 start;
        Vector3 end;
        for (int i=0; i < pointList.Count-1; i++){
            start = pointList[i];
            end = pointList[i+1];

            Debug.Log("Distance : " + Vector3.Distance(start, end));

            float numDots = Mathf.Floor(Vector3.Distance(start, end) * dotsPerUnit);
            Debug.Log("Drawing " + numDots + " dots between " + i + " and " + (i+1));
            for (int j=0; j < numDots; j++){
                Vector3 pos = Vector3.Lerp(start, end, (float)j/numDots);
                pos = new Vector3(pos.x, pos.y, transform.position.z);
                var d = GameObject.Instantiate(dotPrefab);
                d.transform.parent = transform;
                d.transform.position = pos;
                Vector3 defaultScale = d.transform.localScale;
                d.transform.localScale = Vector3.zero;
                d.transform.DOScale(defaultScale, 1.0f);
                dots.Add(d);
                yield return new WaitForSeconds(dotTimeInterval);
            }

        }


    }

    public void TransitionIn(){
       DrawDots();
    }

    public void TransitionOut(){
      StartCoroutine(HideDotsRoutine());
    }

    IEnumerator HideDotsRoutine(){
        for (int i=0; i < dots.Count; i++){
            Destroy(dots[i]);
            yield return new WaitForSeconds(dotTimeInterval/2.0f);
        }
        dots.Clear();

    }




}
