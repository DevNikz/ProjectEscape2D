using System;
using System.Collections.Generic;
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
    [SerializeField] private AudioSource voiceSource;
    [SerializeField] private List<AudioSource> customMusicList;
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
        voiceSource = GetComponentsInChildren<AudioSource>()[2];
        //SwitchAudio();

        switch(SequenceManager.Instance.GetCurrentScene())
        {
            case CurrentScene.ROOM1:
                customMusicList[0].Play();
                customMusicList[1].Play();
                //customMusicList[2].Play();
                // PlayMusic("RoomAmbience1");
                // PlayMusic("ClockTick");
                break;
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.clip == null || musicSource == null) return;

        musicSource.clip = s.clip;
        musicSource.loop = s.loop;
        musicSource.Play();
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

    public void PlayVoice(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.clip == null || voiceSource == null) return;  

        voiceSource.PlayOneShot(s.clip, s.volume);  
    }
}