using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomFishBox : MonoBehaviour
{
    [Tooltip("Holds the ScriptableObjects for every type of fish")]
    [SerializeField] private List<FishData> potentialFishTypes;

    [Tooltip("How many fish are chosen when taking out fish from the box")]
    [SerializeField] private int fishCount;

    public void displayFish()
    {
        List<FishInstance> currentFishOffers = new List<FishInstance>();

        for (int i = 0; i < fishCount; i++)
        {
            FishInstance fishObj = new FishInstance(potentialFishTypes[Random.Range(0, potentialFishTypes.Count)]);
            Debug.Log(fishObj);
            currentFishOffers.Add(fishObj);
        }
          
    }

}
