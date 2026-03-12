using UnityEngine;
using UnityEngine.EventSystems;

public class GameUI_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayerController.Instance.SetOnHover(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerController.Instance.SetOnHover(false);
    }
}