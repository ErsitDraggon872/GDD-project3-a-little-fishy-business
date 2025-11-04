using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "FishSO/Perch")]
public class Perch : Fish
{
    private static FishMetaData DefaultMetaData = new FishMetaData(1, 4);
    protected FishMetaData fishMetaData = new FishMetaData(DefaultMetaData);
    

    public override FishMetaData GetData()
    {
        return fishMetaData; 
    }

    public override void FinalAbility(int x)
    {
        if (x == 1)
        {
            fishMetaData.price *= 0.5f;
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