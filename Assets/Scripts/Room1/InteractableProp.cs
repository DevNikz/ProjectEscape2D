using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableProp : InteractInterface
{
    [SerializeField] GameObject uiCanvas;
    [SerializeField] GameObject menuButton;
    [SerializeField] GameObject targetUI;
    [SerializeField] GameObject targetObj;
    [SerializeField] GameObject secondaryTarget;
    [SerializeField] bool invisHighlight;
    [SerializeField] bool IsTargetAnObj;
    [SerializeField] bool hasNoRenderer;
    [SerializeField] bool hasSecondaryTarget;
    [SerializeField] bool secondaryTargetState;
    GameObject highlight;
    [SerializeField] bool disableComponent;

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
        if(SequenceManager.Instance.IsCanvasOpen() || SequenceManager.Instance.IsZooming()) disableComponent = true;
        else disableComponent = false;
    }

    void OnMouseOver()
    {
        if(!disableComponent)
        {
            Debug.Log($"{this.name}");
            PlayerController.Instance.SetOnHover(true);
            if(!hasNoRenderer) GetComponent<SpriteRenderer>().enabled = false;
            if(!invisHighlight) highlight.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        PlayerController.Instance.SetOnHover(false);
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
            SFXManager.Instance.PlayUI("Open");
            uiCanvas.SetActive(true);
            targetUI.SetActive(true);
            menuButton.SetActive(false);
            SequenceManager.Instance.SetActiveFilter(2);
            SequenceManager.Instance.SetCanvasOpen(true);
        }
        else {
            SFXManager.Instance.PlayUI("Select");
            targetObj.SetActive(true);
            gameObject.SetActive(false);
            if(hasSecondaryTarget) secondaryTarget.SetActive(secondaryTargetState);
        }
    }
}
