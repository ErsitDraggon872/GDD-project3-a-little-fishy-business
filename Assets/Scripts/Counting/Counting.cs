using System;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Counting : MonoBehaviour
{
    #region StaticScoring
    private static float Score = 0;
    private static float Multiplier = 1;

    public static void AddScore(float x)
    {
        Score += x;
    }
    public static void MultScore(float x)
    {
        Multiplier += x;
    }

    #endregion

    public ItemGrid itemGrid;
    public TextMeshProUGUI ScoreText;

    public float UpdateScore()
    {
        Count();
        ScoreText.text = "Current Value: " + Score.ToString("F2");
        return Score;
    }

    private void Count(bool reset = true)
    {
        if (reset)
        {
            Score = 0;
        }
        List<Fish> fishObjects = new List<Fish>();
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
                    Fish fishInfo = itemData.GetFishInformation();
                    if (!counted.Contains(itemData.ID))
                    {
                        fishObjects.Add(fishInfo);
                        counted.Add(itemData.ID);
                        if (fishInfo == null){
                            Debug.Log($"Null FishInfo");
                        }
                        var (finalAbility, trackingName) = fishInfo.GetAbilityTracker();
                        if (countingDict.TryGetValue(trackingName, out var entry))
                        {
                            countingDict[trackingName] = (entry.action, entry.count + 1);
                        }
                        else
                        {
                            countingDict[trackingName] = (finalAbility, 1);
                        }
                        fishInfo.OnCount((i, j), countingDict[trackingName].count, itemGrid);
                        //Abilities affecting adjacency should be added in ItemData with a Count() func
                        //Abilities that requires count should be counted via dict, using a static implementation for final effect
                        //Abilities that provide rawScore/multiplier should be added in onCount() func via static addition/mult
                        //Assuming no on-trigger abilities right now
                    }
                }
            }
        }
        counted.Clear();
        for (int i = 0; i < dim.x; i++)
        {
            for (int j = 0; j < dim.y; j++)
            {
                ItemData itemData = itemGrid.GetSlotItemData(i, j);
                if (itemData)
                {
                    if (!counted.Contains(itemData.ID))
                    {
                        counted.Add(itemData.ID);
                        Debug.Log(itemData);
                        Debug.Log(itemData.GetFishInformation().GetData().price);
                        Fish fishInfo = itemData.GetFishInformation();
                        Score += fishInfo.GetData().price;
                    }
                }
            }
        }

        foreach (var kvp in countingDict)
        {
            var (action, count) = kvp.Value;

            action?.Invoke(count);   // null-safe call
        }

        Score *= Multiplier;
        
        if (reset)
        {
            foreach (Fish fish in fishObjects)
            {
                fish.ResetRuntime();
                Destroy(fish);
            }
        }

    }

}