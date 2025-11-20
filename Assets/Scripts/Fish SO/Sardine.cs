using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "FishSO/Sardine")]
public class Sardine : Fish
{
    public override void FinalAbility(int x)
    {
        if (x >= 3)
        {
            Counting.MultScore(1.15f);
        }
        
    }
    public override void OnCount((int x, int y) pos, int z, ItemGrid itemGrid)
    {
        
        return;
    }

    public override (Action<int>, string) GetAbilityTracker()
    {

        return (FinalAbility, "Sardine");
    }

}