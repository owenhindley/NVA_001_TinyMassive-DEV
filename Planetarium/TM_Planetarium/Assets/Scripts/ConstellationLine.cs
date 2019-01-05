using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ConstellationLine : MonoBehaviour
{

    public LineRenderer line;
    public List<Transform> points;

    [Range(0.0f, 1.0f)]
    public float lineWidth = 1.0f;

    [Range(0.0f, 1.0f)]
    public float alignCoeff = 0.0f;


    void Start()
    {
        var pointList = new List<Vector3>();
        points.ForEach((Transform t)=>{
            pointList.Add(t.localPosition);
        });

        line.positionCount = pointList.Count;
        line.SetPositions(pointList.ToArray());
    }

    // Update is called once per frame
    void Update()
    {
        alignCoeff = Mathf.Pow(Mathf.Abs(Vector3.Dot(transform.forward, Camera.main.transform.forward)), 8.0f);
        line.widthMultiplier = lineWidth * alignCoeff;

        

    }

    public void Flash(){
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine(){
        for (int i=0; i < points.Count; i++){
            points[i].transform.DOPunchScale(Vector3.one * 0.75f, 2.0f, 5, 1.0f);
            yield return new WaitForSeconds(0.2f);
        }

    }
}
