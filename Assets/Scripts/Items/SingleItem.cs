using System;
using Unity.VisualScripting;
using UnityEngine;

//Make sure this object's tag is set to "SingleItem"
public class SingleItem : BaseItem
{
    public event EventHandler OnItemUseStarted;
    public event EventHandler OnItemUseCompleted;
    public event EventHandler OnItemTakeStarted;
    public event EventHandler OnItemTakeCompleted;


    [SerializeField] public Item item;
    [SerializeField] public int itemUses;

    void Update()
    {
        itemUses = GetItemUses();
        if(!hasPickedUp) return;
        else
        {
            OnItemTakeCompleted?.Invoke(this, EventArgs.Empty);
            OnPickupItemComplete();
        }
        // if(!isUsed) return;
        // else
        // {
        //     //Do something here maybe or a checker?

        //     //OnItemCompleted?.Invoke(this, EventArgs.Empty);
        //     //ItemUseComplete();
        //     OnItemUseCompleted?.Invoke(this, EventArgs.Empty);
        //     OnSingleItemComplete();
        // }
    }
    public override string GetItemName()
    {
        return item.itemSO.name;
    }

    public override void TakeItem(Action onItemTakeComplete)
    {
        OnItemTakeStarted?.Invoke(this, EventArgs.Empty);
        ItemTakeStart(onItemTakeComplete);
    }

    public override void UseItem(Action onItemUseComplete)
    {
        //Do something here (i.e. open door or something)
        //Debug.Log($"{gameObject.name} has started.");

        OnItemUseStarted?.Invoke(this, EventArgs.Empty);
        ItemUseStart(onItemUseComplete);
    }

    private void OnSingleItemComplete()
    {
        //Debug.Log($"{gameObject.name} has been used.");
        ItemUseComplete();
    }

    private void OnPickupItemComplete()
    {
        ItemTakeComplete();
    }

    public Item GetItem()
    {
        return item;
    }

    public override int GetItemUses()
    {
        return base.GetItemUses();
    }
}