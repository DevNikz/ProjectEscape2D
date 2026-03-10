using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelInput : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> fields;
    [SerializeField] List<bool> fieldAllowed = new List<bool> {true, true, true, true};
    [SerializeField] int currentFieldIndex;

    void Start()
    {
        ClearFields();
    }

    public void Submit()
    {
        if(CheckAnswer()) {} //do something
        else ClearFields();
    }

    bool CheckAnswer()
    {
        if(fields[0].text == "05" && fields[1].text == "89" &&
            fields[2].text == "07" && fields[3].text == "63")
        {
            return true;
        }
        else return false;
    }

    public void ClearFields()
    {
        for(int i = 0; i < fields.Count; i++) fields[i].text = "";
    }

    public void InputNumber(int num)
    {
        if(CheckCurrentField())
        {
            fields[currentFieldIndex].text += num;
            if(!CheckCharCount()) fieldAllowed[currentFieldIndex] = false; 
        }
    }

    bool CheckCharCount()
    {
        if(fields[currentFieldIndex].text.Length < 2) return true;
        else return false;
    }

    bool CheckCurrentField()
    {
        for(int i = 0; i < fieldAllowed.Count; i++)
        {
            if(fieldAllowed[i] == true) {
                currentFieldIndex = i;
                return true;
            }
        }
        return false;
    }
}
