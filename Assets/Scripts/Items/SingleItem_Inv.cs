using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SingleItem_Inv : BaseItem, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public event EventHandler OnItemUseStarted;
    public event EventHandler OnItemUseCompleted;
    [SerializeField] public Item item;
    public Transform parentAfterDrag;
    public GameObject tempItem;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Begin Drag");
        if(parentAfterDrag != transform.parent) parentAfterDrag = transform.parent;

        //Instantiate
        tempItem = Instantiate(this.gameObject, parentAfterDrag);
        tempItem.gameObject.transform.SetSiblingIndex(gameObject.transform.GetSiblingIndex());

        Color c = tempItem.GetComponent<Image>().color;
        c.a = 0.5f;
        tempItem.GetComponent<Image>().color = c;

        //After Inst
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        Vector2 objectPos = Camera.main.ScreenToWorldPoint(InputManager.Instance.GetMouseScreenPosition());
        transform.position = new Vector3(objectPos.x, objectPos.y, 0);
        GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(tempItem.gameObject);
        transform.SetParent(parentAfterDrag);
        GetComponent<Image>().raycastTarget = true;
    }

    void Update()
    {
        if(item != null)
        {
            item.uses = GetItemUses();
        }

        if(!isUsed) return;
        else
        {
            OnItemUseCompleted?.Invoke(this, EventArgs.Empty);
            OnSingleItemUseComplete();
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

    private void OnSingleItemUseComplete()
    {
        ItemUseComplete();
        InventoryManager.Instance.RemoveItem(item.itemSO.name);
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

    public void SetParent(Transform _transform)
    {
        parentAfterDrag = _transform;
    }
}
