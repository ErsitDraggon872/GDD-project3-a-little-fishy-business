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
        tutorialPanels[currentPanel].SetActive(false);

        currentPanel++;

        if (currentPanel < tutorialPanels.Length)
        {
            tutorialPanels[currentPanel].SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}

