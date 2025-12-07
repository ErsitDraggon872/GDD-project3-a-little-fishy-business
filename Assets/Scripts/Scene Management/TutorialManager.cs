using UnityEngine;
using UnityEngine.SceneManagement;
public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialPanels; // Assign panels in inspector
    private int currentPanel = 0;

    void Start()
    {
        foreach (var panel in tutorialPanels)
            panel.SetActive(false);
        if (tutorialPanels.Length > 0)
            tutorialPanels[0].SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SoundManager.Instance.Play("click2");
            NextPanel();
        }
    }

    void NextPanel()
    {
        if (currentPanel < tutorialPanels.Length - 1)
        {
            tutorialPanels[currentPanel].SetActive(false);
            currentPanel++;
            tutorialPanels[currentPanel].SetActive(true);
        }
        else
        {
            MainSceneManager.Instance.SceneTransition(0);
        }
    }
}

