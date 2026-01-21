using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BasePuzzleDrop : MonoBehaviour, IDropHandler
{
    [SerializeField] protected DropType dropType;
    [SerializeField] protected Item requiredKey;
    protected Image image;
    protected SingleItem_Inv sItem;
    public abstract void OnDrop(PointerEventData eventData);
}