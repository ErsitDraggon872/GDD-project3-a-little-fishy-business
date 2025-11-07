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

    public Vector2Int size;

    //size convention: x, y
    public FishMetaData(int level, float price, Vector2Int size)
    {
        this.level = level;
        this.price = price;
        this.size = size;
    }

    public FishMetaData(FishMetaData other)
    {
        this.price = other.price;
        this.level = other.level;
        this.size = other.size;
    }
}