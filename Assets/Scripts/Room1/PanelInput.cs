using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;

public class PanelInput : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> fields;
    [SerializeField] List<bool> fieldAllowed = new List<bool> {true, true, true, true};
    [SerializeField] int currentFieldIndex;
    [SerializeField] RoomScript room;
    string endInput;

    bool pressedEnd;

    public bool GetEnd() { return pressedEnd; }
    public void SetEnd(bool value) { pressedEnd = value; }

    void Start()
    {
        ClearFields();
    }

    public void Submit()
    {

        switch(SequenceManager.Instance.GetCurrentScene())
        {
            case CurrentScene.ROOM1:
                if(CheckAnswerRoom1())
                {
                    SFXManager.Instance.PlayUI("Select");
                    SequenceManager.Instance.NextScene(CurrentScene.LOADING2);
                } //do something
                else {
                    SFXManager.Instance.PlayUI("Error");
                    ClearFields();
                }
                break;
            case CurrentScene.ROOM3:
                if(CheckAnswerRoom3())
                {
                    SFXManager.Instance.PlayUI("Select");
                    SequenceManager.Instance.NextScene(CurrentScene.LOADING5);
                } //do something
                else {
                    SFXManager.Instance.PlayUI("Error");
                    ClearFields();
                }
                break;
            
            case CurrentScene.ROOM2:
                if(CheckAnswerRoom2())
                {
                    SFXManager.Instance.PlayUI("Select");
                    SequenceManager.Instance.NextScene(CurrentScene.LOADING3);
                    //room.InitJump();
                } //do something
                else {
                    SFXManager.Instance.PlayUI("Error");
                    ClearFields();
                }
                break;
            case CurrentScene.ROOM4:
                if(CheckAnswerEnd())
                {
                    SFXManager.Instance.PlayUI("Select");
                    SequenceManager.Instance.NextScene(CurrentScene.END);
                }
                else {
                    SFXManager.Instance.PlayUI("Error");
                    ClearFields();
                }
                break;
        }
        
    }

    bool CheckAnswerRoom1()
    {
        if(fields[0].text == "05" && fields[1].text == "89" &&
            fields[2].text == "07" && fields[3].text == "63")
        {
            return true;
        }
        else return false;
    }

    bool CheckAnswerRoom2()
    {
        if(fields[0].text == "K6" && fields[1].text == "K4" &&
            fields[2].text == "Q2" && fields[3].text == "Q3")
        {
            return true;
        }
        else return false;
    }

    bool CheckAnswerRoom3()
    {
        if(fields[0].text == "22" && fields[1].text == "03" &&
            fields[2].text == "06" && fields[3].text == "07")
        {
            return true;
        }
        else return false;
    }

    bool CheckAnswerEnd()
    {
        if(endInput == "END")
        {
            return true;
        }
        else return false;
    }

    public void ClearFields()
    {
        SFXManager.Instance.PlayUI("Error");
        
        endInput = "";
        for(int i = 0; i < fields.Count; i++) {
            fields[i].text = "";
            fieldAllowed[i] = true;
        }
    }

    public void InputNumber(int num)
    {
        SFXManager.Instance.PlayUI("Select");
        if(CheckCurrentField())
        {
            fields[currentFieldIndex].text += num;
            if(!CheckCharCount()) fieldAllowed[currentFieldIndex] = false; 
        }
    }

    public void InputLetter(string ch)
    {
        SFXManager.Instance.PlayUI("Select");
        if(CheckCurrentField())
        {
            fields[currentFieldIndex].text += ch;
            endInput += ch;
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
