using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public Image image;
    public TextMeshProUGUI fishName;
    public TextMeshProUGUI fishPrice;
    public FishShopSpawner fishShopSpawner;
    public GameObject disablePanel;
    public GameObject descriptionPanel;
    public TextMeshProUGUI descriptionText;

    public Canvas canvas;

    public Sprite nullImage;


    private Inventory inventory;
    private OverallGameManager game;
    private BuyMenuManager buyMenu;
    private Fish fishSO;

    void Start()
    {
        AssignSlot();
        inventory = FindFirstObjectByType<Inventory>();
        game = FindFirstObjectByType<OverallGameManager>();
        buyMenu = FindFirstObjectByType<BuyMenuManager>();
        buyMenu.UpdateMoney(game.GetMoney());
    }

    public void AssignSlot()
    {
        //readonly here pls
        disablePanel.SetActive(false);
        fishSO = fishShopSpawner.getShopSlot();
        image.sprite = fishSO.Sprite;
        fishName.text = fishSO.GetData().FishName;
        fishPrice.text = "$" + fishSO.GetData().cost.ToString();
    }

    public void Buy()
    {
        inventory.AddFishByName(fishSO.GetData().FishName);
        if (fishName.text == "")
        {
            return;
        }
        if (game.GetMoney() - fishSO.GetData().cost >= 0)
        {
            game.SubtractMoney(fishSO.GetData().cost);
            game.AddExpenses(fishSO.GetData().cost);
            game.PlayPurchase();
            buyMenu.UpdateMoney(game.GetMoney());
            AssignNull();
        } 
        else 
        {
            buyMenu.FailPurchase();
        }
    }

    public void AssignNull()
    {
        image.sprite = nullImage;
        fishName.text = "";
        fishPrice.text = "";
        fishSO = null;
        disablePanel.SetActive(true);
        descriptionPanel.SetActive(false);
    }

    //Assign Null & Assign 

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (descriptionPanel == null || disablePanel.activeSelf)
        {
            return;
        }

        descriptionPanel.SetActive(true);

        if (descriptionText != null)
        {
            string text = "Sell Value: $" + fishSO.GetData().price.ToString() + "\n" + fishSO.GetData().description;
            descriptionText.text = text;
        }

        UpdatePanelPosition(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (descriptionPanel == null) return;

        descriptionPanel.SetActive(false);
        if (!this.enabled)
        {
            return;
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!this.enabled || !descriptionPanel.activeSelf)
        {
            return;
        }
        UpdatePanelPosition(eventData);
    }

    private void UpdatePanelPosition(PointerEventData eventData)
    {
        if (descriptionPanel == null || canvas == null) return;

        RectTransform panelRect = descriptionPanel.GetComponent<RectTransform>();
        RectTransform canvasRect = canvas.transform as RectTransform;

        Vector2 size = panelRect.rect.size;
        // float scale = panelRect.rect.scale;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            canvas.worldCamera,
            out localPoint
        );

        if (panelRect.anchoredPosition == localPoint)
        {
            return;
        }

        // Set position with offset
        panelRect.anchoredPosition = localPoint + new Vector2(size.x/2 * panelRect.lossyScale.x, -(size.y/2 * panelRect.lossyScale.y));

        Vector2 clampedPos = panelRect.anchoredPosition;
        Debug.Log(clampedPos.x + " " + canvasRect.rect.xMin + " " + canvasRect.rect.xMax + " " + size.x);
        clampedPos.x = Mathf.Clamp(clampedPos.x, canvasRect.rect.xMin - 113, canvasRect.rect.xMax - 113);
        Debug.Log(clampedPos.x);
        panelRect.anchoredPosition = clampedPos;
        // panelRect.position = eventData.position + new Vector2(size.x, -size.y);

        // OPTIONAL: clamp inside canvas
        // Vector2 clampedPos = panelRect.anchoredPosition;
        // Vector2 min = canvasRect.rect.min + panelRect.rect.size * 0.5f;
        // Vector2 max = canvasRect.rect.max - panelRect.rect.size * 0.5f;

        // clampedPos.x = Mathf.Clamp(clampedPos.x, min.x, max.x);
        // clampedPos.y = Mathf.Clamp(clampedPos.y, min.y, max.y);

        // panelRect.anchoredPosition = clampedPos;
    }


}