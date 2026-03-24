using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum CurrentScene
{
    TITLE,
    NEXT_TITLE,
    LOADING1,
    ROOM1,
    LOADING2,
    ROOM2,
    LOADING3,
    TITLE2,
    NEXT_TITLE2,
    LOADING4,
    ROOM3,
    LOADING5,
    ROOM4,
    END
}

public class SequenceManager : MonoBehaviour
{
    public static SequenceManager Instance;
    [SerializeField] TitleAnimater titleAnimater;
    [SerializeField] NextTitleAnimater nextTitleAnimater; 
    [SerializeField] LoadingScene loadingScene;
    [SerializeField] RoomScript room1;
    [SerializeField] RoomScript room2;
    [SerializeField] LoadingScene loadingScene2;
    [SerializeField] LoadingScene loadingScene3;
    [SerializeField] TitleAnimater title2;
    [SerializeField] NextTitleAnimater nextTitle2;
    [SerializeField] LoadingScene loadingScene4;
    [SerializeField] RoomScript room3;
    [SerializeField] LoadingScene loadingScene5;
    [SerializeField] RoomScript room4;
    [SerializeField] CurrentScene currentScene;
    [SerializeField] GameObject CinemachineObject;
    [SerializeField] GameObject room2UI;

    [Header("Filters")]
    [SerializeField] List<GameObject> filters;

    [Header("Scenes")]
    [SerializeField] List<GameObject> scenes;
    [SerializeField] bool isCanvasOpen;
    [SerializeField] bool isCinemachineEnabled;
    [SerializeField] float zoomSpeed = 5f;
    [SerializeField] float minFOV = 1f;
    [SerializeField] float maxFOV = 5f;
    [SerializeField] float targetFOV;
    [SerializeField] bool isZooming;
    [SerializeField] float moveSpeed;
    public bool IsZooming() { return isZooming; }

    public bool IsCinemachineEnabled() { return isCinemachineEnabled; }
    public void SetCinemachineState(bool value) { isCinemachineEnabled = value; }

    public bool IsCanvasOpen() { return isCanvasOpen; }
    public void SetCanvasOpen(bool value) { isCanvasOpen = value; } 

    public CurrentScene GetCurrentScene() { return currentScene; }
    public void SetCurrentScene(CurrentScene scene) { currentScene = scene; }

    public void SetActiveFilter(int index)
    {
        for(int i = 0; i < filters.Count; i++) filters[i].SetActive(false);

        filters[index].SetActive(true);
    }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        FirstSetup();
        SetActiveFilter(0);
        SetCinemachineState(false);

        // SFXManager.Instance.PlayMusic("Room1");
        targetFOV = CinemachineObject.GetComponent<CinemachineCamera>().Lens.OrthographicSize;
    }

    void Start()
    {
        StartCoroutine(TitleScreen());
        //StartCoroutine(TO_ROOM4());
    }

    void Update()
    {
        CinemachineObject.SetActive(IsCinemachineEnabled());
        if(CinemachineObject.activeInHierarchy == true) HandleZoom();
    }

    void HandleZoom()
    {
        if(InputManager.Instance.IsRightClickDown()) {
            isZooming = true;
        }
        if(InputManager.Instance.IsRightClickUp()) {
            isZooming = false;
        }

        if(isZooming) {
            room2UI.SetActive(false);
            //SetActiveFilter(3);
            CinemachineObject.transform.localPosition = Vector3.Lerp(CinemachineObject.transform.localPosition, new Vector3(PlayerController.Instance.GetMousePos().x, PlayerController.Instance.GetMousePos().y, -10), Time.deltaTime * moveSpeed);

            targetFOV -= Time.deltaTime * zoomSpeed;
            targetFOV = Mathf.Clamp(targetFOV, minFOV, maxFOV);
            CinemachineObject.GetComponent<CinemachineCamera>().Lens.OrthographicSize = Mathf.Lerp(CinemachineObject.GetComponent<CinemachineCamera>().Lens.OrthographicSize, targetFOV, Time.deltaTime * 5f);

        }
        else {
            //SetActiveFilter(1);
            room2UI.SetActive(true);
            targetFOV += Time.deltaTime * zoomSpeed;
            targetFOV = Mathf.Clamp(targetFOV, minFOV, maxFOV);
            CinemachineObject.GetComponent<CinemachineCamera>().Lens.OrthographicSize = Mathf.Lerp(CinemachineObject.GetComponent<CinemachineCamera>().Lens.OrthographicSize, targetFOV, Time.deltaTime * 5f);
            CinemachineObject.transform.localPosition = Vector3.Lerp(CinemachineObject.transform.localPosition, new Vector3(0, 0, -10), Time.deltaTime * 5f);
        }
    }

    void FirstSetup()
    {
        scenes[0].SetActive(true);
        for(int i = 1; i < scenes.Count; i++)
        {
            scenes[i].SetActive(false);
        }
    }

    IEnumerator TitleScreen()
    {
        PlayerController.Instance.SetOnHover(false);
        titleAnimater.PlayLogo();
        SFXManager.Instance.PlayMusic("Title");
        yield return new WaitForSeconds(2f);       
        StartCoroutine(BlinkingText()); 
    }

    IEnumerator BlinkingText()
    {
        
        titleAnimater.PlayBlinkText();
        PlayerController.Instance.SetOnHover(true);
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
            case CurrentScene.LOADING2:
                StartCoroutine(TO_LOADING2());
                break;
            case CurrentScene.ROOM2:
                StartCoroutine(TO_ROOM2());
                break;
            case CurrentScene.LOADING3:
                StartCoroutine(TO_LOADING3());
                break;
            case CurrentScene.TITLE2:
                BlackScreenManager.Instance.FadeIn();
                StartCoroutine(SetupTitle2());
                break;
            case CurrentScene.NEXT_TITLE2:
                StartCoroutine(NextTitle2());
                break;
            case CurrentScene.LOADING4:
                StartCoroutine(TO_LOADING4());
                break;
            case CurrentScene.ROOM3:
                StartCoroutine(TO_ROOM3());
                break;
            case CurrentScene.LOADING5:
                StartCoroutine(TO_LOADING5());
                break;
            case CurrentScene.ROOM4:
                StartCoroutine(TO_ROOM4());
                break;
            case CurrentScene.END:
                StartCoroutine(Endgame());
                break;
        }
    }

    IEnumerator Endgame()
    {
        BlackScreenManager.Instance.FadeIn();

        yield return new WaitForSeconds(5f);

        Application.Quit();

        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
    }

    IEnumerator TO_LOADING4()
    {
        PlayerController.Instance.SetOnHover(false);
        BlackScreenManager.Instance.FadeIn();
        SFXManager.Instance.StopMusic();
        SFXManager.Instance.StopCustomMusic();
        SFXManager.Instance.StopVoice();
        yield return new WaitForSeconds(1f);

        SetCinemachineState(false);
        SetActiveFilter(1);
        scenes[8].SetActive(false);
        scenes[9].SetActive(true);

        yield return new WaitForSeconds(1f);
        SFXManager.Instance.PlayMusic("Loading");
        BlackScreenManager.Instance.FadeOut();

        StartCoroutine(LOADING4());
        
    }
    
    IEnumerator LOADING4()
    {
        yield return new WaitForSeconds(1f);

        loadingScene4.PlayTextAnim();
        loadingScene4.PlayLoading2();
        loadingScene4.ShowHintBar();
    }

    IEnumerator TO_ROOM3()
    {
        BlackScreenManager.Instance.FadeIn();
        SFXManager.Instance.StopMusic();
        yield return new WaitForSeconds(1f);

        SFXManager.Instance.CheckSceneForAmbience();
        SetCinemachineState(true);
        SetActiveFilter(4);
        scenes[9].SetActive(false);
        scenes[10].SetActive(true);

        yield return new WaitForSeconds(1f);

        SFXManager.Instance.PlayMusic("Room1");
        SFXManager.Instance.PlayVoice("Room1_VoiceClip");
        BlackScreenManager.Instance.FadeOut();

        StartCoroutine(ROOM3());
    }

    IEnumerator ROOM3()
    {
        yield return new WaitForSeconds(0.5f);

        room3.AnimateObjective();
    }

    IEnumerator TO_LOADING5()
    {
        PlayerController.Instance.SetOnHover(false);
        BlackScreenManager.Instance.FadeIn();
        SFXManager.Instance.StopMusic();
        SFXManager.Instance.StopCustomMusic();
        SFXManager.Instance.StopVoice();
        yield return new WaitForSeconds(1f);

        SetCinemachineState(false);
        SetActiveFilter(1);
        scenes[10].SetActive(false);
        scenes[11].SetActive(true);

        yield return new WaitForSeconds(1f);
        SFXManager.Instance.PlayMusic("Loading");
        BlackScreenManager.Instance.FadeOut();

        StartCoroutine(LOADING5());
        
    }
    
    IEnumerator LOADING5()
    {
        yield return new WaitForSeconds(1f);

        loadingScene5.PlayTextAnim();
        loadingScene5.PlayLoading2();
        loadingScene5.ShowHintBar();
    }

    IEnumerator TO_ROOM4()
    {
        BlackScreenManager.Instance.FadeIn();
        SFXManager.Instance.StopMusic();
        yield return new WaitForSeconds(1f);

        SFXManager.Instance.CheckSceneForAmbience();
        SetCinemachineState(true);
        SetActiveFilter(4);
        scenes[11].SetActive(false);
        scenes[12].SetActive(true);

        yield return new WaitForSeconds(1f);

        SFXManager.Instance.PlayMusic("Room1");
        BlackScreenManager.Instance.FadeOut();

        StartCoroutine(ROOM4());
    }

    IEnumerator ROOM4()
    {
        yield return new WaitForSeconds(0.5f);

        room4.AnimateObjective();
    }

    IEnumerator SetupTitle2()
    {
        yield return null;
        title2.HideScareText();
        SetCinemachineState(false);
        SetActiveFilter(1);
        scenes[6].SetActive(false);
        scenes[7].SetActive(true);

        yield return new WaitForSeconds(1f);

        BlackScreenManager.Instance.FadeOut();

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(Title2()); 
    }

    IEnumerator Title2()
    {
        PlayerController.Instance.SetOnHover(false);
        title2.PlayLogo();
        SFXManager.Instance.PlayMusic("Title");
        yield return new WaitForSeconds(2f);       
        StartCoroutine(Blink2()); 
    }

    IEnumerator Blink2()
    {
        title2.PlayBlinkText();
        PlayerController.Instance.SetOnHover(true);
        title2.EnableButton();
        yield return null; 
    }

    IEnumerator NextTitle2()
    {
        PlayerController.Instance.SetOnHover(false);
        BlackScreenManager.Instance.FadeIn();
        SFXManager.Instance.StopMusic();
        yield return new WaitForSeconds(1f);

        SFXManager.Instance.CheckSceneForAmbience();
        scenes[7].SetActive(false);
        scenes[8].SetActive(true);

        yield return new WaitForSeconds(1f);

        SFXManager.Instance.PlayMusic("NextTitle");
        BlackScreenManager.Instance.FadeOut();

        StartCoroutine(NEXT_Title2());
    }

    IEnumerator NEXT_Title2()
    {
        nextTitle2.PlayLogo();
        yield return new WaitForSeconds(2f);       
        StartCoroutine(NEXT_BlinkingText2()); 
    }

    IEnumerator NEXT_BlinkingText2()
    {
        nextTitle2.FormShape();
        yield return new WaitForSeconds(0.25f);
        PlayerController.Instance.SetOnHover(true);
        nextTitle2.EnableButton();
        yield return null; 
    }


    IEnumerator To_NEXT_TITLE()
    {
        PlayerController.Instance.SetOnHover(false);
        BlackScreenManager.Instance.FadeIn();
        SFXManager.Instance.StopMusic();
        yield return new WaitForSeconds(1f);

        SFXManager.Instance.CheckSceneForAmbience();
        scenes[0].SetActive(false);
        scenes[1].SetActive(true);

        yield return new WaitForSeconds(1f);

        SFXManager.Instance.PlayMusic("NextTitle");
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
        PlayerController.Instance.SetOnHover(true);
        nextTitleAnimater.EnableButton();
        yield return null; 
    }

    IEnumerator TO_LOADING1()
    {
        PlayerController.Instance.SetOnHover(false);
        BlackScreenManager.Instance.FadeIn();
        SFXManager.Instance.StopMusic();
        yield return new WaitForSeconds(1f);

        SFXManager.Instance.CheckSceneForAmbience();
        scenes[1].SetActive(false);
        scenes[2].SetActive(true);

        yield return new WaitForSeconds(1f);
        SFXManager.Instance.PlayMusic("Loading");
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
        SFXManager.Instance.StopMusic();
        yield return new WaitForSeconds(1f);

        SFXManager.Instance.CheckSceneForAmbience();
        SetCinemachineState(true);
        SetActiveFilter(1);
        scenes[2].SetActive(false);
        scenes[3].SetActive(true);

        yield return new WaitForSeconds(1f);

        SFXManager.Instance.PlayMusic("Room1");
        SFXManager.Instance.PlayVoice("Room1_VoiceClip");
        BlackScreenManager.Instance.FadeOut();

        StartCoroutine(ROOM1());
    }

    IEnumerator ROOM1()
    {
        yield return new WaitForSeconds(0.5f);

        room1.AnimateObjective();
    }

    IEnumerator TO_LOADING2()
    {
        PlayerController.Instance.SetOnHover(false);
        BlackScreenManager.Instance.FadeIn();
        SFXManager.Instance.StopMusic();
        SFXManager.Instance.StopCustomMusic();
        SFXManager.Instance.StopVoice();
        yield return new WaitForSeconds(1f);

        scenes[3].SetActive(false);
        scenes[4].SetActive(true);

        yield return new WaitForSeconds(1f);
        SFXManager.Instance.PlayMusic("Loading");
        BlackScreenManager.Instance.FadeOut();

        StartCoroutine(LOADING2());
        
    }
    
    IEnumerator LOADING2()
    {
        yield return new WaitForSeconds(1f);

        loadingScene2.PlayTextAnim();
        loadingScene2.PlayLoading2();
        loadingScene2.ShowHintBar();
    }

    IEnumerator TO_ROOM2()
    {
        BlackScreenManager.Instance.FadeIn();
        SFXManager.Instance.StopMusic();
        yield return new WaitForSeconds(1f);

        SFXManager.Instance.CheckSceneForAmbience();
        SetCinemachineState(true);
        SetActiveFilter(1);
        scenes[4].SetActive(false);
        scenes[5].SetActive(true);

        yield return new WaitForSeconds(1f);

        SFXManager.Instance.PlayMusic("Room1");
        BlackScreenManager.Instance.FadeOut();

        StartCoroutine(ROOM2());
    }

    IEnumerator ROOM2()
    {
        yield return new WaitForSeconds(0.5f);

        room2.AnimateObjective();
    }

    IEnumerator TO_LOADING3()
    {
        yield return null;

        BlackScreenManager.Instance.Block();
        SFXManager.Instance.StopMusic();
        SFXManager.Instance.StopCustomMusic();
        SFXManager.Instance.PlaySFX("LightsBreak");
        SFXManager.Instance.PlaySFX("PowerDown");

        PlayerController.Instance.SetOnHover(false);

        yield return new WaitForSeconds(1f);

        SFXManager.Instance.PlayCustomMusic(2);

        yield return new WaitForSeconds(3f);

        scenes[5].SetActive(false);
        scenes[6].SetActive(true); //black screen / loading turned off
        loadingScene3.SetPowerDown(true);
        BlackScreenManager.Instance.Unblock();

        StartCoroutine(LOADING3());
    }

    IEnumerator LOADING3()
    {
        yield return new WaitForSeconds(1f);
        loadingScene3.RevealLight();
    }
}
