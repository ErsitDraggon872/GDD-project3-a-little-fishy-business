using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class PocketGridController : MonoBehaviour
{
    public List<ItemGrid> pocketPages;
    public int currentPage = 0;
    [SerializeField] private TextMeshProUGUI pageNumText;

    void Start()
    {
        ShowPage(0);
    }

    public void ShowPage(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= pocketPages.Count)
            return;

        foreach (var pg in pocketPages)
            pg.gameObject.SetActive(false);

        pocketPages[pageIndex].gameObject.SetActive(true);
        currentPage = pageIndex;
        pageNumText.text = $"{currentPage + 1}";
    }

    public void NextPage()
    {
        int next = currentPage;
        do
        {
            next = (next + 1) % pocketPages.Count;

            // Page 0 always valid
            if (next == 0)
                break;

        } while (pocketPages[next].GetItemCount() == 0);
        ShowPage(next);
    }

    public void PreviousPage()
    {
        int prev = currentPage;
        do
        {
            prev--;
            if (prev < 0)
                prev = pocketPages.Count - 1;

            if (prev == 0)
                break;

        } while (pocketPages[prev].GetItemCount() == 0);
        ShowPage(prev);
    }

    public ItemGrid GetActivePage()
    {
        return pocketPages[currentPage];
    }

    public void PopulatePocketPages(List<InventoryItem> items)
    {
        // Clear all pages first (optional depending on load logic)
        foreach (var page in pocketPages)
            page.ClearGrid();

        int pageIndex = 0;
        ItemGrid currentPage = pocketPages[pageIndex];

        foreach (InventoryItem item in items)
        {
            // ⭐ Force logical size BEFORE placement calculations
            item.itemData.width = 1;
            item.itemData.height = 1;
            item.rotated = false;

            // ⭐ Also force visual pocket sizing
            InventoryController.ApplyPocket(item, currentPage);

            // Try placing in current page
            Vector2Int? pos = currentPage.FindSpaceForObject(item);

            if (pos == null)
            {
                // Move to next page
                pageIndex++;
                if (pageIndex >= pocketPages.Count)
                {
                    Debug.LogError("⚠ Pocket pages full!");
                    return;
                }

                currentPage = pocketPages[pageIndex];

                // ⭐ Must force 1×1 AGAIN before rechecking space
                item.itemData.width = 1;
                item.itemData.height = 1;
                item.rotated = false;
                InventoryController.ApplyPocket(item, currentPage);

                pos = currentPage.FindSpaceForObject(item);
            }

            currentPage.PlaceItem(item, pos.Value.x, pos.Value.y);
        }

        for (int i = 0; i < pocketPages.Count; i++)
        {
            if (i == 0)
            {
                // Page 0 always visible
                pocketPages[i].gameObject.SetActive(true);
            }
            else
            {
                // Visible only if it has items
                bool hasItems = pocketPages[i].GetItemCount() > 0;
                pocketPages[i].gameObject.SetActive(hasItems);
            }
        }

        this.currentPage = 0;
        ShowPage(0);
    }

}