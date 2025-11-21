using UnityEngine;
using UnityEngine.UI;

public class SellButton : MonoBehaviour
{
    public Button myButton;

    void Start()
    {
        myButton = GetComponent<Button>();
        OverallGameManager game = FindAnyObjectByType<OverallGameManager>();
        myButton.onClick.AddListener(game.EndDay);
        myButton.onClick.AddListener(SoundManager.Instance.ClickSound);
    }
}
