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
                    sItem = puzzleItem.GetComponent<SingleItem_Inv>();
                    if(!TrySpendItemUseToUseItem(sItem)) return;

                    sItem.SetDropped(true);
                    sItem.UseItem(NothingHere);
                    sItem.parentAfterDrag = transform;

                    //Change Drop Visuals?
                    image.color = Color.softRed;
                }

                break;

                case DropType.Multiple:
                
                if(puzzleItem.GetComponent<MultiItem_Inv>() != null && 
                puzzleItem.GetComponent<MultiItem_Inv>().item.itemSO == requiredKey.itemSO)
                {
                    mItem = puzzleItem.GetComponent<MultiItem_Inv>();
                    if(!TrySpendItemUseToUseItem(mItem)) return;

                    mItem.SetDropped(true);
                    mItem.UseItem(NothingHere);
                    //mItem.parentAfterDrag = transform;

                    image.color = Color.softBlue;
                }

                break;
            }
        }
    }

    public bool TrySpendItemUseToUseItem(BaseItem baseItem)
    {
        if(CanSpendItemUseToUseItem(baseItem))
        {
            SpendActionPoints(baseItem);
            return true;
        }
        else return false;
    }

    public bool CanSpendItemUseToUseItem(BaseItem baseItem)
    {
        if (baseItem.GetItemUses() > 0) return true;
        else return false;
    }

    private void SpendActionPoints(BaseItem baseItem)
    {
        baseItem.DecreaseItemUses();
    }

    void NothingHere()
    {
        //Default
    }
}