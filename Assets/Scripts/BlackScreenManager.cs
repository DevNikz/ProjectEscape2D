using PrimeTween;
using UnityEngine;

public class BlackScreenManager : MonoBehaviour
{   
    public static BlackScreenManager Instance;
    
    [SerializeField] Transform fade;
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
    public void FadeIn()
    {
        SFXManager.Instance.PlaySFX("Transition");
        Tween.LocalPositionX(fade, fadeIn);
    }

    public void FadeOut()
    {
        Tween.LocalPositionX(fade, fadeOut);
    }
}
