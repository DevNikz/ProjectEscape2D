using UnityEngine;
using UnityEngine.EventSystems;

public class SceneBounds : MonoBehaviour, IPointerEnterHandler
{
    public int LevelTarget;
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Mouse Has Entered");
        //game
        if(LevelTarget > 0)
        {
            LevelManager.Instance.TrySetTarget(LevelTarget);
            LevelManager.Instance.SetMoving(true);
        }

    }
}
