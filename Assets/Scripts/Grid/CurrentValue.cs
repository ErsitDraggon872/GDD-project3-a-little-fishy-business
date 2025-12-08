using System.Collections.Generic;
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
        //textbox.text = "Current Value: " + score.ToString();
        textbox.text = "Base Value: " + score.ToString("F2");
    }

    public float SumItemGridPrices()
    {
        float totalPrice = 0;

        List<int> ids = new List<int>();

        (int rows, int cols) = itemGrid.GetGridDimensions();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                ItemData item = itemGrid.GetSlotItemData(i, j);

                if (item != null)
                {
                    if (ids.Contains(item.ID))
                    {
                        continue;
                    }
                    ids.Add(item.ID);
                    totalPrice += item.GetFishInformation().GetData().price;
                }
            }
        }

        return totalPrice;
    }

}