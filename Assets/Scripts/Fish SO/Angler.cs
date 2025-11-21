using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "FishSO/Angler")]
public class Angler : Fish
{
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

            if (slotData == null)
            {
                runtimeData.price*1.1f;
                continue;
            }
                
            visited.Add(slotData.ID);
        }
        return;
    }

    public override (Action<int>, string) GetAbilityTracker()
    {

        return (FinalAbility, "");
    }

}