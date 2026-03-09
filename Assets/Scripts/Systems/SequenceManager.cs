using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

public enum CurrentScene
{
    TITLE,
    NEXT_TITLE,
    LOADING1,
    ROOM1
}

public class SequenceManager : MonoBehaviour
{
    public static SequenceManager Instance;
    [SerializeField] TitleAnimater titleAnimater;
    [SerializeField] NextTitleAnimater nextTitleAnimater; 
    [SerializeField] LoadingScene loadingScene;
    [SerializeField] CurrentScene currentScene;

    [Header("Scenes")]
    [SerializeField] List<GameObject> scenes;

    public CurrentScene GetCurrentScene() { return currentScene; }
    public void SetCurrentScene(CurrentScene scene) { currentScene = scene; }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        scenes[0].SetActive(true);
        for(int i = 1; i < scenes.Count; i++)
        {
            scenes[i].SetActive(false);
        }
    }

    void Start()
    {
        StartCoroutine(TitleScreen());   
    }

    IEnumerator TitleScreen()
    {
        titleAnimater.PlayLogo();
        yield return new WaitForSeconds(2f);       
        StartCoroutine(BlinkingText()); 
    }

    IEnumerator BlinkingText()
    {
        titleAnimater.PlayBlinkText();
        titleAnimater.EnableButton();
        yield return null; 
    }

    public void NextScene(CurrentScene scene)
    {
        SetCurrentScene(scene);
        switch(scene)
        {
            case CurrentScene.NEXT_TITLE:
                StartCoroutine(To_NEXT_TITLE());
                break;
            case CurrentScene.LOADING1:
                StartCoroutine(TO_LOADING1());
                break;
            case CurrentScene.ROOM1:
                StartCoroutine(TO_ROOM1());
                break;

        }
    }

    IEnumerator To_NEXT_TITLE()
    {
        BlackScreenManager.Instance.FadeIn();
        yield return new WaitForSeconds(1f);

        scenes[0].SetActive(false);
        scenes[1].SetActive(true);

        yield return new WaitForSeconds(1f);
        BlackScreenManager.Instance.FadeOut();

        StartCoroutine(NEXT_TitleScreen());
    }

    IEnumerator NEXT_TitleScreen()
    {
        nextTitleAnimater.PlayLogo();
        yield return new WaitForSeconds(2f);       
        StartCoroutine(NEXT_BlinkingText()); 
    }

    IEnumerator NEXT_BlinkingText()
    {
        nextTitleAnimater.FormShape();
        //nextTitleAnimater.PlayBlinkText();
        yield return new WaitForSeconds(0.25f);
        nextTitleAnimater.EnableButton();
        yield return null; 
    }

    IEnumerator TO_LOADING1()
    {
        BlackScreenManager.Instance.FadeIn();
        yield return new WaitForSeconds(1f);

        scenes[1].SetActive(false);
        scenes[2].SetActive(true);

        yield return new WaitForSeconds(1f);
        BlackScreenManager.Instance.FadeOut();

        StartCoroutine(LOADING1());
    }

    IEnumerator LOADING1()
    {
        yield return new WaitForSeconds(1f);

        loadingScene.PlayTextAnim();
        loadingScene.PlayLoadingBar();
        loadingScene.ShowHintBar();
    }

    IEnumerator TO_ROOM1()
    {
        BlackScreenManager.Instance.FadeIn();
        yield return new WaitForSeconds(1f);

        scenes[2].SetActive(false);
        //scenes[3].SetActive(true);

        yield return new WaitForSeconds(1f);
        BlackScreenManager.Instance.FadeOut();

        //StartCoroutine(LOADING1());
    }
}
