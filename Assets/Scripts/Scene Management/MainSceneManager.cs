using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainSceneManager : MonoBehaviour
{
    #region Variables

    public static MainSceneManager Instance { get; private set; }
    private Image transitionImage;
    private bool transitioning;

    #endregion
    
    #region Initialization
    private void Awake()
    {
        // Creates scenemanager instance if does not exist yet
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        transitionImage = GameObject.FindWithTag("TransitionFade").GetComponent<Image>();
    }

    #endregion

    public void SceneTransition(int sceneNum)
    {
        if (transitioning)
        {
            return;
        }
        transitioning = true;
        StartCoroutine(AsyncSceneTransition(sceneNum));
    }

    IEnumerator AsyncSceneTransition(int s)
    {   
        transitionImage.gameObject.SetActive(true);

        // load next scene asynchronously for smooth transition
        AsyncOperation async = SceneManager.LoadSceneAsync(s);
        // allows control over when the next scene will load by stopping it from loading automatically
        async.allowSceneActivation = false;

        float t = 0;

        while (async.progress < 0.9f || t < 1)
        {
            // minimum fade out time is 1 second
            t += Time.deltaTime;

            // fade out transition
            t = Mathf.Clamp01(t);
            transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, t);

            // does nothing until scene is loaded 
            yield return null;
        }

        // once progress > 90%, allows next scene to load
        async.allowSceneActivation = true;

        while (s != SceneManager.GetActiveScene().buildIndex)
        {
            yield return null;
        }
        Debug.Log("finished loading!");

        // get new transition image
        transitionImage = GameObject.FindWithTag("TransitionFade").GetComponent<Image>();
        transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 1);
        transitionImage.gameObject.SetActive(true);

        while (t > 0)
        {
            // minimum fade in time is 1 second
            t -= Time.deltaTime;

            // fade in transition
            t = Mathf.Clamp01(t);
            transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, t);

            // does nothing until fully transparent
            yield return null;
        }

        transitioning = false;
        transitionImage.gameObject.SetActive(false);
    }

}
