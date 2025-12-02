using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Tracing;
using System;
using Unity.VisualScripting;
using System.Linq;

[System.Serializable]
public struct SpawnRateEntry
{
    //Needed for inspector view
    public string fishType;
    public float rate;
}

public class FishShopSpawner : MonoBehaviour
{ 
    [SerializeField]
    public List<SpawnRateEntry> SpawnRates;
    
    private List<(float, string)> FishLookup = new();
    private Inventory inventory;

    void Awake()
    {
        float total_rates = SpawnRates.Sum(x => x.rate);
        float cumulative = 0f;
        foreach (SpawnRateEntry entry in SpawnRates)
        {
            cumulative += entry.rate;
            FishLookup.Add((cumulative, entry.fishType));
        }
    }
    void Start()
    {
        inventory = GameObject.FindFirstObjectByType<Inventory>();
    }

    private void ensureInventory()
    {
        if (!inventory)
        {
            inventory = GameObject.FindFirstObjectByType<Inventory>();
        }
    }

    public Fish getShopSlot()
    {
        ensureInventory();
        float val = UnityEngine.Random.Range(0f, 1f);
        foreach ((float rate, string name) in FishLookup)
        {
            if (val<rate)
            {
                return inventory.getFishByName(name);
            }
        }
        return inventory.getFishByName(FishLookup[FishLookup.Count - 1].Item2);
    }
}