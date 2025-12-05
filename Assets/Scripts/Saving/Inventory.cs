using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Tracing;
using System;


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
        string[] types = { "Empty" };

        this.fishMetaData = new FishMetaData(
            (int)JsonData["Level"],
            (float)JsonData["Price"],
            (float)JsonData["Cost"],
            size,
            types,
            (string)JsonData["FishName"],
            (string)JsonData["Description"]
        );

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
    private Dictionary<string, Fish> fishLookupByName;

    private List<FishEntry> currentInventory = new List<FishEntry>();

    void Awake()
    {
        Debug.Log("Inventory Awake() instance = " + GetInstanceID() + 
          "  count=" + currentInventory.Count);

        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate Inventory detected. Destroying: " + gameObject.name);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("Inventory Awake(): instance = " + GetInstanceID());

        // continue initialization
        fishLookup = new Dictionary<string, Fish>();
        fishLookupByName = new Dictionary<string, Fish>();

        foreach (Fish fish in FishSOs)
        {
            fishLookupByName[fish.GetData().FishName] = fish;
        }

        LoadInventory();
    }

    private void AddFish(int id, JObject fishMetaData)
    {
        Debug.Log("AddFish() added: " + fishMetaData["FishName"]);
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
        JObject obj = new JObject();
        int index = 0;

        foreach (var entry in currentInventory)
        {
            JObject fishObj = new JObject();
            fishObj["Level"] = entry.fishMetaData.level;
            fishObj["Price"] = entry.fishMetaData.price;
            fishObj["Cost"] = entry.fishMetaData.cost;

            JArray size = new JArray();
            size.Add(entry.fishMetaData.size.x);
            size.Add(entry.fishMetaData.size.y);
            fishObj["Size"] = size;

            fishObj["FishName"] = entry.fishMetaData.FishName;
            fishObj["Description"] = entry.fishMetaData.description;

            obj[index.ToString()] = fishObj;
            index++;
        }

        return obj;
    }

    public void LoadInventory()
    {
        Debug.Log("LoadInventory() called");
        JObject inventory = OverallGameManager.GetLoadedInventory();

        if (inventory == null)
        {
            Debug.LogWarning("Loaded inventory was null. Starting empty.");
            return;
        }

        foreach (JProperty keyValuePair in inventory.Properties())
        {
            int ID = Convert.ToInt32(keyValuePair.Name);
            JObject value = (JObject)keyValuePair.Value;

            AddFish(ID, value);
        }
    }

    public Fish getFishByName(string name)
    {
        if (fishLookupByName.TryGetValue(name, out Fish fish))
        {
            return fish;   
        }

        return null;
    }

    public void AddFishByName(string name)
    {
        currentInventory.Add(new FishEntry(fishLookupByName[name].GetData()));
    }

    public List<InventoryItem> CreateInventoryItems(GameObject itemPrefab)
    {
        List<InventoryItem> created = new List<InventoryItem>();

        Debug.Log("Creating " + currentInventory.Count + " inventory items...");

        foreach (FishEntry entry in currentInventory)
        {
            Debug.Log("Looking up fish: " + entry.fishMetaData.FishName);

            if (entry.fishMetaData.FishName == "GoldFish")
                entry.fishMetaData.FishName = "Goldfish";

            InventoryItem newItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();

            ItemData data = ScriptableObject.CreateInstance<ItemData>();
            Fish fishSO = fishLookupByName[entry.fishMetaData.FishName];

            data.SetData(fishSO);
            newItem.Set(data);

            created.Add(newItem);
        }

        return created;
    }

    public void RemoveFishByName(string name)
    {
        currentInventory.RemoveAll(f => f.fishMetaData.FishName == name);
    }

}


