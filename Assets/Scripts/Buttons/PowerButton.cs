using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PowerButton : MonoBehaviour
{
    [SerializeField] GameObject red, green;
    [SerializeField] LightCollider redCollider;
    [SerializeField] LoadingScene loadingScene;

    bool hasBeenInteracted;

    void OnMouseOver()
    {
        PlayerController.Instance.SetOnHover(true);
    }

    void OnMouseExit()
    {
        PlayerController.Instance.SetOnHover(false);
    }

    void OnMouseDown()
    {
        if(redCollider.GetDetect() && loadingScene.HasPoweredDown())
        {
            if(SequenceManager.Instance.GetCurrentScene() == CurrentScene.LOADING3)
            {
                loadingScene.RevealUI();
            }

            StartCoroutine(Resume());
            
            // red.SetActive(false);
            // green.SetActive(true);
            // loadingScene.ResumeTextAnim();
        }
    }

    IEnumerator Resume()
    {
        yield return new WaitForSeconds(0.1f);

        SFXManager.Instance.PlayUI("Select");
        loadingScene.ResumeLoadingBar();
        loadingScene.SetPowerDown(false);
        hasBeenInteracted = true;
    }

    void Update()
    {
        if(loadingScene.HasPoweredDown())
        {
            red.SetActive(true);
            green.SetActive(false);
        }
        else
        {
            red.SetActive(false);
            green.SetActive(true);
            if(hasBeenInteracted) green.GetComponent<Light2D>().intensity = 1;
            else green.GetComponent<Light2D>().intensity = 0.25f;
        }
    }
}
