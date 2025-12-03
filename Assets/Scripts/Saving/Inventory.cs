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
        // Creates inventory instance if does not exist yet
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
        fishLookupByName = new Dictionary<string, Fish>();

        foreach (Fish fish in FishSOs)
        {
            Debug.Log($"SO asset name = {fish.name}, SO FishName = {fish.GetData().FishName}, SO FishType = {fish.GetData().FishType}");
            fishLookupByName[fish.GetData().FishName] = fish;
        }
        Debug.Log(fishLookup);
        LoadInventory();
    }

    private void AddFish(int id, JObject fishMetaData)
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
        return fishLookupByName[name];
    }

    public void AddFishByName(string name)
    {
        currentInventory.Add(new FishEntry(fishLookupByName[name].GetData()));
    }

    public void PopulateInventoryGrid(ItemGrid grid, GameObject itemPrefab)
    {
        Debug.Log("Populating grid with " + currentInventory.Count + " fish");

        foreach (FishEntry entry in currentInventory)
        {

            Debug.Log("Looking up fish: " + entry.fishMetaData.FishName);
            
            //bandaid from ersit cuz idk how to edit the fishmetadata
            if (entry.fishMetaData.FishName == "GoldFish")
            {
                entry.fishMetaData.FishName = "Goldfish";
            }

            // 1. Create inventory item object
            InventoryItem newItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();

            // 2. Create fresh ItemData
            ItemData data = ScriptableObject.CreateInstance<ItemData>();
            var fishSO = fishLookupByName[entry.fishMetaData.FishName];
            data.SetData(fishSO);
            data.width = fishSO.GetData().size.x;
            data.height = fishSO.GetData().size.y;

            newItem.Set(data);

            // 3. Parent temporarily into the grid
            RectTransform rt = newItem.GetComponent<RectTransform>();
            rt.SetParent(grid.transform);
            rt.localScale = Vector3.one;

            if (grid.isPocketGrid)
            {
                InventoryController.ApplyPocket(newItem, grid);
            }
            else
            {
                InventoryController.ApplyMain(newItem, grid);
            }

            // 4. Find a free spot in the grid
            Vector2Int? pos = grid.FindSpaceForObject(newItem);

            if (pos == null)
            {
                Debug.LogWarning("Inventory grid full, cannot place: " + entry.fishMetaData.FishName);
                Destroy(newItem.gameObject);
                continue;
            }

            // 5. Place it
            grid.PlaceItem(newItem, pos.Value.x, pos.Value.y);
        }
    }
}


