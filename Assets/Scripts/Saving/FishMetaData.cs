using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Tracing;
using System;


public class FishMetaData
{
    public int level;
    public float price;


    public FishMetaData(int level, float price)
    {
        this.level = level;
        this.price = price;
    }

    public FishMetaData(FishMetaData other)
    {
        this.price = other.price;
        this.level = other.level;
    }
}