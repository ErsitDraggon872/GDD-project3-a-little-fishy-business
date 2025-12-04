using UnityEngine;
using System.Collections.Generic;

public class InventoryInitializer : MonoBehaviour
{
    public ItemGrid inventoryGrid;
    public GameObject itemPrefab;

    void Start()
    {
        List<InventoryItem> items = Inventory.Instance.CreateInventoryItems(itemPrefab);
        
        List<InventoryItem> pocketItems = new List<InventoryItem>(items);

        // 4. Spread pocket items across pages
        InventoryController.Instance.PopulatePocketAtStart(pocketItems);

        // 5. Set reference for dragging
        InventoryController.Instance.MainGridReference = inventoryGrid;
    }
    
}
