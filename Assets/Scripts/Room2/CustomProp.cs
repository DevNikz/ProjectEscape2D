using System.Collections.Generic;
using UnityEngine;

public class CustomProp : InteractInterface
{
    [SerializeField] GameObject uiCanvas;
    [SerializeField] GameObject targetUI;
    [SerializeField] List<GameObject> pairedChildren;
    [SerializeField] List<GameObject> pairedHighlight;
    [SerializeField] bool invisHighlight;
    [SerializeField] bool hasNoRenderer;
    [SerializeField] bool IsTargetAnObj;
    GameObject highlight;
    bool disableComponent;

    void Start()
    {
        if(!invisHighlight)
        {
            highlight = transform.Find("Highlight").gameObject;
            highlight.SetActive(false);
        }

        if(pairedChildren != null)
        {
            for(int i = 0; i < pairedChildren.Count; i++)
            {
                pairedHighlight.Add(pairedChildren[i].transform.Find("Highlight").gameObject);
            }
        }
    }

    void RevealPairedHighlight()
    {
        for(int i = 0; i < pairedHighlight.Count; i++) {
            pairedChildren[i].GetComponent<SpriteRenderer>().enabled = false;
            pairedHighlight[i].SetActive(true);
        }
    }

    void UnrevealPairedHighlight()
    {
        for(int i = 0; i < pairedHighlight.Count; i++) {
            pairedChildren[i].GetComponent<SpriteRenderer>().enabled = true;
            pairedHighlight[i].SetActive(false);
        }
    }

    void Update()
    {
        if(SequenceManager.Instance.IsCanvasOpen() || SequenceManager.Instance.IsZooming()) disableComponent = true;
        else disableComponent = false;
    }

    void OnMouseOver()
    {
        if(!disableComponent)
        {
            PlayerController.Instance.SetOnHover(true);
            if(!hasNoRenderer) GetComponent<SpriteRenderer>().enabled = false;
            if(!invisHighlight) highlight.SetActive(true);
            if(pairedHighlight != null) RevealPairedHighlight();
        }
    }

    void OnMouseExit()
    {
        PlayerController.Instance.SetOnHover(false);
        if(!hasNoRenderer) GetComponent<SpriteRenderer>().enabled = true;
        if(!invisHighlight) highlight.SetActive(false);
        if(pairedHighlight != null) UnrevealPairedHighlight();
    }

    void OnMouseDown()
    {
        if(!disableComponent) {
            if(!hasNoRenderer) GetComponent<SpriteRenderer>().enabled = true;
            if(!invisHighlight) highlight.SetActive(false);
            InteractObject();
        }
    }

    public override void InteractObject()
    {
        SFXManager.Instance.PlayUI("Open");
        uiCanvas.SetActive(true);
        targetUI.SetActive(true);
        // menuButton.SetActive(false);
        SequenceManager.Instance.SetActiveFilter(2);
        SequenceManager.Instance.SetCanvasOpen(true);
    }
}
