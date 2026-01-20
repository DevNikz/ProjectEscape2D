using System;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    public static event EventHandler OnAnyItemUseStarted;
    public static event EventHandler OnAnyItemUseCompleted;
    public static event EventHandler OnAnyItemTakeStarted;
    public static event EventHandler OnAnyItemTakeCompleted;

    protected bool hasPickedUp;
    protected bool isUsed;
    protected bool isLocked;
    protected int numUses = 1;

    protected Action onItemUseComplete;
    protected Action onItemTakeComplete;
    public abstract string GetItemName();
    public abstract void UseItem(Action onItemUseComplete);
    public abstract void TakeItem(Action onItemTakeComplete);
    public abstract Item GetItem();

    public virtual int GetItemUses()
    {
        return numUses;
    }

    public int AddItemUses()
    {
        return numUses++;
    }

    public virtual bool HasPickedUp()
    {
        return hasPickedUp;
    }

    public int DecreaseItemUses()
    {
        return numUses--;
    }

    protected void ItemUseStart(Action onItemUseComplete)
    {
        isUsed = true;
        this.onItemUseComplete = onItemUseComplete;

        OnAnyItemUseStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ItemUseComplete()
    {
        isUsed = false;
        onItemUseComplete();

        OnAnyItemUseCompleted?.Invoke(this, EventArgs.Empty);
    }

    protected void ItemTakeStart(Action onItemTakeComplete)
    {
        hasPickedUp = true;
        this.onItemTakeComplete = onItemTakeComplete;

        OnAnyItemTakeStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ItemTakeComplete()
    {
        onItemTakeComplete();

        OnAnyItemUseCompleted?.Invoke(this, EventArgs.Empty);
    }

    public bool GetMultiUseComplete()
    {
        if(GetItemUses() == 0) return true;
        return false;
    }

    public void SetItemUses(int value)
    {
        numUses = value;
    }
}
