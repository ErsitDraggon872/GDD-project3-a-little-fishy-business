using UnityEngine;
using UnityEngine.UI;


//size in grid, attached to GO
public class InventoryItem : MonoBehaviour
{
    public ItemData itemData;

    public int HEIGHT
    {
        get
        {
            if (rotated == false)
            {
                return itemData.height;
            }
            return itemData.width;
        }
    }

    public int WIDTH
    {
        get
        {
            if (rotated == false)
            {
                return itemData.width;
            }
            return itemData.height;
        }
    }

    public int onGridPositionX;
    public int onGridPositionY;

    public bool rotated = false;

    // assigns given item data to the InventoryItem object
    internal void Set(ItemData itemData)
    {
        this.itemData = itemData;

        // Apply sprite
        Image img = GetComponent<Image>();
        img.sprite = itemData.itemIcon;
        img.preserveAspect = true;

        // Try to find grid
        ItemGrid parentGrid = GetComponentInParent<ItemGrid>();

        // If not inside a grid yet, skip sizing
        if (parentGrid == null)
            return;

        // If inside grid → apply proper size
        Vector2 size = new Vector2(
            WIDTH * parentGrid.tileSizeWidth,
            HEIGHT * parentGrid.tileSizeHeight
        );

        GetComponent<RectTransform>().sizeDelta = size;
    }


    internal void Rotate()
    {
        rotated = !rotated;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, rotated == true ? 90f : 0f);
    }
}
