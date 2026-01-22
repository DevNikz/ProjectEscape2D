using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Button object with text as visuals
public abstract class BasePuzzleText : MonoBehaviour, IPointerClickHandler 
{
    [SerializeField] protected TextMeshProUGUI baseText;
    [SerializeField] protected List<GameObject> baseListTexts;

    public virtual TextMeshProUGUI GetBaseText()
    {
        return baseText;
    }

    public virtual List<GameObject> GetBaseTextList()
    {
        return baseListTexts;
    }

    public abstract void OnPointerClick(PointerEventData eventData);    
}

/*
public abstract void OnPointerClick(PointerEventData eventData)
{
    if(eventData.button == PointerEventData.InputButton.Left)
    {
        //Do something here? maybe animate text
    }
}
*/