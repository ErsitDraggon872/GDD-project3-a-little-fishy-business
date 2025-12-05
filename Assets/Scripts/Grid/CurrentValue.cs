using TMPro;
using UnityEngine;

public class CurrentValue : MonoBehaviour
{
    public TextMeshProUGUI textbox;
    public Counting counting;
    public ItemGrid itemGrid;

    void Update()
    {
        float score = SumItemGridPrices();
        textbox.text = "Current Value: " + score.ToString();
    }

    public float SumItemGridPrices()
    {
        float totalPrice = 0;

        (int rows, int cols) = itemGrid.GetGridDimensions();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                ItemData item = itemGrid.GetSlotItemData(i, j);

                if (item != null)
                {
                    totalPrice += item.GetFishInformation().GetData().price;
                }
            }
        }

        return totalPrice;
    }

}