using UnityEngine;
using UnityEngine.EventSystems;

public class CustomUI_Pickup : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    Vector3 startPosition;
    RectTransform rect;
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor; 
    }
    public void OnEndDrag(PointerEventData eventData)
    {
    }

}

/*
public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        startParent = transform.parent;
        // Optional: make the image slightly transparent and not a raycast target
        // so the object underneath can receive the IDropHandler event.
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the image with the mouse pointer
        // For Screen Space - Overlay or Screen Space - Camera, different position logic might be needed.
        // eventData.delta provides the change in position since the last frame
        rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor; 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Return the image to full opacity and make it a raycast target again
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        // If the item wasn't dropped on a valid drop zone with IDropHandler, it returns to its start position
        if (transform.parent == startParent)
        {
            transform.position = startPosition;
        }
    }
*/