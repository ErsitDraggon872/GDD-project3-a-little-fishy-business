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
    public List<SpawnRateEntry> ShallowSpawnRates;
    [SerializeField]
    public List<SpawnRateEntry> MidSpawnRates;
    [SerializeField]
    public List<SpawnRateEntry> DeepSpawnRates;
    
    private List<(float, string)> FishLookup = new();
    private int day;



    private void setSpawnRate(int day)
    {
        if (FishLookup.Count > 0)
        {
            return;
        }
        if (day < 3)
        {
            float total_rates = ShallowSpawnRates.Sum(x => x.rate);
            float cumulative = 0f;
            foreach (SpawnRateEntry entry in ShallowSpawnRates)
            {
                cumulative += entry.rate/total_rates;
                FishLookup.Add((cumulative, entry.fishType));
            }
        }
        else if (day < 5)
        {
            float total_rates = ShallowSpawnRates.Sum(x => x.rate)*0.6f;
            total_rates += MidSpawnRates.Sum(x => x.rate)*0.4f;
            float cumulative = 0f;
            foreach (SpawnRateEntry entry in ShallowSpawnRates)
            {
                cumulative += entry.rate*0.6f/total_rates;
                FishLookup.Add((cumulative, entry.fishType));
            }
            foreach (SpawnRateEntry entry in MidSpawnRates)
            {
                cumulative += entry.rate*0.4f/total_rates;
                FishLookup.Add((cumulative, entry.fishType));
            }
        }
        else
        {
            float total_rates = ShallowSpawnRates.Sum(x => x.rate)*0.45f;
            total_rates += MidSpawnRates.Sum(x => x.rate)*0.35f;
            total_rates += DeepSpawnRates.Sum(x => x.rate)*0.2f;
            float cumulative = 0f;
            foreach (SpawnRateEntry entry in ShallowSpawnRates)
            {
                cumulative += entry.rate*0.45f/total_rates;
                FishLookup.Add((cumulative, entry.fishType));
            }
            foreach (SpawnRateEntry entry in MidSpawnRates)
            {
                cumulative += entry.rate*0.35f/total_rates;
                FishLookup.Add((cumulative, entry.fishType));
            }
            foreach (SpawnRateEntry entry in DeepSpawnRates)
            {
                cumulative += entry.rate*0.2f/total_rates;
                FishLookup.Add((cumulative, entry.fishType));
            }
        }
    }

    public Fish getShopSlot()
    {
        setSpawnRate(OverallGameManager.Instance.GetDay());
        float val = UnityEngine.Random.Range(0f, 1f);
        foreach ((float rate, string name) in FishLookup)
        {
            if (val<rate)
            {
                Fish fish = Inventory.Instance.getFishByName(name);
                if (fish == null)
                {
                    return getShopSlot();
                }
                return fish;
            }
        }
        return Inventory.Instance.getFishByName(FishLookup[0].Item2);
    }
}