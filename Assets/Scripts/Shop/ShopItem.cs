using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI fishName;
    public TextMeshProUGUI fishPrice;
    public FishShopSpawner fishShopSpawner;

    private Fish fishSO;

    void Start()
    {
        AssignSlot();
    }

    public void AssignSlot()
    {
        //readonly here pls
        fishSO = fishShopSpawner.getShopSlot();
        image.sprite = fishSO.Sprite;
        fishName.text = fishSO.GetData().FishType;
        fishPrice.text = fishSO.GetData().cost.ToString();
    }

    //Assign Null & Assign 

}