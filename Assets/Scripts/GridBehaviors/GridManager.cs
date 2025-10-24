using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [SerializeField] private UIDocument uiDoc;
    [SerializeField] private VisualTreeAsset slotTemplate;
    [SerializeField] private int gridSize;

    public static List<SlotArea> gridSlots = new List<SlotArea>();

    private VisualElement grid;

    private void Start()
    { 
        // find the grid element from the ui document
        VisualElement root = uiDoc.rootVisualElement;
        grid = root.Q<VisualElement>("Grid");

        if (grid != null)
        {
            GenerateGrid(gridSize);
        }
    }

    private void OnEnable()
    {
        GenerateGrid(gridSize);
    }

    public void GenerateGrid(int size)
    {
        grid.Clear();
        gridSlots.Clear();

        for (int i = 0; i < size * size; i++)
        {
            SlotArea slot = new SlotArea();

            // changes size of boxes depending on grid size!
            // slot.style.width = 512f / size;
            // slot.style.height = 512f / size;
            slot.style.width = new Length(100f / size, LengthUnit.Percent);
            slot.style.height = new Length(100f / size, LengthUnit.Percent);

            gridSlots.Add(slot);    

            grid.Add(slot);
        }

        Debug.Log($"{size*size} slots added!");

    }

}
