using UnityEngine;

public class PowerButton : MonoBehaviour
{
    [SerializeField] GameObject red, green;
    [SerializeField] LightCollider redCollider;
    [SerializeField] LoadingScene loadingScene;

    void OnMouseDown()
    {
        if(redCollider.GetDetect())
        {
            red.SetActive(false);
            green.SetActive(true);
            loadingScene.ResumeTextAnim();
            loadingScene.ResumeLoadingBar();
        }
    }
}
