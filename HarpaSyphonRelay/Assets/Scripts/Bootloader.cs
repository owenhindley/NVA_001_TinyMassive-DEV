using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootloader : MonoBehaviour
{

    public string CaptureSceneName = "CaptureScene";
    public string ReceiveSceneName = "ReceiveScene";

    // Start is called before the first frame update
    void Start()
    {
        if (TMConfig.Current.appMode == TMConfig.AppMode.Capture){
            StartCoroutine(LoadScene(CaptureSceneName));
        } else {
            StartCoroutine(LoadScene(ReceiveSceneName));
        }
    }

    IEnumerator LoadScene(string sceneName){

        // SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        var sceneLoadingOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // sceneLoadingOp.allowSceneActivation = true;
        yield return sceneLoadingOp;

        Debug.Log("Loaded scene " + sceneName);
        yield return null;
    }

}
