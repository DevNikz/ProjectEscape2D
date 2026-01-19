using System;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    public static event EventHandler OnAnyItemUseStarted;
    public static event EventHandler OnAnyItemUseCompleted;

    protected bool isUsed;
    protected bool isLocked;
    protected int numUses = 1;

    protected Action onItemUseComplete;
    public abstract string GetItemName();
    public abstract void UseItem(Action onItemUseComplete);

    public virtual int GetItemUses()
    {
        return numUses;
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
