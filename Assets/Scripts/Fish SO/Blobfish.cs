using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "FishSO/Blobfish")]
public class Blobfish : Fish
{
    public int classNum = 2;
    public override void FinalAbility(int x)
    {
        return;
    }
    public override void OnCount((int x, int y) pos, int z, ItemGrid itemGrid)
    {
        var(gridX, gridY) = itemGrid.GetGridDimensions();

        var places = new (int x, int y)[]
        {
            (0, gridY - 1),
            (gridX - 1, gridY - 1),
            (0, 0),
            (gridX - 1, 0)
        };

        if (places.Contains(pos))
        {
            runtimeData.price *= 1.2f;
        }
    }

    public override (Action<int>, string) GetAbilityTracker()
    {

        return (FinalAbility, "");
    }

}