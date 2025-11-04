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
}

public class FishEntry
{
    public int ID;

    //Level etc
    public FishMetaData fishMetaData;


    public FishEntry(int ID, JObject JsonData)
    {
        this.ID = ID;
        this.fishMetaData = new FishMetaData((int)JsonData["Level"], (float)JsonData["Price"]);
    }
}

public class Inventory : MonoBehaviour
{
    [Header("All Fish Prefabs (Containing SO)")]
    public List<GameObject> FishPrefabs;


    private Dictionary<int, GameObject> fishLookup;
    private List<FishEntry> currentInventory;

    void Awake()
    {
        // Build lookup for quick access
        fishLookup = new Dictionary<int, GameObject>();

        foreach (GameObject fish in FishPrefabs)
        {
            fishLookup[fish.GetComponent<Fish>().GetID()] = fish;
        }

        LoadInventory();
    }

    private void AddFish(int id, JObject fishMetaData)
    {
        FishEntry newFish = new FishEntry(id, fishMetaData);
        currentInventory.Add(newFish);
    }

    private void DeleteFish(int index)
    {
        currentInventory.RemoveAt(index);
    }

    public void GetFish(int index)
    {
        int id = currentInventory[index].ID;
        FishMetaData fishMetaData = currentInventory[index].fishMetaData;
        GameObject newFish = Instantiate(fishLookup[index], OverallGameManager.FishSpawnLocation, Quaternion.identity);
        newFish.GetComponent<Fish>().Load(fishMetaData);
    }

    public JObject GetInventory()
    {
        //Convert FishMetaData to JObject
        return new JObject();
    }

    public void LoadInventory()
    {
        JObject inventory = OverallGameManager.GetLoadedInventory();
        foreach (JProperty keyValuePair in inventory.Properties())
        {
            int ID = Convert.ToInt32(keyValuePair.Name);
            JObject value = (JObject)keyValuePair.Value;

            AddFish(ID, value);
        }
    }








}


