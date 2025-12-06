using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.IO;
using TMPro;

public class OverallGameManager : MonoBehaviour
{
    public static OverallGameManager Instance;
    #region Variables
    public static string path_to_save = "data.json";
    private static float money;
    public static List<float> FirstSevenRent = new List<float>{20, 30, 45, 60, 75, 100, 150};
        //new List<float>{50, 60, 75, 100, 130, 165, 200};
    private static int currentDay;
    private static float currentRent;
    private float currentRevenue;
    private float currentExpenses;
    private bool transitioning;
    #endregion

    #region GameReferences

    private Counting counting;
    private ItemGrid itemGrid;
    private SellMenuManager sellMenu;

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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        path_to_save = Path.Combine(Application.streamingAssetsPath, path_to_save);
        if (!loadJson(1))
        {
            Application.Quit();
        }
        RestartGame();  
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
        if (currentDay == 1)
        {
            money = 100f;
        }

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

    #region Rent, Money, and Day Managers
    private bool PayRent()
    {
        if (money > currentRent)
        {
            money -= currentRent;
            sellMenu.UpdateRent(currentRent);
            return true;
        }
        else
        {
            money -= currentRent;
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

    public int GetDay()
    {
        return currentDay;
    }

    public void NextDay()
    {
        currentDay += 1;
        currentRent = CalculateNewRent();
        currentExpenses = 0;
        transitioning = false;
        MainSceneManager.Instance.SceneTransition(1);
    }

    public void EndDay()
    {
        if (transitioning) return;

        transitioning = true;
        currentRevenue = counting.UpdateScore();
        AddMoney(currentRevenue);

        if (!PayRent())
        {
            MainSceneManager.Instance.SceneTransition(4);
            return;
        }

        RemoveUsedFishesFromInventory();
        itemGrid.ClearGrid();
        counting.UpdateScore();

        if (inventory != null)
        {
            Destroy(inventory.gameObject);
        }

        Debug.Log("transition!");
        MainSceneManager.Instance.SceneTransition(3);
    }
    
    public float GetMoney()
    {
        return money;
    }
    
    public float GetRevenue()
    {
        return currentRevenue;
    }
    
    public float GetRent()
    {
        return currentRent;
    }

    public void AddMoney(float moneyToAdd)
    {
        money += moneyToAdd;
    }
    public void SubtractMoney(float moneyToSubtract)
    {
        money -= moneyToSubtract;
    }

    public void AddExpenses(float expensesToAdd)
    {
        currentExpenses += expensesToAdd;
    }

    public float GetExpenses()
    {
        return currentExpenses;
    }
    #endregion

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

    #region Sell Menu Assigning
    public void assignSellMenu()
    {
        Debug.Log("assigned sell menu");
        sellMenu = FindFirstObjectByType<SellMenuManager>();
        counting = FindFirstObjectByType<Counting>();
        itemGrid = GameObject.FindWithTag("SellGrid").GetComponent<ItemGrid>();
        sellMenu.UpdateDay(currentDay);
        sellMenu.UpdateMoney(money);
        sellMenu.UpdateRent(currentRent);
    }
    #endregion

    #region Sounds

    public void ClickSound()
    {
        SoundManager.Instance.Play("click2");
    }
    public void PlayVictory()
    {
        SoundManager.Instance.Play("victory");
    }
    public void PlayLoss()
    {
        SoundManager.Instance.Play("loss");
    }
    public void PlayPurchase()
    {
        SoundManager.Instance.Play("money");
    }
    #endregion

    #region End Game
    public void RestartGame()
    {
        currentDay = 1;
        money = 100;
        currentExpenses = 0;
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            MainSceneManager.Instance.SceneTransition(0);
        }
    }

    #endregion

    private void RemoveUsedFishesFromInventory()
    {
        // This grid holds the fish sold that day
        InventoryItem[,] grid = itemGrid.GetSlotGrid(); // you will need to add accessor

        HashSet<string> removed = new HashSet<string>();

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                InventoryItem item = grid[x, y];
                if (item != null)
                {
                    string fishName = item.itemData.GetFishInformation().GetData().FishName;

                    if (!removed.Contains(fishName))
                    {
                        Inventory.Instance.RemoveFishByName(fishName);
                        removed.Add(fishName);
                    }
                }
            }
        }
    }

}

























