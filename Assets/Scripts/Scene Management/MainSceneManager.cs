using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainSceneManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void SceneTransition(int sceneNum)
    {
        StartCoroutine(AsyncSceneTransition(sceneNum));
    }

    IEnumerator AsyncSceneTransition(int s)
    {

        // load next scene asynchronously for smooth transition
        AsyncOperation async = SceneManager.LoadSceneAsync(s);
        // allows control over when the next scene will load by stopping it from loading automatically
        async.allowSceneActivation = false;

        float t = 0;

        while (async.progress < 0.9f || t < 1)
        {
            // minimum fade out time is 1 second
            t += Time.deltaTime;
            
            // does nothing until scene is loaded 
            yield return null;
        }

        // once progress > 90%, allows next scene to load
        async.allowSceneActivation = true;
    }
}
