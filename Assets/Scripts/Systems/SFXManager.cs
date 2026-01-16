using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    public Sound[] sounds;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource uiSource;
    [SerializeField] private AudioMixer masterMixer;

    [Range(0.1f, 10f)][SerializeField] public float fadeThreshold = 0.1f;
    [ReadOnly] public float volumeTemp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        sfxSource = GetComponentsInChildren<AudioSource>()[0]; //first in hierarchy
        musicSource = GetComponentsInChildren<AudioSource>()[1];
        uiSource = GetComponentsInChildren<AudioSource>()[2];

        SceneManager.sceneLoaded += OnSceneLoaded;
        //SwitchAudio();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0: //Main Menu
                StopMusic();
                //PlayMusic("ClockworkRondo"); //TitleMusic
                break;
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.clip == null || musicSource == null) return;

        musicSource.clip = s.clip;
        musicSource.Play();
        musicSource.loop = true;
    }

    public void StopMusic() //Call this first before play music
    {
        if (musicSource != null && musicSource.isPlaying)
            musicSource.Stop();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.clip == null || sfxSource == null) return;    
        
        // if(name == "Alerted")
        // {
        //     if (!hasPlayedAlert)
        //     {
        //         sfxSource.PlayOneShot(s.clip, s.volume);
        //         hasPlayedAlert = true;
        //     }
        //     else return;
        // }
        //else sfxSource.PlayOneShot(s.clip, s.volume);

        sfxSource.PlayOneShot(s.clip, s.volume);
    }

    public void PlaySFXAtPosition(string name, Vector3 position)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.clip == null) return;

        GameObject tempGO = new GameObject("TempSFX3D_" + s.name);
        tempGO.transform.position = position;

        AudioSource tempSource = tempGO.AddComponent<AudioSource>();
        tempSource.clip = s.clip;
        tempSource.volume = s.volume;
        tempSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        tempSource.outputAudioMixerGroup = sfxSource.outputAudioMixerGroup;
        tempSource.spatialBlend = 1.0f;
        tempSource.minDistance = 1.0f;  
        tempSource.maxDistance = 90.0f;
        tempSource.rolloffMode = AudioRolloffMode.Linear;

        tempSource.Play();

        Destroy(tempGO, s.clip.length / tempSource.pitch);
    }

    public void PlayUI(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.clip == null || uiSource == null) return;  

        uiSource.PlayOneShot(s.clip, s.volume);  
    }
}