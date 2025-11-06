using System.Security.Cryptography;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    private static int newID = 1;
    public int ID;
    public int width;
    public int height;


    public Sprite itemIcon;

    [SerializeField] private Fish fishInformationData;

    public Fish GetFishInformation()
    {
        return fishInformationData;
    }

    public void SetData(Fish fishTemplate)
    {
        SetID();
        fishInformationData = fishTemplate.CreateRuntimeInstance();
        this.width = fishTemplate.GetData().size.x;
        this.height = fishTemplate.GetData().size.y;
        this.itemIcon = fishInformationData.Sprite;
        Debug.Log($"{width}, {height}");
    }

    public void SetID()
    {
        ID = newID;
        newID += 1;
    }
}
