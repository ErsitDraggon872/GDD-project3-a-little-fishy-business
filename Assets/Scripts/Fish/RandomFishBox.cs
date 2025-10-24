using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

public class RandomFishBox : MonoBehaviour
{
    [Tooltip("Holds the ScriptableObjects for every type of fish")]
    [SerializeField] private List<FishData> potentialFishTypes;

    [Tooltip("How many fish are chosen when taking out fish from the box")]
    [SerializeField] private int fishCount;

    [Tooltip("The item to be added to the UI")]
    [SerializeField] GameObject fishPrefab;

    [Tooltip("References the main grid ui element")]
    [SerializeField] private UIDocument gridHolder;
    private VisualElement hotbar;

    private void Start()
    {
        // find the hotbar element from the ui document
        VisualElement root = gridHolder.rootVisualElement;
        hotbar = root.Q<VisualElement>("Hotbar");
    }

    public void displayFish()
    {
        List<FishInstance> currentFishOffers = new List<FishInstance>();

        for (int i = 0; i < fishCount; i++)
        {
            FishInstance fishObj = new FishInstance(potentialFishTypes[Random.Range(0, potentialFishTypes.Count)]);
            Debug.Log(fishObj);
            currentFishOffers.Add(fishObj);

            GameObject instance = Instantiate(fishPrefab);
            DraggableItem instanceScript = instance.GetComponent<DraggableItem>();
            instanceScript.sprite = fishObj.fishData.Icon;

            hotbar[i].Add(instanceScript.visElement);
        }
          
    }



}
