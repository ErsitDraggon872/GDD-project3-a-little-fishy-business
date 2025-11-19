using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Tracing;
using System;
using Unity.VisualScripting;



public class FishEntry
{

    //Level etc
    public FishMetaData fishMetaData;


    public FishEntry(JObject JsonData)
    {
        JArray sizeArr = (JArray)JsonData["Size"];
        int x = sizeArr[0].Value<int>();
        int y = sizeArr[1].Value<int>();

        Vector2Int size = new Vector2Int(x, y);
        this.fishMetaData = new FishMetaData((int)JsonData["Level"], (float)JsonData["Price"], (float)JsonData["Cost"], size, (string)JsonData["FishType"]);
    }

    public FishEntry(FishMetaData fishMetaData)
    {
        this.fishMetaData = fishMetaData;
    }
}


public class Inventory : MonoBehaviour
{
    [Header("All Fish Prefabs (Containing SO)")]
    public List<Fish> FishSOs;
    public static Inventory Instance;


    private Dictionary<string, Fish> fishLookup;
    private List<FishEntry> currentInventory;

    void Awake()
    { 
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        // Build lookup for quick access
        fishLookup = new Dictionary<string, Fish>();

        foreach (Fish fish in FishSOs)
        {
            fishLookup[fish.GetData().FishType] = fish;
        }
        Debug.Log(fishLookup);
        LoadInventory();
    }

    private void AddFish(int id, JObject fishMetaData)
    {
        FishEntry newFish = new FishEntry(fishMetaData);
        currentInventory.Add(newFish);
    }
    
    private void AddFish(int id, FishMetaData fishMetaData)
    {
        FishEntry newFish = new FishEntry(fishMetaData);
        currentInventory.Add(newFish);
    }

    private void DeleteFish(int index)
    {
        currentInventory.RemoveAt(index);
    }

    // public void GetFish(int index)
    // {
    //     int id = currentInventory[index].ID;
    //     FishMetaData fishMetaData = currentInventory[index].fishMetaData;
    //     GameObject newFish = Instantiate(fishLookup[index], OverallGameManager.FishSpawnLocation, Quaternion.identity);
    //     newFish.GetComponent<Fish>().Load(fishMetaData);
    // }

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

    public Fish getFishByName(string name)
    {
        return fishLookup[name];
    }

    public void AddFishByName(string name)
    {
        currentInventory.Add(new FishEntry(fishLookup[name].GetData()));
    }






}


