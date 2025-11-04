using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    private static int newID = 1;
    public int ID;
    public int width = 1;
    public int height = 1;


    public Sprite itemIcon;

    [SerializeField]
    private Fish fishTemplate;
    private Fish fishInformationData;
    public Fish GetFishInformation()
    {
        return fishInformationData;
    }

    public void SetID()
    {
        ID = newID;
        newID += 1;
        fishInformationData = ScriptableObject.Instantiate(fishTemplate);
    }
}
