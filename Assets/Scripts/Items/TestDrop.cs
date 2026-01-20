using UnityEngine;
using UnityEngine.EventSystems;

public class TestDrop : MonoBehaviour, IDropHandler
{
    public DropType type;
    public void OnDrop(PointerEventData eventData)
    {
        /*
        if(eventData.pointerDrag != null)
        {
            Debug.Log("Object was dropped: " + eventData.pointerDrag.name);

        }
        */
        
        GameObject dropped = eventData.pointerDrag;
        switch(type)
        {
            case DropType.Single:
            SingleItem_Inv item1;
            if(dropped.GetComponent<SingleItem_Inv>() != null) 
            {
                Debug.Log("Exists!");
                item1 = dropped.GetComponent<SingleItem_Inv>();
                item1.parentAfterDrag = transform;
            }
            
            break;
        }
        
    }
}