using System;
using UnityEngine;

//Make sure this object's tag is set to "MultiItem"
public class MultiItem : BaseItem
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

        if(!hasPickedUp) return;
        else
        {
            OnItemTakeCompleted?.Invoke(this, EventArgs.Empty);
            OnPickupItemComplete();
        }
    }

    public override string GetItemName()
    {
        return item.itemSO.name;
    }

    public override void TakeItem(Action onItemTakeComplete)
    {
        //Event Invokers
        OnItemTakeStarted?.Invoke(this, EventArgs.Empty);
        ItemTakeStart(onItemTakeComplete);
        item.isPickedUp = HasPickedUp();
    }

    public override void UseItem(Action onItemUseComplete) {}

    private void OnPickupItemComplete()
    {
        ItemTakeComplete();
        Destroy(gameObject);
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