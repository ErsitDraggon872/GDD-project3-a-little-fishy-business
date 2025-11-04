using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Counting : MonoBehaviour
{
    #region StaticScoring
    private static float Score = 0;

    public static void AddScore(float x)
    {
        Score += x;
    }
    public static void MultScore(float x)
    {
        Score *= x;
    }

    #endregion

    public ItemGrid itemGrid;

    public float Count(bool reset = true)
    {
        if (reset)
        {
            Score = 0;
        }
        HashSet<int> counted = new HashSet<int>();
        (int x, int y) dim = itemGrid.GetGridDimensions();
        Dictionary<string, (Action<int> action, int count)> countingDict = new Dictionary<string, (Action<int> action, int count)> { [""] = (null, 0)};
        for (int i = 0; i < dim.x; i++)
        {
            for (int j = 0; j < dim.y; j++)
            {
                ItemData itemData = itemGrid.GetSlotItemData(i, j);
                if (itemData)
                {
                    Fish fishInfo = itemData.fishInformation;
                    if (!counted.Contains(fishInfo.GetID()))
                    {
                        counted.Add(fishInfo.GetID());
                        var (finalAbility, trackingName) = fishInfo.GetAbilityTracker();
                        if (countingDict.TryGetValue(trackingName, out var entry)){
                            countingDict[trackingName] = (entry.action, entry.count + 1);
                        }
                        fishInfo.OnCount(countingDict[trackingName].count);
                        //Abilities affecting adjacency should be added in ItemData with a Count() func
                        //Abilities that requires count should be counted via dict, using a static implementation for final effect
                        //Abilities that provide rawScore/multiplier should be added in onCount() func via static addition/mult
                        //Assuming no on-trigger abilities right now
                    }
                }
            }
        }
        for (int i = 0; i < dim.x; i++)
        {
            for (int j = 0; j < dim.y; j++)
            {
                ItemData itemData = itemGrid.GetSlotItemData(i, j);
                if (itemData)
                {
                    Fish fishInfo = itemData.fishInformation;
                    Score += fishInfo.GetData().price;
                }
            }
        }
        return Score;
    }

}