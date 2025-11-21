using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI fishName;
    public TextMeshProUGUI fishPrice;
    public FishShopSpawner fishShopSpawner;
    public GameObject disablePanel;

    public Sprite nullImage;


    private Inventory inventory;
    private Fish fishSO;

    void Start()
    {
        AssignSlot();
        inventory = FindFirstObjectByType<Inventory>();
    }

    public void AssignSlot()
    {
        //readonly here pls
        disablePanel.SetActive(false);
        fishSO = fishShopSpawner.getShopSlot();
        image.sprite = fishSO.Sprite;
        fishName.text = fishSO.GetData().FishName;
        fishPrice.text = fishSO.GetData().cost.ToString();
    }

    public void Buy()
    {
        inventory.AddFishByName(fishSO.GetData().FishName);
        AssignNull();
    }

    public void AssignNull()
    {
        image.sprite = nullImage;
        fishName.text = "";
        fishPrice.text = "";
        disablePanel.SetActive(true);
    }

    //Assign Null & Assign 

}