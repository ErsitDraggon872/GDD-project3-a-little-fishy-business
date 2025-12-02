using UnityEngine;
using UnityEngine.UI;

public class NextButton : MonoBehaviour
{
    public Button myButton;

    void Start()
    {
        myButton = GetComponent<Button>();
        OverallGameManager game = FindAnyObjectByType<OverallGameManager>();
        myButton.onClick.AddListener(game.NextDay);
        myButton.onClick.AddListener(SoundManager.Instance.ClickSound);
    }
}
