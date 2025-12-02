using UnityEngine;
using UnityEngine.UI;

public class RetryButton : MonoBehaviour
{
    public Button myButton;

    void Start()
    {
        myButton = GetComponent<Button>();
        OverallGameManager game = FindAnyObjectByType<OverallGameManager>();
        myButton.onClick.AddListener(game.RestartGame);
        myButton.onClick.AddListener(SoundManager.Instance.ClickSound);
    }
}
