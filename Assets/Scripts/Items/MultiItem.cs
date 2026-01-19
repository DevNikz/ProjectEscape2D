using System;
using UnityEngine;

//Make sure this object's tag is set to "MultiItem"
public class MultiItem : BaseItem
{
    [SerializeField] public Item item;
    [SerializeField, Range(2f, 5f)] private int itemUses;
    private int currentItemUses;

    void Awake()
    {
        currentItemUses = itemUses;
    }

    public override string GetItemName()
    {
        return item.itemSO.name;
    }

    public override int GetItemUses()
    {
        return currentItemUses;
    }

    public override void UseItem(Action onItemUseComplete)
    {
        ItemUseStart(onItemUseComplete);
    }

    private void OnMultiItemComplete()
    {
        ItemUseComplete();
    }

    public Item GetItem()
    {
        return item;
    }
}