using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "FishSO/Goldfish")]
public class Goldfish : Fish
{
    public int classNum = 0;
    public override void FinalAbility(int x)
    {
        return;
    }
    public override void OnCount((int x, int y) pos, int z, ItemGrid itemGrid)
    {
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(-1, -1),
            new Vector2Int(-1,  1),
            new Vector2Int( 1,  1),
            new Vector2Int( 1, -1)
        };

        List<int> visited = new List<int>{itemGrid.GetSlotItemData(pos.x, pos.y).ID};

        foreach (var dir in directions)
        {
            var slotData = itemGrid.GetSlotItemData(pos.x + dir.x, pos.y + dir.y);

            if (slotData == null || visited.Contains(slotData.ID))
                continue;

            visited.Add(slotData.ID);

            // Deconstruct (Action<int> finalAbility, string trackingName)
            var tracker = slotData.GetFishInformation().GetAbilityTracker();
            var finalAbility = tracker.Item1;
            var trackingName = tracker.Item2;

            // Check ability name
            if (trackingName == "Guppy")
            {
                runtimeData.price *= 2f;    // use 2f for float math
                Debug.Log($"{pos.x}, {pos.y}");
            }
        }
    }

    public override (Action<int>, string) GetAbilityTracker()
    {

        return (FinalAbility, "Goldfish");
    }
}