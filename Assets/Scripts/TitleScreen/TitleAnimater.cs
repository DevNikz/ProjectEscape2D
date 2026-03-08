using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleAnimater : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] Transform Logo;
    [SerializeField] Transform text;
    [SerializeField] Button bgButton;
    [SerializeField] Transform popUp1, popUp2, popUp3;


    [Header("Properties")]
    [SerializeField] TweenSettings<float> yLogoPos;
    [SerializeField] TweenSettings<Color> textAlpha;
    [SerializeField] TweenSettings<float> scale1, scale2, scale3;

    Sequence sequenceText;

    void Start()
    {
        popUp1.localScale = Vector3.zero;
        popUp2.localScale = Vector3.zero;
        popUp3.localScale = Vector3.zero;
    }

    public void PlayLogo()
    {
        Sequence.Create(1)
            .Chain(Tween.PositionY(Logo, yLogoPos));
    }

    public void PlayBlinkText()
    {
        sequenceText = Sequence.Create(10, Sequence.SequenceCycleMode.Yoyo)
            .Chain(Tween.Custom(textAlpha, onValueChange: newVal => text.GetComponent<TextMeshProUGUI>().color = newVal))
            .OnComplete(PlayBlinkText);
    }

    public void EnableButton()
    {
        bgButton.enabled = true;
    }

    public void DisableButton()
    {
        bgButton.enabled = false;
    }

    public void OpenPopUp(int value)
    {
        switch(value)
        {
            case 1:
                sequenceText.timeScale = 0f;
                popUp1.gameObject.SetActive(true);
                ShowPop1();
                break;
            case 2:
                popUp2.gameObject.SetActive(true);
                ShowPop2();
                break;
            case 3:
                popUp3.gameObject.SetActive(true);
                ShowPop3();
                break;
        }
    }

    public void ShowPop1()
    {
        Sequence.Create(1)
            .Chain(Tween.Scale(popUp1, scale1));
    }

    public void ShowPop2()
    {
        Sequence.Create(1)
            .Chain(Tween.Scale(popUp2, scale2));
    }

    public void ShowPop3()
    {
        Sequence.Create(1)
            .Chain(Tween.Scale(popUp3, scale3));
    }

    public void NextScene()
    {
        SequenceManager.Instance.NextScene(CurrentScene.NEXT_TITLE);
    }

    public void DebugText()
    {
        Debug.Log("Button Clicked Here.");
        sequenceText.timeScale = 0f;
    }
}
