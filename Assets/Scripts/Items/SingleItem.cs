using System;
using Unity.VisualScripting;
using UnityEngine;

//Make sure this object's tag is set to "SingleItem"
public class SingleItem : BaseItem
{
    public event EventHandler OnItemStarted;
    public event EventHandler OnItemCompleted;
    [SerializeField] public Item item;
    [SerializeField] public int itemUses;

    void Update()
    {
        itemUses = GetItemUses();
        if(!isUsed) return;
        else
        {
            //Do something here maybe or a checker?

            //OnItemCompleted?.Invoke(this, EventArgs.Empty);
            //ItemUseComplete();
            OnItemCompleted?.Invoke(this, EventArgs.Empty);
            OnSingleItemComplete();
        }
    }
    public override string GetItemName()
    {
        return item.name;
    }

    public override void UseItem(Action onItemUseComplete)
    {
        //Do something here (i.e. open door or something)
        //Debug.Log($"{gameObject.name} has started.");

        OnItemStarted?.Invoke(this, EventArgs.Empty);
        ItemUseStart(onItemUseComplete);
    }

    private void OnSingleItemComplete()
    {
        //Debug.Log($"{gameObject.name} has been used.");
        ItemUseComplete();
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