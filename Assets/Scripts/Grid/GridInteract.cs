using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GridController gridController;
    ItemGrid itemGrid;
    private void Awake()
    {
        gridController = FindFirstObjectByType(typeof(GridController)) as GridController;
        itemGrid = GetComponent<ItemGrid>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gridController.selectedItemGrid = itemGrid;
        Debug.Log("Pointer enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gridController.selectedItemGrid = null;
        Debug.Log("Pointer exit");
    }


}
