using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.IO;

public class OverallGameManager : MonoBehaviour
{
    public static string path_to_save = "data.json";
    private static int money;
    private static int currentDay;
    private static int currentRent;

    #region Upgrades
    private static Upgrades upgrades;
    private static JObject loadedUpgrades;
    #endregion

    #region FishInventory
    public static UnityEngine.Vector3 FishSpawnLocation;
    private static JObject loadedInventory;
    private static Inventory inventory;

    #endregion

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    #region Saving/Loading 
    private bool loadJson(int idx)
    {
        string json = File.ReadAllText(path_to_save);
        JObject root = JObject.Parse(json);
        JObject chosen_file = root["Slot_" + idx.ToString()].ToObject<JObject>();

        if (!(bool)chosen_file["Alive"])
        {
            return false;
        }

        money = (int)chosen_file["Money"];
        currentDay = (int)chosen_file["Current_Day"];
        currentRent = (int)chosen_file["Current_Rent"];
        loadedUpgrades = (JObject)chosen_file["Upgrades"];
        loadedInventory = (JObject)chosen_file["Inventory"];

        return true;
    }

    private void saveJson(int idx)
    {
        string json = File.ReadAllText(path_to_save);
        JObject root = JObject.Parse(json);
        JObject new_save = new JObject();
        
        new_save["Money"] = money;
        new_save["Current_Day"] = currentDay;
        new_save["Current_Rent"] = currentRent;
        new_save["Upgrades"] = GetCurrentUpgrades();
        new_save["Inventory"] = GetCurrentInventory();
        root["Save_" + idx.ToString()] = new_save;

        File.WriteAllText(path_to_save, root.ToString());
    }
    #endregion

    private bool PayRent()
    {
        if (money > currentRent)
        {
            money -= currentRent;
            currentRent = CalculateNewRent();
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private int CalculateNewRent()
    {
        //Change formula
        return currentRent * 2;
    }

    public int GetDay()
    {
        return currentDay;
    }

    public void EndDay()
    {
        currentDay += 1;
        //count money
        //AddMoney
    }
    
    public int GetMoney()
    {
        return money;
    }

    public void AddMoney(int moneyToAdd)
    {
        money += moneyToAdd;
    }

    #region JSON Getters

    private JObject GetCurrentInventory()
    {
        inventory.GetInventory();
        return new JObject();
    }

    public static JObject GetLoadedInventory()
    {
        return loadedInventory;
    }

    private JObject GetCurrentUpgrades()
    {
        return upgrades.GetUpgrades();
    }

    public static JObject GetLoadedUpgrades()
    {
        return loadedUpgrades;
    }
    #endregion

}

























