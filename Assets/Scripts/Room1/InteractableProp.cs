using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableProp : InteractInterface
{
    [SerializeField] GameObject uiCanvas;
    [SerializeField] GameObject menuButton;
    [SerializeField] GameObject targetUI;
    [SerializeField] GameObject targetObj;
    [SerializeField] bool invisHighlight;
    [SerializeField] bool IsTargetAnObj;
    [SerializeField] bool hasNoRenderer;
    GameObject highlight;
    bool disableComponent;

    void Start()
    {
        if(!invisHighlight)
        {
            highlight = transform.Find("Highlight").gameObject;
            highlight.SetActive(false);
        }
    }

    void Update()
    {
        if(SequenceManager.Instance.IsCanvasOpen()) disableComponent = true;
        else disableComponent = false;
    }

    void OnMouseOver()
    {
        if(!disableComponent)
        {
            if(!hasNoRenderer) GetComponent<SpriteRenderer>().enabled = false;
            if(!invisHighlight) highlight.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if(!hasNoRenderer) GetComponent<SpriteRenderer>().enabled = true;
        if(!invisHighlight) highlight.SetActive(false);
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
        if(!IsTargetAnObj) {
            uiCanvas.SetActive(true);
            targetUI.SetActive(true);
            menuButton.SetActive(false);
            SequenceManager.Instance.SetActiveFilter(2);
            SequenceManager.Instance.SetCanvasOpen(true);
        }
        else {
            targetObj.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
