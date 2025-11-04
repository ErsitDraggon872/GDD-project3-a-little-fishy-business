using System;
using UnityEngine;

public interface Fish
{

    public int GetID();
    public FishMetaData GetData();

    public (Action<int>, string) GetAbilityTracker();
    public void FinalAbility(int x);
    public void OnCount(int x);
    //Instantiate in real world as game object
    public void Load(FishMetaData fishMetaData);
    
}