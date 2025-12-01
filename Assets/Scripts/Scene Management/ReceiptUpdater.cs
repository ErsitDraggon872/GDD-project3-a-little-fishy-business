using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ReceiptUpdater : MonoBehaviour
{
    private OverallGameManager gameManager;

    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI revenueNumText;
    [SerializeField] private TextMeshProUGUI expensesNumText;
    [SerializeField] private TextMeshProUGUI rentNumText;
    [SerializeField] private TextMeshProUGUI profitNumText;
    [SerializeField] private TextMeshProUGUI balanceNumText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<OverallGameManager>();
        dayText.text = $"Day {gameManager.GetDay()}";
        revenueNumText.text = $"${gameManager.GetRevenue().ToString("F2")}";
        expensesNumText.text = $"${gameManager.GetExpenses().ToString("F2")}";
        rentNumText.text = $"${gameManager.GetRent().ToString("F2")}";
        float profit = gameManager.GetRevenue() - gameManager.GetExpenses() - gameManager.GetRent();
        profitNumText.text = $"${profit.ToString("F2")}";
        balanceNumText.text = $"${gameManager.GetMoney().ToString("F2")}";

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            gameManager.PlayVictory();

        } else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            gameManager.PlayLoss();
        }
    }

}
