using UnityEngine;
using UnityEngine.UI;

public class SellButton : MonoBehaviour
{
    private Button myButton;

    void Awake()
    {
        myButton = GetComponent<Button>();
    }

    public void Sell()
    {
        SoundManager.Instance.ClickSound();
        OverallGameManager.Instance.EndDay();
    }
}
