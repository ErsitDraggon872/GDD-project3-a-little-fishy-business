using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    public Button myButton;
    public Button tutorialButton;

    void Start()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(() => MainSceneManager.Instance.SceneTransition(1));
        myButton.onClick.AddListener(SoundManager.Instance.ClickSound);
        tutorialButton.onClick.AddListener(() => MainSceneManager.Instance.SceneTransition(5));
        tutorialButton.onClick.AddListener(SoundManager.Instance.ClickSound);
    }
}
