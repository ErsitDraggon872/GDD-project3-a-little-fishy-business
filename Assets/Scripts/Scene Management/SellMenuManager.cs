using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class SellMenuManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI rentText;
    public Button sellButton;

    private void Start()
    {
        OverallGameManager.Instance.assignSellMenu();
    }

    public void UpdateMoney(float money)
    {
        moneyText.SetText("Money: $" + money.ToString("F2"));
    }

    public void UpdateDay(float day)
    {
        dayText.SetText($"Day: {day}");
    }

    public void UpdateRent(float rent)
    {
        rentText.SetText($"Rent: ${Mathf.Round(rent)}");
    }
}
