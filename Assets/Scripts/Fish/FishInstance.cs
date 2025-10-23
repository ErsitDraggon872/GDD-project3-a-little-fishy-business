using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishInstance
{
    public FishData fishData;
    public float weight;

    // Constructor for the FishInstance, passing in one of the FishData scriptable objects
    public FishInstance(FishData data) 
    {
        fishData = data;
        // sets weight to a random number between min and max, rounded to 2 decimal places
        weight = Mathf.Round(Random.Range(fishData.MinWeight, fishData.MaxWeight) * 100f) / 100f;
    }

    // Override ToString() so we can print fish instances in a readable format
    public override string ToString()
    {
        return $"{fishData.Name}: " +
            $"\nWeight: {weight} " +
            $"\nBuy Cost: {fishData.BuyCost} " +
            $"\nSell Value: {fishData.SellValue} " +
            $"\nAbility: {fishData.Ability}";
    }
}
