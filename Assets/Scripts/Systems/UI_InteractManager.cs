using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InteractManager : MonoBehaviour
{
    GraphicRaycaster graphicRaycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;

    public event EventHandler<bool>  OnInteractChanged;
    public static event EventHandler OnAnyItemUsed;
    public event EventHandler OnItemUseStarted;
    public event EventHandler OnItemTakeStarted;
    public BaseItem tempSelectedItem;
    
    [SerializeField] private bool hasInteracted;
    [SerializeField] private GameObject pointerItemPrefab;

    void Start()
    {
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        if(InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = InputManager.Instance.GetMouseScreenPosition();
            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerEventData, results);

            if(results.Count > 0) 
            {
                //InstantiateItem(results[0].gameObject);
                //CheckUI(results[0].gameObject);
            }
            //Debug.Log($"UI count: {results.Count}");
        }
    }

    void CheckUI(GameObject gameObject)
    {
        if(gameObject.CompareTag("Inv_SingleItem"))
        {
            tempSelectedItem = gameObject.GetComponent<SingleItem_Inv>();
            if(!TrySpendItemUseToUseItem(tempSelectedItem)) return;

            SetInteract();
            tempSelectedItem.UseItem(ClearInteract);
            OnItemUseStarted?.Invoke(this, EventArgs.Empty);
        }

        if(gameObject.CompareTag("Inv_MultiItem"))
        {
            tempSelectedItem = gameObject.GetComponent<MultiItem_Inv>();
            if(!TrySpendItemUseToUseItem(tempSelectedItem)) return;

            SetInteract();
            tempSelectedItem.UseItem(ClearInteract);
            OnItemUseStarted?.Invoke(this, EventArgs.Empty);
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
        OnAnyItemUsed?.Invoke(this, EventArgs.Empty);
    }

    private void SetInteract()
    {
        hasInteracted = true;
        OnInteractChanged?.Invoke(this, hasInteracted);
    }

    private void ClearInteract()
    {
        hasInteracted = false;
        OnInteractChanged?.Invoke(this, hasInteracted);
    }
}
