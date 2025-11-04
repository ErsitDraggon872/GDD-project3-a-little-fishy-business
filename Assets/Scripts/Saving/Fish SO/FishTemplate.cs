using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "#Proj_3 - A Little Fishy Business/FishTemplate")]
public class FishTemplate : Fish
{
    protected int ID = 1;
    protected FishMetaData fishMetaData;
    public int GetID()
    {
        return ID;
    }

    public FishMetaData GetData()
    {
        return fishMetaData;
    }

    public void FinalAbility(int x)
    {
        return;
    }
    public void OnCount(int x)
    {
        
        return;
    }

    public (Action<int>, string) GetAbilityTracker()
    {

        return (FinalAbility, "");
    }

    public void Load(FishMetaData fishMetaData)
    {
        //init
        return;
    }
}