using System;
using UnityEngine;

//Make sure this object's tag is set to "SingleItem"
public class SingleItem : BaseItem
{
    [SerializeField] public Item item;
    public override string GetItemName()
    {
        return item.name;
    }

    public override void UseItem(Action onItemUseComplete)
    {
        ItemUseStart(onItemUseComplete);
    }

    private void OnSingleItemComplete()
    {
        ItemUseComplete();
    }

    public Item GetItem()
    {
        return item;
    }

}