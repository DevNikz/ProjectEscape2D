using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextTitleAnimater : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] Transform Logo;
    [SerializeField] Transform text;
    [SerializeField] Transform textShape;
    [SerializeField] Button bgButton;

    [Header("Properties")]
    [SerializeField] TweenSettings<float> yLogoPos;
    [SerializeField] TweenSettings<Color> textAlpha;
    [SerializeField] TweenSettings<float> yTextShape;
    [SerializeField] ShakeSettings cameraShake;
    [SerializeField] ShakeSettings cameraShake2;
    [SerializeField] ShakeSettings cameraShake3;

    Sequence sequenceLogo;
    Sequence sequenceText;

    [SerializeField] float insertAtTime;
    [SerializeField] float insertAtTime2;
    [SerializeField] float insertAtTime3;
  
    public void PlayLogo()
    {
        Sequence.Create(1)
            .Chain(Tween.PositionY(Logo, yLogoPos))
            .Insert(atTime: insertAtTime, Tween.ShakeLocalPosition(Camera.main.transform, cameraShake))
            .Insert(atTime: insertAtTime2, Tween.ShakeLocalPosition(Camera.main.transform, cameraShake2))
            .Insert(atTime: insertAtTime3, Tween.ShakeLocalPosition(Camera.main.transform, cameraShake3));
    }

    public void FormShape()
    {
        Tween.PositionY(textShape, yTextShape).OnComplete(PlayBlinkText);
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

    public void NextScene()
    {
        SequenceManager.Instance.NextScene(CurrentScene.LOADING1);
    }
}
