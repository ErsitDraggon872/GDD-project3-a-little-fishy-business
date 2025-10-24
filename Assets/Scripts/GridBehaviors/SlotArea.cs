using UnityEngine;
using UnityEngine.UIElements;

public class SlotArea : VisualElement
{
    public bool filled = false;
    public Image icon;
    public string itemGUID = "";    

    public SlotArea()
    {
        AddToClassList("GridSlot");
    }

    public void HoldItem(VisualElement item)
    {
        Add(item);
        filled = true;
    }

    public void RemoveItem()
    {
        Clear();
        filled = false;
    }

}
