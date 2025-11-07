using UnityEngine;
using UnityEngine.UI;


//size in grid, attached to GO
public class GridItem : MonoBehaviour
{
    public ItemData itemData;

    public int onGridPositionX;
    public int onGridPositionY;

    internal void Set(ItemData itemDataTemplate)
    {
        this.itemData = ScriptableObject.Instantiate(itemDataTemplate);

        GetComponent<Image>().sprite = itemData.itemIcon;

        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid.tileSizeWidth;
        size.y = itemData.height * ItemGrid.tileSizeHeight;
        GetComponent<RectTransform>().sizeDelta = size;
    }
}
