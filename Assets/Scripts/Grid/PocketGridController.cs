using UnityEngine;
using System.Collections.Generic;

public class PocketGridController : MonoBehaviour
{
    public List<ItemGrid> pocketPages;
    public int currentPage = 0;

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
    }

    public void NextPage()
    {
        int next = currentPage + 1;
        if (next >= pocketPages.Count) next = 0;
        ShowPage(next);
    }

    public void PreviousPage()
    {
        int prev = currentPage - 1;
        if (prev < 0) prev = pocketPages.Count - 1;
        ShowPage(prev);
    }

    public ItemGrid GetActivePage()
    {
        return pocketPages[currentPage];
    }
}