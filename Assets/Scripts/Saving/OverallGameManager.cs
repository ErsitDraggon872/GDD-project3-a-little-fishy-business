using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.IO;
using TMPro;

public class OverallGameManager : MonoBehaviour
{
    public static string path_to_save = "data.json";
    private static float money;
    public static List<float> FirstSevenRent = new List<float>{50, 60, 75, 100, 130, 165, 200};
    private static int currentDay;
    private static float currentRent;

    #region GameReferences

    public Counting counting;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI DayText;
    public TextMeshProUGUI RentText;
    public GameObject DisableGamePanel;
    public TextMeshProUGUI GameOverText;
    public ItemGrid itemGrid;

    #endregion

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
        DontDestroyOnLoad(this.gameObject);
        path_to_save = Path.Combine(Application.streamingAssetsPath, path_to_save);
        if (!loadJson(1))
        {
            EndGame();
        }
    }

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

        UpdateDay();
        UpdateMoney();
        UpdateRent();

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

    private bool PayRent()
    {
        if (money > currentRent)
        {
            money -= currentRent;
            currentRent = CalculateNewRent();
            UpdateRent();
            return true;
        }
        else
        {
            return false;
        }
    }

    private float CalculateNewRent()
    {
        //Change formula
        if (currentDay <= 7)
        {
            return FirstSevenRent[currentDay - 1];
        }
        else
        {
            return currentRent * 2;
        }
    }

    private void UpdateMoney()
    {
        moneyText.SetText($"Money: {money}");
    }

    private void UpdateDay()
    {
        DayText.SetText($"Day: {currentDay}");
    }

    private void UpdateRent()
    {
        RentText.SetText($"Rent: {currentRent}");
    }

    private void EndGame()
    {
        DisableGamePanel.SetActive(true);
        GameOverText.gameObject.SetActive(true);
    }
    
    public int GetDay()
    {
        return currentDay;
    }

    public void EndDay()
    {
        currentDay += 1;
        UpdateDay();

        float newMoney = counting.UpdateScore();
        AddMoney(newMoney);
        if (!PayRent())
        {
            EndGame();
            return;
        }
        UpdateMoney();

        itemGrid.ClearGrid();
    }
    
    public float GetMoney()
    {
        return money;
    }

    public void AddMoney(float moneyToAdd)
    {
        money += moneyToAdd;
    }

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




}

























