using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using DG.Tweening;

// responsible for managing sound effects and music
// sound effects can be played by name and music can persist through scene transitions


public class AudioManager : MonoBehaviour
{
    public AudioClip[] sounds;
    AudioSource audioSource;

    public SongSO mainSong;

    public GameObject musicObj;
    public float musicVolume;
    private List<AudioSource> musicSources;
    private SongSO currentSong;
    private int currentVersion;
    private bool isPlaying; // this gets set to false immediately when the music starts fading out
                                // so its value does not necessarily equal musicSources[0].isPlaying

    public void Init() {
        audioSource = GetComponent<AudioSource>();
        musicSources = musicObj.GetComponents<AudioSource>().ToList();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start() {
        PlaySong(mainSong);
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode) {
        if (Managers.ScenesManager.IsHubSceneLoaded()) {
            SwitchVersion(0);
        }
        else if (Managers.ScenesManager.IsPuzzleSceneLoaded()) {
            SwitchVersion(1);
        }
    }


    private void Update() {
        // loop music
        if (musicSources.Count > 0 && musicSources[0].isPlaying && musicSources[0].time >= currentSong.loopEndTime) {
            foreach (AudioSource s in musicSources) {
                s.time = musicSources[0].time - (currentSong.loopEndTime - currentSong.loopStartTime);
            }
        }
    }


    public void PlaySound(string soundName, float volumeMultiplier = 1f) {
        // todo
        //get the sound
        AudioClip s = Array.Find(sounds, sound => sound.name == soundName);
        if(s != null)
        {
            audioSource.PlayOneShot(s, volumeMultiplier);
        }
        else
        {
            Debug.Log("Could not play the sound because it's not in the array!");
        }

        
    }

    public void PlaySoundAt(string soundName, Vector3 location, float volumeMultiplier = 1f) {
        // todo
    }

    public void PlaySong(SongSO song, float fadeInTime = 0f) {
        if (song.versions.Length == 0) {
            Debug.LogError("Trying to play a song that doesn't have any primary audio!");
            return;
        }
        
        if (song == currentSong) {
            return;
        }
        currentSong = song;
        isPlaying = true;

        // ensure we have the right number of audio sources for this song
        while (musicSources.Count > song.versions.Length + song.layers.Length) {
            // delete audio sources
            AudioSource s = musicSources[musicSources.Count - 1];
            musicSources.RemoveAt(musicSources.Count - 1);
            Destroy(s);
        }
        while (musicSources.Count < song.versions.Length + song.layers.Length) {
            // create audio sources
            AudioSource s = musicObj.AddComponent<AudioSource>();
            s.outputAudioMixerGroup = musicSources[0].outputAudioMixerGroup;
            musicSources.Add(s);
        }

        foreach (AudioSource musicSource in musicSources) {
            if (musicSource.isPlaying)
                musicSource.Stop();
            musicSource.volume = 0;
        }

        int m = 0;
        foreach (AudioClip clip in currentSong.versions) {
            musicSources[m].clip = clip;
            m++;
        }
        foreach (AudioClip clip in currentSong.layers) {
            musicSources[m].clip = clip;
            m++;
        }
        musicSources[0].volume = musicVolume;

        foreach (AudioSource musicSource in musicSources) {
            musicSource.Play();
        }
    }

    public void StopMusic(string musicName, float fadeOutTime = 2f) {
        if (!isPlaying)
            return;
        isPlaying = false;

        var seq = DOTween.Sequence();
        foreach (AudioSource source in musicSources) {
            source.DOKill();
            seq.Insert(0, GetFadeTween(source, false, fadeOutTime, false));
        }
        seq.SetTarget(this).SetLink(gameObject);
    }

    public void SwitchVersion(int version, float fadeTime = 2f) {
        if (!isPlaying)
            return;
        
        var seq = DOTween.Sequence();
        for (int v = 0; v < currentSong.versions.Length; v++) {
            musicSources[v].DOKill();
            seq.Insert(0, GetFadeTween(musicSources[v], v == version, fadeTime, true));
        }
    }

    public void EnableLayer(int layer, float fadeTime = 1f) {
        if (!isPlaying)
            return;
        
        AudioSource source = musicSources[currentSong.versions.Length + layer];
        source.DOKill();
        GetFadeTween(source, true, fadeTime, true);
    }

    public void DisableLayer(int layer, float fadeTime = 1f) {
        if (!isPlaying)
            return;

        AudioSource source = musicSources[currentSong.versions.Length + layer];
        source.DOKill();
        GetFadeTween(source, false, fadeTime, true);
    }


    private Tween GetFadeTween(AudioSource audioSource, bool shouldPlay, float duration, bool hasLayers) {
        Ease ease = Ease.Linear;
        if (hasLayers) {
            ease = shouldPlay ? Ease.OutCubic : Ease.InCubic;
        }

        audioSource.DOKill();
        return audioSource.DOFade(shouldPlay ? musicVolume : 0, duration).SetEase(ease);
    }

}
