using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.ProceduralImage;

public class GridBlocks : MonoBehaviour, IPointerClickHandler
{
    public bool isAllowed;
    public Sprite defaultSprite;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(PlayerController.Instance.IsFocused() && isAllowed) {
            GetComponent<ProceduralImage>().sprite = defaultSprite;
            GetComponent<ParticleSystem>().Play();
            isAllowed = false;
        }
    }
}
