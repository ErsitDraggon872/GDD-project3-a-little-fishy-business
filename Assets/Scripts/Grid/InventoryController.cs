using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

//
public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance;
    public ItemGrid MainGridReference;
    public PocketGridController pocketController;

    #region Variables/References
    [HideInInspector]
    private ItemGrid selectedItemGrid;

    public ItemGrid SelectedItemGrid {
        get => selectedItemGrid;
        set {
            selectedItemGrid = value;

            if (value != null)
                pointerInsideGrid = true;
            else
                pointerInsideGrid = false;

            if (value != null)
                inventoryHighlight.SetParent(value);
            else
                inventoryHighlight.Show(false);
        }
    }

    private ItemGrid originalGrid;
    private Vector2 originalItemPosition;
    private int originalX;
    private int originalY;
    private bool pointerInsideGrid = false;


    InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform rectTransform;

    [SerializeField] List<Fish> randomFishes;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI abilityText;

    InventoryHighlight inventoryHighlight;
    #endregion

    private void Awake()
    {
        Instance = this;
        inventoryHighlight = GetComponent<InventoryHighlight>();
    }

    private void Update()
    {
        ItemIconDrag();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (selectedItem == null)
            {
                //CreateRandomItem();
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            //InsertRandomItem();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }

        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);

            if (selectedItem != null && !Input.GetMouseButton(0)) {
                {
                    PlaceDown();
                }
            }

            return;
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            PickUp();
        }

        if (!Input.GetMouseButton(0))
        {
            PlaceDown();
        }
    }

    #region Rotation Call
    private void RotateItem()
    {
        if (selectedItem == null)
        {
            return;
        }

        selectedItem.Rotate();
    }
    #endregion

    #region Item Placement
    public void PopulatePocketAtStart(List<InventoryItem> pocketItems)
    {
        pocketController.PopulatePocketPages(pocketItems);
    }

    private void SnapBack()
    {
        if (rectTransform == null)
            rectTransform = selectedItem.GetComponent<RectTransform>();

        if (originalGrid != null)
        {
            if (originalGrid.isPocketGrid)
                ApplyPocketGridSize(selectedItem, originalGrid);
            else
                ApplyMainGridSize(selectedItem, originalGrid);

            originalGrid.PlaceItem(selectedItem, originalX, originalY);
        }

        rectTransform.localPosition = originalItemPosition;

        selectedItem = null;
    }
    private void InsertItem(InventoryItem itemToInsert)
    {
        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);

        if (posOnGrid == null)
        {
            return;
        }

        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
        if (complete)
        {
            selectedItem = null;
            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
                rectTransform.SetAsLastSibling();
            }
        }
        else
        {
            SnapBack();
        }
    }
    private void PickUp()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
    }

    private void PlaceDown()
    {
        if (!pointerInsideGrid)
        {
            SnapBack();

            return;
        }

        if (selectedItem != null)
        {
            if (selectedItemGrid != originalGrid)
            {
                if (selectedItemGrid.isPocketGrid)
                    ApplyPocketGridSize(selectedItem, selectedItemGrid);
                else
                    ApplyMainGridSize(selectedItem, selectedItemGrid);
            }

            Vector2Int tileGridPosition = GetTileGridPosition();
            PlaceItem(tileGridPosition);
        }
    }

    #endregion

    #region Item Randomization
    public void InsertRandomItem()
    {
        if (selectedItemGrid == null)
        {
            return;
        }

        CreateRandomItem();
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);
    }

    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        rectTransform.SetAsLastSibling();

        // soon to be changed with actual probabilities
        int selectedItemID = Random.Range(0, randomFishes.Count);
        ItemData item = ScriptableObject.CreateInstance<ItemData>();
        item.SetData(randomFishes[selectedItemID]);
        inventoryItem.Set(item);
    }
    #endregion

    #region Item Movement
    // private void LeftMouseButtonPress()
    // {
    //     //Debug.Log(selectedItemGrid.GetTileGridPosition(Input.mousePosition));
    //     Vector2Int tileGridPosition = GetTileGridPosition();

    //     if (selectedItem == null)
    //     {
    //         PickUpItem(tileGridPosition);
    //     }
    //     else
    //     {
    //         PlaceItem(tileGridPosition);
    //     }
    // }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null && !selectedItemGrid.isPocketGrid)
        {
            position.x -= (selectedItem.WIDTH - 1) * selectedItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.HEIGHT - 1) * selectedItemGrid.tileSizeHeight / 2;
        }

        return selectedItemGrid.GetTileGridPosition(position);
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            //DisplayItemDescription(selectedItem);

            rectTransform = selectedItem.GetComponent<RectTransform>();

            originalGrid = selectedItemGrid;

            if (originalGrid.isPocketGrid)
                ApplyPocketGridSize(selectedItem, originalGrid);
            else
                ApplyMainGridSize(selectedItem, originalGrid);

            originalX = selectedItem.onGridPositionX;
            originalY = selectedItem.onGridPositionY;
            originalItemPosition = rectTransform.localPosition;
        }
    }

    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }

        // Resize only if pointer is inside a *different* grid from the one we picked up from
        if (selectedItem != null && selectedItemGrid != null && pointerInsideGrid)
        {
            // Did we actually change grids?
            if (selectedItemGrid != originalGrid)
            {
                if (selectedItemGrid.isPocketGrid)
                    ApplyPocketGridSize(selectedItem, selectedItemGrid);
                else
                    ApplyMainGridSize(selectedItem, selectedItemGrid);
            }
        }
    }

    public void ApplyPocketGridSize(InventoryItem item, ItemGrid pocketGrid)
    {
        item.itemData.width = 1;
        item.itemData.height = 1;
        //item.rotated = false;

        RectTransform rt = item.GetComponent<RectTransform>();
        rt.localScale = Vector3.one;   // IMPORTANT
        rt.sizeDelta = new Vector2(
            pocketGrid.tileSizeWidth,
            pocketGrid.tileSizeHeight
        );

        Image img = item.GetComponent<Image>();
        img.preserveAspect = true;

    }

    public void ApplyMainGridSize(InventoryItem item, ItemGrid mainGrid)
    {
        var fish = item.itemData.GetFishInformation();
        item.itemData.width = fish.GetData().size.x;
        item.itemData.height = fish.GetData().size.y;

        RectTransform rt = item.GetComponent<RectTransform>();
        rt.localScale = Vector3.one;  // IMPORTANT
        rt.sizeDelta = new Vector2(
            item.WIDTH * mainGrid.tileSizeWidth,
            item.HEIGHT * mainGrid.tileSizeHeight
        );

        Image img = item.GetComponent<Image>();
        img.preserveAspect = true;

    }

    public static void ApplyPocket(InventoryItem item, ItemGrid grid)
    {
        Instance.ApplyPocketGridSize(item, grid);
    }

    public static void ApplyMain(InventoryItem item, ItemGrid grid)
    {
        Instance.ApplyMainGridSize(item, grid);
    }


    #endregion

    #region Item Highlighting

    Vector2Int oldPosition;
    InventoryItem itemToHighlight;

    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();

        if (oldPosition == positionOnGrid)
        {
            return;
        }

        oldPosition = positionOnGrid;
        if (selectedItem == null)
        {
            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);
            if (itemToHighlight != null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
                DisplayItemDescription(itemToHighlight);
            }
            else
            {
                inventoryHighlight.Show(false);
            }

        }
        else
        {
            inventoryHighlight.Show(selectedItemGrid.BoundaryCheck(
                positionOnGrid.x,
                positionOnGrid.y,
                selectedItem.WIDTH,
                selectedItem.HEIGHT
                ));

            DisplayItemDescription(selectedItem);
            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }
    #endregion

    #region Description
    public void DisplayItemDescription(InventoryItem item)
    {
        var fishData = item.itemData.GetFishInformation().GetData();
        nameText.text = fishData.FishName;
        priceText.text = "value: $" + fishData.price.ToString();
        abilityText.text = fishData.description;

    }
    #endregion

    public void BuyItem()
    {
        if (selectedItem == null)
        {
            CreateRandomItem();
        }
    }
}