using UnityEngine;
using TMPro;

public class SellMenuManager : MonoBehaviour
{ 
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI rentText;

    private void Start()
    {
        FindFirstObjectByType<OverallGameManager>().assignSellMenu();
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
        rentText.SetText($"rent due: ${Mathf.Round(rent)}");
    }
}
