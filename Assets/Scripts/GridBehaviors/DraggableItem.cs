using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour
{
    #region Private Variables
    private VisualElement visElement;
    private Vector2 cursorStartPos;
    private Vector2 itemStartPos;
    #endregion

    private void OnEnable()
    {
        // register to unity events so this script knows when they happen
        visElement.RegisterCallback<PointerDownEvent>(OnPointerDown);
        visElement.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        visElement.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    private void OnDisable()
    {
        // unregister
        visElement.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        visElement.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
        visElement.UnregisterCallback<PointerUpEvent>(OnPointerUp);
    }

    // PointerDownEvent sends when a pointer is pressed inside a visual element (our item)
    private void OnPointerDown(PointerDownEvent eventData)
    {
        // save information about the visual element using the eventData
        visElement.CapturePointer(eventData.pointerId);
        cursorStartPos = eventData.position;
        itemStartPos = visElement.layout.position;

        // allow for the item to be freely moved in front of everything else
        visElement.style.position = Position.Absolute; 
        visElement.BringToFront();

        // temporarily show that we are indeed holding the item
        visElement.style.borderTopColor = Color.green; 
    }

    private void OnPointerMove(PointerMoveEvent eventData)
    {
        // check if the pointer is on the item we started dragging 
        if (visElement.HasPointerCapture(eventData.pointerId))
        {
            // if it is, move it from where it was
            Vector2 newPos = (Vector2)eventData.position - itemStartPos;
            visElement.style.left = itemStartPos.x + newPos.x;
            visElement.style.top = itemStartPos.y + newPos.y;
        }
    }

    private void OnPointerUp(PointerUpEvent eventData)
    {
        // check if the pointer is on this item we started dragging
        if (visElement.HasPointerCapture(eventData.pointerId))
        {
            if (false)
            {
                // is this a valid position to drop the element?
            }

            // go back if not a valid position
            visElement.style.left = itemStartPos.x;
            visElement.style.top = itemStartPos.y;
            visElement.style.position = Position.Relative;
            visElement.style.borderTopColor = new Color(0, 0, 0, 0);

        }
    }

}
