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

    private List<int> adjDeepCheck(int i, int j, int id)
    {
        List<int> differentIds = new List<int>();

        (int rows, int cols) = itemGrid.GetGridDimensions();

        // 4-way directions: up, down, left, right
        int[,] dirs = new int[,]
        {
            { -1,  0 }, // up
            {  1,  0 }, // down
            {  0, -1 }, // left
            {  0,  1 }  // right
        };

        for (int d = 0; d < dirs.GetLength(0); d++)
        {
            int ni = i + dirs[d, 0];
            int nj = j + dirs[d, 1];

            // bounds check
            if (ni < 0 || ni >= rows || nj < 0 || nj >= cols)
                continue;

            int adjId = itemGrid.GetSlotItemData(ni, nj)?.ID ?? -1;

            if (adjId == -1)
            {
                continue;
            }

            // if occupied and different from id
            if (adjId != id)
            {
                if (!differentIds.Contains(adjId))
                    differentIds.Add(adjId);
            }
        }

        return differentIds;
    }

    private void Count(bool reset = true)
    {
        if (reset)
        {
            Score = 0;
        }
        List<Fish> fishObjects = new List<Fish>();
        HashSet<int> counted = new HashSet<int>();
        List<int> fishClasses = new List<int>{0, 0, 0};
        List<int> deepSeaAdj = new List<int>();
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
                    if (fishInfo.classNum == 2)
                    {
                        
                    }
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
                        if (fishInfo.classNum != 2)
                        {
                            fishClasses[fishInfo.classNum] += 1;
                        }
                        else if (fishInfo.classNum == 2)
                        {
                            fishClasses[fishInfo.classNum] += 1;
                            List<int> ids = adjDeepCheck(i, j, itemData.ID);
                            foreach (int id in ids)
                            {
                                if (deepSeaAdj.Contains(id))
                                {
                                    continue;
                                }
                                else
                                {
                                    deepSeaAdj.Add(id);
                                }
                            }
                        }
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

        //final ability
        foreach (var kvp in countingDict)
        {
            var (action, count) = kvp.Value;

            action?.Invoke(count);   // null-safe call
        }

        //class ability
        if (fishClasses[0] >= 3)
        {
            MultScore(0.25f);
        }
        if (fishClasses[0] > 0 || fishClasses[1] > 0 || fishClasses[2] > 0)
        {
            MultScore(0.20f);
        }
        if (fishClasses[2] > 0)
        {
            int deepSeaCount = fishClasses[2];
            int adj = deepSeaAdj.Count;
            if (deepSeaCount > adj)
            {
                MultScore(0.20f*(deepSeaCount-adj));
            }
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