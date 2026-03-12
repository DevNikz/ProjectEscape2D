using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class RoomScript : MonoBehaviour
{
    [SerializeField] Transform objective;
    [SerializeField] CanvasGroup canvas;
    [SerializeField] TweenSettings<float> yPos;
    [SerializeField] TweenSettings<float> alpha;
    [SerializeField] AudioMixer master;
    public void AnimateObjective()
    {
        Sequence.Create(1)
            .Group(Tween.Alpha(canvas, alpha))
            .Group(Tween.PositionY(objective, yPos));   
    }

    public void AdjustMusicVolume(float volume)
    {
        master.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void AdjustSFXVolume(float volume)
    {
        master.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    public void AdjustVoiceVolume(float volume)
    {
        master.SetFloat("VoiceVolume", Mathf.Log10(volume) * 20);
    }
}
