using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [Header("Properties")]
    [SerializeField] private List<Item> inventoryItems;
    [SerializeField] private List<GameObject> inventoryGameObjects;

    [Header("Reference")]
    [SerializeField] private GameObject inventoryContentParent;
    [SerializeField] private GameObject itemTemplatePrefab;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public bool TryAddingSingleItem(Item item)
    {
        if(CanAddSingleItem(item))
        {
            AddItem(item);
            TryAddingItemToInventory(item);
            return true;
        }
        else {
            Debug.Log($"{item.itemSO.name} is already in inventory.");
            return false;
        }
    }

    public bool CanAddSingleItem(Item item)
    {
        //Check if current single item alr exists in the list
        for(int i = 0; i < inventoryItems.Count; i++)
        {
            //If Found, return
            if(inventoryItems[i].itemSO.name == item.itemSO.name) return false;
        }
        return true;
    }

    public bool TryAddingMultiItem(Item item)
    {
        //if true, only add to item use
        //else add new item to inventory
        if(!CanAddMultiItem(item))
        {
            //Debug.Log("Adding to Inventory.");
            AddItem(item);
            TryAddingItemToInventory(item);
            return true;
        }
        return false;
    }

    public bool CanAddMultiItem(Item item)
    {
        //true = exists | false = does not exist
        for(int i = 0; i < inventoryGameObjects.Count; i++)
        {
            if(inventoryGameObjects[i].name == item.itemSO.name) 
            {
                Debug.Log("Incrementing Uses.");
                inventoryGameObjects[i].GetComponent<MultiItem>().AddItemUses();
                return true;
            }
        }
        return false;
    }

    private void TryAddingItemToInventory(Item item)
    {
        GameObject tempItem = Instantiate(itemTemplatePrefab, inventoryContentParent.transform);
        switch(item.itemSO._itemType)
        {
            case ItemType.SingleUse:
                //Init SingleItem
                tempItem.AddComponent<SingleItem>();
                tempItem.GetComponent<SingleItem>().SetItem(item);

                //Edit Sprite
                tempItem.GetComponent<Image>().sprite = item.itemSO._itemSprite;
                tempItem.GetComponent<Image>().color = item.itemSO._itemColor;

                //Edit Object
                tempItem.name = item.itemSO._itemName;
                tempItem.tag = "Inv_SingleItem";
                
                break;

            case ItemType.MultipleUse:
                //Init SingleItem
                tempItem.AddComponent<MultiItem>();
                tempItem.GetComponent<MultiItem>().SetItem(item);

                //Edit Sprite
                tempItem.GetComponent<Image>().sprite = item.itemSO._itemSprite;
                tempItem.GetComponent<Image>().color = item.itemSO._itemColor;

                //Edit Object
                tempItem.name = item.itemSO._itemName;
                tempItem.tag = "Inv_MultiItem";
                break;
        }
        inventoryGameObjects.Add(tempItem);
    }

    public List<Item> GetInventory()
    {
        return inventoryItems;
    }

    private void AddItem(Item item)
    {
        inventoryItems.Add(item);
    }

    public void RemoveItemAt(int index)
    {
        inventoryItems.RemoveAt(index);
    }

    public void ClearInventory() 
    { 
        inventoryItems.Clear(); 
    }
}
