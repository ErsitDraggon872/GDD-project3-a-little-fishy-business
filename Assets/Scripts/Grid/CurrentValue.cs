using TMPro;
using UnityEngine;

public class CurrentValue : MonoBehaviour
{
    public TextMeshProUGUI textbox;
    public Counting counting;

    void Update()
    {
        textbox.text = "Current Value: " + counting.UpdateScore().ToString();
    }

}