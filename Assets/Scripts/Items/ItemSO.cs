using UnityEngine;

[CreateAssetMenu(fileName = "DefaultItem", menuName = "ProjectEscape/General/Item", order = 1)]
public class ItemSO : ScriptableObject
{
    [Header("Item Properties")]
    public string _itemName;
    public ItemType _itemType;

    //Sprite Properties
    [Header("Sprite Properties")]
    public Sprite _itemSprite;
    public Color _itemColor;
}
