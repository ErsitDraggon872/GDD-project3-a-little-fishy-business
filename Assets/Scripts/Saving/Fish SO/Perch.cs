using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "FishSO/Perch")]
public class Perch : Fish
{
    public override void FinalAbility(int x)
    {
        if (x == 1)
        {
            runtimeData.price *= 0.5f;
        }
    }
    public override void OnCount((int x, int y) pos, int z, ItemGrid itemGrid)
    {
        return;
    }

    public override (Action<int>, string) GetAbilityTracker()
    {

        return (FinalAbility, "Perch");
    }

}