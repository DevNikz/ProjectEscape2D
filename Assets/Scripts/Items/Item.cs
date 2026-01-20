using System;
using UnityEngine;

[Serializable]
public class Item
{
    //public string name;
    //public ItemType itemType;
    public ItemSO itemSO;
    public int uses;
    public bool isPickedUp;
    public bool isEquipped;
    public bool isUsed;
}