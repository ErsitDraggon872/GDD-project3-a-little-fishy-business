using System;
using UnityEngine;

public abstract class Fish: ScriptableObject
{
    [SerializeField] protected Sprite sprite;
    public Sprite Sprite => sprite;
    [SerializeField] protected FishMetaData baseData;  


    [NonSerialized] protected FishMetaData runtimeData;

    public virtual Fish CreateRuntimeInstance()
    {
        var clone = Instantiate(this);
        clone.runtimeData = new FishMetaData(baseData);
        return clone;
    }

    public FishMetaData GetData() => runtimeData ?? baseData;

    public virtual void ResetRuntime()
    {
        runtimeData = new FishMetaData(baseData);
    }

    abstract public (Action<int>, string) GetAbilityTracker();
    abstract public void FinalAbility(int x);
    abstract public void OnCount((int x, int y) pos, int z, ItemGrid itemGrid);
    //Instantiate in real world as game object
    
}