using PrimeTween;
using UnityEngine;

public class BlackScreenManager : MonoBehaviour
{   
    public static BlackScreenManager Instance;
    
    [SerializeField] GameObject blocker;
    [SerializeField] RectTransform fade;
    [SerializeField] TweenSettings<float> fadeIn;
    [SerializeField] TweenSettings<float> fadeOut;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void Block()
    {
        blocker.SetActive(true);
    }

    public void Unblock()
    {
        blocker.SetActive(false);
    }

    public void FadeIn()
    {
        SFXManager.Instance.PlaySFX("Transition");
        //Tween.UIAnchoredPositionX(fade, fadeIn);
        Tween.LocalPositionX(fade, fadeIn);
    }

    public void FadeOut()
    {
        //Tween.UIAnchoredPositionX(fade, fadeOut);
        Tween.LocalPositionX(fade, fadeOut);
    }
}
