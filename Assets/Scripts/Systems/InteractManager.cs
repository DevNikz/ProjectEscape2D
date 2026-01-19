using System;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    public static InteractManager Instance;

    public event EventHandler<bool>  OnInteractChanged;
    public static event EventHandler OnAnyItemUsed;
    public event EventHandler OnItemUseStarted;
    public event EventHandler OnItemTakeStarted;
    
    public BaseItem tempSelectedItem;

    [SerializeField] private bool hasInteracted;
    private Camera mainCamera;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        mainCamera = Camera.main;
    }

    void Update()
    {
        if(hasInteracted) return;
        HandleInteraction();
    }

    void HandleInteraction()
    {
        if(InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(InputManager.Instance.GetMouseScreenPosition());
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if(hit.collider != null)
            {
                CheckItem(hit.collider.gameObject);
            }
        }
    }

    void CheckItem(GameObject gameObject)
    {
        if(gameObject.CompareTag("SingleItem"))
        {
            tempSelectedItem = gameObject.GetComponent<SingleItem>();
            if(!TrySpendItemUseToUseItem(tempSelectedItem)) return;

            SetInteract();
            tempSelectedItem.TakeItem(ClearInteract);
            //tempSelectedItem.UseItem(ClearInteract);
            
            OnItemTakeStarted?.Invoke(this, EventArgs.Empty);
            //OnItemUseStarted?.Invoke(this, EventArgs.Empty);
        }

        if(gameObject.CompareTag("MultiItem"))
        {
            tempSelectedItem = gameObject.GetComponent<MultiItem>();
            if(!TrySpendItemUseToUseItem(tempSelectedItem)) return;

            SetInteract();
            tempSelectedItem.TakeItem(ClearInteract);
            OnItemTakeStarted?.Invoke(this, EventArgs.Empty);
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