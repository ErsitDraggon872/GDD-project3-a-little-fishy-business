using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "FishSO/Guppy")]
public class Guppy : Fish
{
    private static FishMetaData DefaultMetaData = new FishMetaData(1, 4);
    protected FishMetaData fishMetaData = new FishMetaData(DefaultMetaData);


    public override FishMetaData GetData()
    {
        return fishMetaData; 
    }

    public override void FinalAbility(int x)
    {
        return;
    }
    public override void OnCount((int x, int y) pos, int z, ItemGrid itemGrid)
    {
        // Directions around the current position (diagonals)
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(-1, -1),
            new Vector2Int(-1,  1),
            new Vector2Int( 1,  1),
            new Vector2Int( 1, -1)
        };

        foreach (var dir in directions)
        {
            var slotData = itemGrid.GetSlotItemData(pos.x + dir.x, pos.y + dir.y);

            if (slotData == null || slotData.ID == itemGrid.GetSlotItemData(pos.x, pos.y).ID)
                continue;
            

            // Deconstruct (Action<int> finalAbility, string trackingName)
            var tracker = slotData.GetFishInformation().GetAbilityTracker();
            var finalAbility = tracker.Item1;
            var trackingName = tracker.Item2;

            // Check ability name
            if (trackingName == "Guppy")
            {
                fishMetaData.price *= 2f;    // use 2f for float math
                Debug.Log($"{pos.x}, {pos.y}");
            }
        }


    }

    public override (Action<int>, string) GetAbilityTracker()
    {

        return (FinalAbility, "Guppy");
    }

    public override void Load(FishMetaData fishMetaData)
    {
        //init
        return;
    }
    
    public override void Reset()
    {
        fishMetaData = new FishMetaData(DefaultMetaData);
    }
}