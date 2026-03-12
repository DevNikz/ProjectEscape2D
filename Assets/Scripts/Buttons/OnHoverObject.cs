using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnHoverObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject objectToAppear;
    Color origColor = new Color(0.8018868f, 0.4880016f, 0.2458615f);
    Color highlightColor = new Color(0.4528302f, 0.2431698f, 0.08330368f);

    public void OnPointerEnter(PointerEventData eventData)
    {
        objectToAppear.SetActive(true);
        GetComponent<Image>().color = highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        objectToAppear.SetActive(false);
        GetComponent<Image>().color = origColor;
    }
}
