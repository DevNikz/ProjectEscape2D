using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestDrop : BasePuzzleDrop
{
    void Start()
    {
        if(GetComponent<Image>() != null) image = GetComponent<Image>();
    }
    public override void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            GameObject puzzleItem = eventData.pointerDrag;
            switch(dropType)
            {
                case DropType.Single:

                if(puzzleItem.GetComponent<SingleItem_Inv>() != null && 
                puzzleItem.GetComponent<SingleItem_Inv>().item.itemSO == requiredKey.itemSO)
                {
                    puzzleItem.GetComponent<SingleItem_Inv>().SetDropped(true);

                    sItem = puzzleItem.GetComponent<SingleItem_Inv>();
                    sItem.parentAfterDrag = transform;

                    //Change Drop Visuals?
                    image.color = Color.darkRed;
                }

                break;
            }
        }
    }
}

// public class TestDrop : MonoBehaviour, IDropHandler
// {
//     public DropType type;
//     public void OnDrop(PointerEventData eventData)
//     {
//         /*
//         if(eventData.pointerDrag != null)
//         {
//             Debug.Log("Object was dropped: " + eventData.pointerDrag.name);

//         }
//         */
        
//         GameObject dropped = eventData.pointerDrag;
//         switch(type)
//         {
//             case DropType.Single:
//             SingleItem_Inv item1;
//             if(dropped.GetComponent<SingleItem_Inv>() != null) 
//             {
//                 Debug.Log("Exists!");
//                 item1 = dropped.GetComponent<SingleItem_Inv>();
//                 item1.parentAfterDrag = transform;
//             }
            
//             break;
//         }
        
//     }
// }