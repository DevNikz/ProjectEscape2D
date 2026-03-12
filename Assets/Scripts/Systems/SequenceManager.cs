using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VectorGraphics;
using UnityEngine;

public enum CurrentScene
{
    TITLE,
    NEXT_TITLE,
    LOADING1,
    ROOM1,
    LOADING2,
    INTERMISSION,
}

public class SequenceManager : MonoBehaviour
{
    public static SequenceManager Instance;
    [SerializeField] TitleAnimater titleAnimater;
    [SerializeField] NextTitleAnimater nextTitleAnimater; 
    [SerializeField] LoadingScene loadingScene;
    [SerializeField] RoomScript room1;
    [SerializeField] LoadingScene loadingScene2;
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
        targetFOV = CinemachineObject.GetComponent<CinemachineCamera>().Lens.OrthographicSize;
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

    void Start()
    {
        StartCoroutine(TitleScreen());   
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
        }
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
        SFXManager.Instance.PlayVoice("Room1_Voice");
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
}
