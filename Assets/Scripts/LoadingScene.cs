using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] Transform loadingText;
    [SerializeField] Animator loadingTextAnim;
    [SerializeField] Slider loadingBar;
    [SerializeField] CanvasGroup hintContainer;
    [SerializeField] List<GameObject> textList;
    [SerializeField] float loadingSpeed;
    [SerializeField] float percent;
    [SerializeField] float seconds;
    [SerializeField] TweenSettings<float> alpha;
    int currentIndex = 0;
    bool hasPoweredDown;

    public void SetPowerDown(bool value) { hasPoweredDown = value; }
    public bool HasPoweredDown() { return hasPoweredDown; } 

    public void PlayTextAnim()
    {
        loadingTextAnim.enabled = true;
    }

    public void PauseTextAnim()
    {
        loadingTextAnim.speed = 0f;
    }

    public void ResumeTextAnim()
    {
        loadingTextAnim.speed = 1f;
    }

    public void PlayLoadingBar()
    {
        StartCoroutine(PlayLoadingBarCoroutine());
    }

    public void PlayLoading2()
    {
        StartCoroutine(PlayLoading2Coroutine());
    }

    IEnumerator PlayLoading2Coroutine()
    {
        yield return null;

        while(loadingBar.value < 100)
        {
            yield return new WaitForSeconds(seconds);
            percent = loadingBar.value;
            loadingBar.value += loadingSpeed;
        }

        yield return new WaitForSeconds(1f);
        SequenceManager.Instance.NextScene(CurrentScene.INTERMISSION);
    }

    IEnumerator PlayLoadingBarCoroutine()
    {
        yield return null;

        while(loadingBar.value <= 25)
        {
            yield return new WaitForSeconds(seconds);
            percent = loadingBar.value;
            loadingBar.value += loadingSpeed;
        }

        SetPowerDown(true);
        SFXManager.Instance.PauseMusic();
        SFXManager.Instance.PlaySFX("PowerDown");
        yield return new WaitForSeconds(1f);
        SFXManager.Instance.PlayCustomMusic(2);

        PauseTextAnim();
    }

    public void ResumeLoadingBar()
    {
        StartCoroutine(ResumeLoadingBarCoroutine());
    }

    IEnumerator ResumeLoadingBarCoroutine()
    {
        yield return null;

        SFXManager.Instance.StopCustomMusic();
        yield return new WaitForSeconds(0.5f);
        SFXManager.Instance.PlaySFX("PowerOn");
        yield return new WaitForSeconds(0.5f);
        SFXManager.Instance.ResumeMusic();

        ResumeTextAnim();
        
        while(loadingBar.value < 100)
        {
            yield return new WaitForSeconds(seconds / 2);
            percent = loadingBar.value;
            loadingBar.value += loadingSpeed;
        }

        yield return new WaitForSeconds(1f);
        SFXManager.Instance.StopSFX();
        SequenceManager.Instance.NextScene(CurrentScene.ROOM1);
    }

    public void ShowHintBar()
    {
        Tween.Alpha(hintContainer, alpha);
    }

    public void NextHint()
    {
        currentIndex = (currentIndex + 1) % textList.Count;
        HideHints();
        ShowHint(currentIndex);
    }

    public void PreviousHint()
    {
        currentIndex = (currentIndex - 1 + textList.Count) % textList.Count;
        HideHints();
        ShowHint(currentIndex);
    }

    void HideHints()
    {
        for(int i = 0; i < textList.Count; i++)
        {
            textList[i].SetActive(false);
        }
    }

    void ShowHint(int index)
    {
        textList[index].SetActive(true);
    }

    public void TurnTextToNormal()
    {
        textList[2].GetComponent<TextMeshProUGUI>().text = "Let there be <color=#696969>LIGHT</color>!";
    }
}
