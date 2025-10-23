using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "Scriptable Objects/FishData")]
public class FishData : ScriptableObject
{
    [Tooltip("Easily access fish with a number")]
    public int ID;

    [Tooltip("Name of the fish")]
    public string Name;

    [Tooltip("Sprite of the fish")]
    public Sprite Icon;

    [Tooltip("Optional description of the fish")]
    [TextArea]
    public string Description;

    [Tooltip("Cost of the fish when buying it from suppliers")]
    public float BuyCost;

    [Tooltip("Value of the fish when selling it to customers")]
    public float SellValue;

    [Tooltip("Minimum weight of the fish")]
    public float MinWeight;

    [Tooltip("Maximum weight of the fish")]
    public float MaxWeight;

    [Tooltip("Unique ability of the fish during selling period")]
    public FishAbility Ability;
    // FishAbility is the parent class of the actual abilities

}
