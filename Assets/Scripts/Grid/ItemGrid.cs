using System;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    public const float tileSizeWidth = 32;
    public const float tileSizeHeight = 32;

    GridItem[,] gridItemSlot;

    RectTransform rectTransform;

    [SerializeField] int gridSizeWidth;
    [SerializeField] int gridSizeHeight;

    [SerializeField] GameObject gridItemPrefab;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);

        GridItem gridItem = Instantiate(gridItemPrefab).GetComponent<GridItem>();
        PlaceItem(gridItem, 1, 1);
    }

    private void Init(int width, int height)
    {
        gridItemSlot = new GridItem[width, height];
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();
    public (int, int) GetGridDimensions()
    {
        return (gridSizeWidth, gridSizeHeight);
    }

    public ItemData GetSlotItemData(int x, int y)
    {
        // Debug.Log(gridItemSlot);
        return gridItemSlot[x, y]?.itemData;
    }

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }

    public void PlaceItem(GridItem gridItem, int posX, int posY)
    {
        RectTransform rectTransform = gridItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);
        for (int x = 0; x < gridItem.itemData.width; x++)
        {
            for (int y = 0; y < gridItem.itemData.height; y++)
            {
                gridItemSlot[posX + x, posY + y] = gridItem;
            }
        }

        gridItem.onGridPositionX = posX;
        gridItem.onGridPositionY = posY;
            

        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth;
        position.y = -(posY * tileSizeHeight);

        rectTransform.localPosition = position;
    }

    public GridItem PickUpItem(int x, int y)
    {
        GridItem toReturn = gridItemSlot[x, y];

        for (int ix = 0; ix < toReturn.itemData.width; ix++)
        {
            for (int iy = 0; iy < toReturn.itemData.height; iy++)
            {
                gridItemSlot[toReturn.onGridPositionX + ix, toReturn.onGridPositionY + iy] = null;
            }
        }

        return toReturn;
    }
    
    public void ClearGrid()
    {
        for (int x = 0; x < gridSizeWidth; x++)
        {
            for (int y = 0; y < gridSizeHeight; y++)
            {
                gridItemSlot[x, y] = null;
            }
        }
    }
}

