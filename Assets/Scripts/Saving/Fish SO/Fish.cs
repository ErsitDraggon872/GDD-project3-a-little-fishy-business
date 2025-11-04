using System;
using UnityEngine;

public abstract class Fish: ScriptableObject
{

    abstract public FishMetaData GetData();

    abstract public (Action<int>, string) GetAbilityTracker();
    abstract public void FinalAbility(int x);
    abstract public void OnCount((int x, int y) pos, int z, ItemGrid itemGrid);
    //Instantiate in real world as game object
    abstract public void Load(FishMetaData fishMetaData);
    abstract public void Reset();
    
}