using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]

public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    InventoryController inventoryController;
    ItemGrid itemGrid;
    private void Awake()
    {
        inventoryController = Object.FindFirstObjectByType<InventoryController>();
        itemGrid = GetComponent<ItemGrid>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = itemGrid;

        // If dragging an item → resize according to destination grid
        if (inventoryController.IsDragging())
        {
            InventoryItem dragged = inventoryController.GetSelectedItem();

            if (itemGrid.isPocketGrid)
                InventoryController.ApplyPocket(dragged, itemGrid);   // entering pocket = pocket size
            else
                InventoryController.ApplyMain(dragged, itemGrid);     // entering main = main size
        }

        Debug.Log("Pointer enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // If dragging an item, resize depending on the grid we are leaving
        if (inventoryController.IsDragging())
        {
            InventoryItem dragged = inventoryController.GetSelectedItem();

            if (itemGrid.isPocketGrid)
                InventoryController.ApplyMain(dragged, inventoryController.MainGridReference);
            else
                InventoryController.ApplyPocket(dragged, inventoryController.PocketGridReference);
        }

        // Must clear this AFTER resizing
        inventoryController.SelectedItemGrid = null;

        Debug.Log("Pointer exit");
    }


}
