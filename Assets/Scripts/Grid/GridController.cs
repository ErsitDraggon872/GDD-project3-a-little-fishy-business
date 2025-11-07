using UnityEngine;
using System.Collections.Generic;



//
public class GridController : MonoBehaviour
{
    [HideInInspector]
    public ItemGrid selectedItemGrid;

    GridItem selectedItem;
    RectTransform rectTransform;

    [SerializeField] List<Fish> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;

    private void Update()
    {
        ItemIconDrag();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }

        if (selectedItemGrid == null) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }
    }

    private void CreateRandomItem()
    {
        GridItem gridItem = Instantiate(itemPrefab).GetComponent<GridItem>();
        selectedItem = gridItem;

        rectTransform = gridItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        ItemData itemData = ScriptableObject.CreateInstance<ItemData>();
        itemData.SetData(items[selectedItemID]);
        gridItem.Set(itemData);
    }

    private void LeftMouseButtonPress()
    {
        //Debug.Log(selectedItemGrid.GetTileGridPosition(Input.mousePosition));

        Vector2Int tileGridPosition = selectedItemGrid.GetTileGridPosition(Input.mousePosition);

        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y);
        selectedItem = null;
    }
    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }

}
