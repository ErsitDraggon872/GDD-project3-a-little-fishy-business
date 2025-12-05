using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "FishSO/Bass")]
public class Bass : Fish
{
    public int classNum = 0;
    public override void FinalAbility(int x)
    {
        return;
    }
    public override void OnCount((int x, int y) pos, int z, ItemGrid itemGrid)
    {
        return;
    }

    public override (Action<int>, string) GetAbilityTracker()
    {

        return (FinalAbility, "Bass");
    }
}