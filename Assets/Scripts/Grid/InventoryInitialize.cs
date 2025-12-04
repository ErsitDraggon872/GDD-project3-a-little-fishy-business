using UnityEngine;

public class InventoryInitializer : MonoBehaviour
{
    public ItemGrid inventoryGrid;
    public GameObject itemPrefab;

    void Start()
    {
        Inventory.Instance.PopulateInventoryGrid(inventoryGrid, itemPrefab);
        InventoryController.Instance.MainGridReference = inventoryGrid; 
    }
    
}
