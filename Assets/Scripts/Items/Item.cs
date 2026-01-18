using System;
using UnityEngine;

[Serializable]
public class Item
{
    public string name;
    public ItemType itemType;
    public bool isPickedUp;
    public bool isEquipped;
    public bool isUsed;
}