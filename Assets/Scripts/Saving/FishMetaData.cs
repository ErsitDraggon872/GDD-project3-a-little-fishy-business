using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Tracing;
using System;

[System.Serializable]
public class FishMetaData
{
    public int level;
    public float price;
    public float cost;
    public string[] FishType;
    public string FishName;
    [TextArea]
    public string description;

    public Vector2Int size;

    //size convention: x, y
    public FishMetaData(int level, float price, float cost, Vector2Int size, string[] FishType, string FishName, string description)
    {
        this.level = level;
        this.price = price;
        this.cost = cost;
        this.size = size;
        this.FishType = FishType;
        this.FishName = FishName;
        this.description = description;
    }

    public FishMetaData(FishMetaData other)
    {
        this.price = other.price;
        this.level = other.level;
        this.size = other.size;
        this.FishType = other.FishType;
        this.FishName = other.FishName;
        this.cost = other.cost;
        this.description = other.description;
    }
}