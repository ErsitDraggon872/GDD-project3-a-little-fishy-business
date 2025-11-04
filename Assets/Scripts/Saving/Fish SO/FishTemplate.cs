using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "FishSO/FishTemplate")]
public class FishTemplate : Fish
{
    private static FishMetaData DefaultMetaData = new FishMetaData(3, 4);
    protected FishMetaData fishMetaData = new FishMetaData(DefaultMetaData);
    
    public override FishMetaData GetData()
    {
        return fishMetaData; 
    }

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

        return (FinalAbility, "");
    }

    public override void Load(FishMetaData fishMetaData)
    {
        //init
        Debug.Log("Reset");
        return;
    }
    public override void Reset()
    {
        fishMetaData = new FishMetaData(DefaultMetaData);
    }
}