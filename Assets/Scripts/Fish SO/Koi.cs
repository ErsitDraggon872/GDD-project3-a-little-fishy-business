using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "FishSO/Koi")]
public class Koi : Fish
{
    public int classNum = 1;
    public override void FinalAbility(int x)
    {
        return;
    }
    public override void OnCount((int x, int y) pos, int z, ItemGrid itemGrid)
    {
        int r = UnityEngine.Random.Range(1, 101);
        if (r <= 10)
        {
            Counting.MultScore(0.25f);
        }
        return;
    }

    public override (Action<int>, string) GetAbilityTracker()
    {

        return (FinalAbility, "");
    }

}