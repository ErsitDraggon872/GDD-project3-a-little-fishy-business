using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class BuyMenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private GameObject failedPurchasePanel;

    public void UpdateMoney(float money)
    {
        moneyText.SetText("balance: $" + money.ToString("F2"));
    }

    public void FailPurchase()
    {
        StartCoroutine(DisplayMessage());
    }

    IEnumerator DisplayMessage()
    {
        failedPurchasePanel.SetActive(true);
        yield return new WaitForSeconds(1);
        failedPurchasePanel.SetActive(false);
    }

}
