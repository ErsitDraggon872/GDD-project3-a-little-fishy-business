using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    [SerializeField] private UIDocument uiDoc;
    [SerializeField] private VisualTreeAsset slotTemplate;
    [SerializeField] private int gridSize;

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

        for (int i = 0; i < size * size; i++)
        {
            Debug.Log("slot added!");
            TemplateContainer itemSlot = slotTemplate.Instantiate();

            // changes size of boxes depending on grid size!
            itemSlot.style.width = 512f / size;
            itemSlot.style.height = 512f / size;
            itemSlot.style.width = new Length(100f / size, LengthUnit.Percent);
            itemSlot.style.height = new Length(100f / size, LengthUnit.Percent);

            grid.Add(itemSlot);
        }
    }

}
