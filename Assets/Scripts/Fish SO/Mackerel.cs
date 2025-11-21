using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "FishSO/Mackerel")]
public class Mackerel : Fish
{
    public override void FinalAbility(int x)
    {
        return;
    }
    public override void OnCount((int x, int y) pos, int z, ItemGrid itemGrid)
    {
        OverallGameManager overallGameManager = GameObject.FindFirstObjectByType<OverallGameManager>();
        if (overallGameManager.GetDay()%2 == 1)
        {
            runtimeData.price *= 2;
        }
        return;
    }

    public override (Action<int>, string) GetAbilityTracker()
    {

        return (FinalAbility, "");
    }

}