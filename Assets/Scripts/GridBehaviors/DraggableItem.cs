using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour
{
    #region Private Variables
    private Vector2 cursorStartPos;
    private Vector2 itemStartPos;
    private SlotArea originalSlot;
    #endregion

    public Sprite sprite;
    public VisualElement visElement;
    private VisualElement siblingElement;

    private void Start()
    {   
        visElement = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Item");
        visElement.style.backgroundImage = new StyleBackground(sprite);

        if (visElement == null)
        {
            Debug.LogError("No visual element found!");
        }

        // register to unity events so this script knows when they happen
        visElement.RegisterCallback<PointerDownEvent>(OnPointerDown);
        visElement.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        visElement.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    #region Pointer Behaviors

    // PointerDownEvent sends when a pointer is pressed inside a visual element (our item)
    private void OnPointerDown(PointerDownEvent eventData)
    {
        Debug.Log("pointer down!");
        // save information about the visual element using the eventData
        visElement.CapturePointer(eventData.pointerId);
        cursorStartPos = eventData.position;
        itemStartPos = visElement.layout.position;

        // allow for the item to be freely moved in front of everything else
        //siblingElement = siblingFinder(visElement.parent);
        visElement.style.position = Position.Absolute;
        //visElement.parent.BringToFront();
        visElement.BringToFront();

        // temporarily show that we are indeed holding the item
        visElement.style.scale = new StyleScale(new Vector2(1.2f, 1.2f));

        // check if the pointer is on one of the slot areas
        foreach (SlotArea s in GridManager.gridSlots)
        {
            if (s.worldBound.Contains(eventData.position))
            {
                originalSlot = s;
            }
        }
    }

    private void OnPointerMove(PointerMoveEvent eventData)
    {
        Debug.Log("pointer move!");
        // check if the pointer is on the item we started dragging 
        if (visElement.HasPointerCapture(eventData.pointerId))
        {
            // if it is, move it from where it was
            Vector2 newPos = (Vector2)eventData.position - cursorStartPos;
            visElement.style.left = itemStartPos.x + newPos.x;
            visElement.style.top = itemStartPos.y + newPos.y;
        }
    }

    private void OnPointerUp(PointerUpEvent eventData)
    {
        Debug.Log("pointer up!");
        // check if the pointer is on this item we started dragging
        if (visElement.HasPointerCapture(eventData.pointerId))
        {
            visElement.ReleasePointer(eventData.pointerId);
            visElement.style.scale = new StyleScale(new Vector2(1f, 1f));
            visElement.style.borderTopColor = new Color(0, 0, 0, 0.2f);

            // check if the pointer is on one of the slot areas
            foreach (SlotArea s in GridManager.gridSlots)
            {
                if (s.worldBound.Contains(eventData.position))
                {
                    // is this a valid position to drop the element?
                    Debug.Log("yes there is a slot!");
                    originalSlot.RemoveItem();
                    //visElement.parent.PlaceInFront(siblingElement);

                    if (!s.filled)
                    {
                        visElement.style.position = Position.Relative;
                        visElement.style.left = 0;
                        visElement.style.top = 0;
                        s.HoldItem(visElement);
                    }
                    else
                    {
                        originalSlot.HoldItem(visElement);
                    }
                }
            }

            // go back if not a valid position

            visElement.style.position = Position.Relative;
            Debug.Log("no there is no slot here");
            visElement.style.left = 0;
            visElement.style.top = 0;
        }
    }
    #endregion

    #region Misc Methods

    // takes in a SlotArea and returns the previous SlotArea in the hierarchy so we know where to go back to after 
    private VisualElement siblingFinder(VisualElement vis)
    {
        VisualElement parent = vis.parent;
        if (parent != null)
        {
            var prev = vis;
            foreach (var child in parent.Children())
            {
                Debug.Log(child);
                if (child == vis)
                {
                    return prev;
                }
                prev = child;
            }
            return prev;
        } 
        else
        {
            return vis;
        }
    }
    #endregion
}
