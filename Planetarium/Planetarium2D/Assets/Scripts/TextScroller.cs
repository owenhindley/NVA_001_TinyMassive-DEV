using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextScroller : MonoBehaviour
{

    Vector3 TextStartingLocalPosition;
    public TextMeshPro Text;
    public float scrollTime = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        TextStartingLocalPosition = Text.transform.localPosition;
    }

    public IEnumerator ShowText(string text){
        Text.transform.DOKill();
        Text.transform.localPosition = TextStartingLocalPosition;
        Text.SetText(text);
        Text.transform.DOLocalMove(TextStartingLocalPosition + (Vector3.right * Text.margin.z * 2.0f), scrollTime, false).SetEase(Ease.Linear);
        yield return new WaitForSeconds(scrollTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
