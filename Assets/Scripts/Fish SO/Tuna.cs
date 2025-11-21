using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "FishSO/Tuna")]
public class Tuna : Fish
{
    public override void FinalAbility(int x)
    {
        for (int i = 0; i < x; i++)
        {
            Counting.MultScore(1.05f);
        }
    }
    public override void OnCount((int x, int y) pos, int z, ItemGrid itemGrid)
    {
        
        return;
    }

    public override (Action<int>, string) GetAbilityTracker()
    {

        return (FinalAbility, "");
    }

}