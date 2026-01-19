using UnityEngine;

[CreateAssetMenu(fileName = "DefaultItem", menuName = "ProjectEscape/General/Item", order = 1)]
public class ItemSO : ScriptableObject
{
    [Header("Name")]
    public string _itemName;
    
    [Header("Type")]
    public ItemType _itemType;
}
