using System;
using UnityEngine;

public class MultiItem_Inv : BaseItem
{
    public event EventHandler OnItemUseStarted;
    public event EventHandler OnItemUseCompleted;
    public event EventHandler OnItemTakeStarted;
    public event EventHandler OnItemTakeCompleted;
    [SerializeField] public Item item;

    void Update()
    {
        if(item != null)
        {
            item.uses = GetItemUses();
        }

        if(!isUsed) return;
        else
        {
            OnItemTakeCompleted?.Invoke(this, EventArgs.Empty);
            OnMultiItemUseComplete();
        }
    }

    public override string GetItemName()
    {
        return item.itemSO.name;
    }

    public override void TakeItem(Action onItemTakeComplete) {}
    public override void UseItem(Action onItemUseComplete)
    {
        OnItemUseStarted?.Invoke(this, EventArgs.Empty);
        ItemUseStart(onItemUseComplete);
    }

    private void OnMultiItemUseComplete()
    {
        ItemUseComplete();
        Debug.Log(item.uses);
        if(item.uses <= 0) {
            InventoryManager.Instance.RemoveItem(item.itemSO.name);
            Destroy(gameObject);
        }
    }

    public override int GetItemUses()
    {
        return base.numUses;
    }

    public override bool HasPickedUp()
    {
        return base.hasPickedUp;
    }

    public void SetItem(Item value)
    {
        item = value;
    }
    public override Item GetItem()
    {
        return item;
    }
}