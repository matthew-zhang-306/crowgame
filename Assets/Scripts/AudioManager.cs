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
    public enum MusicState {
        UNLOADED,
        STOPPED,
        FADING,
        PLAYING
    }
    private MusicState state = MusicState.UNLOADED;
    

    public AudioClip[] sounds;
    AudioSource audioSource;

    public GameObject musicObj;
    public float musicVolume;
    private List<AudioSource> musicSources;

    private bool[] shouldClipPlay;
    private SongSO currentSong;
    private int currentVersion;
    private SongSO nextSong;
    private int nextVersion;


    public void Init() {
        audioSource = GetComponent<AudioSource>();
        musicSources = musicObj.GetComponents<AudioSource>().ToList();
    }


    private void Update() {
        // loop music
        if (musicSources.Count > 0 && musicSources[0].isPlaying && musicSources[0].time >= currentSong.loopEndTime) {
            float currentTime = musicSources[0].time;
            foreach (AudioSource s in musicSources) {
                s.time = currentTime - (currentSong.loopEndTime - currentSong.loopStartTime);
            }
        }
    }


    public void PlaySound(string soundName, float volumeMultiplier = 1f) {
        // todo
        //get the sound
        AudioClip s = Array.Find(sounds, sound => sound.name == soundName);
        if (s != null)
        {
            Debug.Log("Playing " + soundName);
            audioSource.PlayOneShot(s, volumeMultiplier);
        }
        else
        {
            Debug.Log("Could not play the sound because it's not in the array!");
            Debug.Log("song name: " + soundName);
        }
            

        
    }

    public void PlaySoundAt(string soundName, Vector3 location, float volumeMultiplier = 1f) {
        // todo
    }


    public void SetSong(SongSO song, int version = 0) {
        switch (state) {
            case MusicState.PLAYING:
                if (song == currentSong) {
                    // we only need to switch versions
                    currentVersion = version;
                    SwitchVersion(currentVersion);
                }
                else {
                    // queue up this song to be played next
                    nextSong = song;
                    nextVersion = version;
                    StopSong();
                }
                break;
            case MusicState.FADING:
                if (song == currentSong) {
                    // stop the current fade prematurely and allow the current song to continue
                    // (but if there is a version change we need to swap that over)
                    currentVersion = version;
                    SwitchVersion(currentVersion);
                    PlaySong(song);
                }
                else {
                    // change the song that is currently queued
                    nextSong = song;
                    nextVersion = version;
                }
                break;
            case MusicState.STOPPED:
            case MusicState.UNLOADED:
                currentVersion = version;
                PlaySong(song);
                break;
        }
    }

    private void PlaySong(SongSO song, float fadeInTime = 2f) {
        if (song.versions.Length == 0) {
            Debug.LogError("Trying to play a song that doesn't have any primary audio!");
            return;
        }
        
        if (song == currentSong) {
            state = MusicState.PLAYING;

            // fade the current song back in
            var seq = DOTween.Sequence();
            for (int m = 0; m < currentSong.GetNumClips(); m++) {
                musicSources[m].DOKill();
                seq.Insert(0, GetFadeTween(musicSources[m], shouldClipPlay[m], fadeInTime, false));
            }
            seq.SetTarget(this).SetLink(gameObject);
        }
        else {
            state = MusicState.PLAYING;

            // perform an immediate song switch
            currentSong = song;

            // ensure we have enough audio sources for this song
            while (musicSources.Count < song.GetNumClips()) {
                // create audio sources
                AudioSource s = musicObj.AddComponent<AudioSource>();
                s.outputAudioMixerGroup = musicSources[0].outputAudioMixerGroup;
                musicSources.Add(s);
            }

            // force stop all existing audio sources
            foreach (AudioSource musicSource in musicSources) {
                if (musicSource.isPlaying)
                    musicSource.Stop();
                musicSource.volume = 0;
                musicSource.time = 0;
            }

            // set whether clips should be playing
            shouldClipPlay = new bool[currentSong.GetNumClips()];
            shouldClipPlay[currentVersion] = true;

            // switch clips to the current song and set everything playing
            for (int m = 0; m < song.GetNumClips(); m++) {
                musicSources[m].clip = song.GetClip(m);
                musicSources[m].volume = shouldClipPlay[m] ? musicVolume : 0;
                musicSources[m].Play();
            }
        }
    }

    public void StopSong(float fadeOutTime = 2f) {
        if (state != MusicState.PLAYING)
            return;
        state = MusicState.FADING;

        // fade out all of the music
        // note that this doesn't actually stop running the audio sources, so that the song can fade back in if we want
        var seq = DOTween.Sequence();
        for (int m = 0; m < currentSong.GetNumClips(); m++) {
            musicSources[m].DOKill();
            seq.Insert(0, GetFadeTween(musicSources[m], false, fadeOutTime, false));
        }
        seq.SetTarget(this).SetLink(gameObject);
        
        seq.OnComplete(() => {
            // check if there is a next song we should switch to
            if (nextSong != null) {
                currentVersion = nextVersion;
                PlaySong(nextSong);
                nextSong = null;
            }
            else {
                state = MusicState.STOPPED;
            }
        });
    }

    public void SwitchVersion(int version, float fadeTime = 2f) {
        if (state == MusicState.UNLOADED)
            return;

        // set the version in the shouldClipPlay list
        for (int v = 0; v < currentSong.versions.Length; v++) {
            shouldClipPlay[v] = v == version;
        }

        // don't change volumes of any audio sources if we're not supposed to be playing music
        if (state != MusicState.PLAYING)
            return;
        
        var seq = DOTween.Sequence();
        for (int v = 0; v < currentSong.versions.Length; v++) {
            musicSources[v].DOKill();
            seq.Insert(0, GetFadeTween(musicSources[v], v == version, fadeTime, true));
        }
    }

    public void EnableLayer(int layer, float fadeTime = 1f) {
        if (state == MusicState.UNLOADED)
            return;
        
        // set the layer in the shouldClipPlay list
        shouldClipPlay[currentSong.versions.Length + layer] = true;

        // don't change volumes of any audio sources if we're not supposed to be playing music
        if (state != MusicState.PLAYING)
            return;
        
        AudioSource source = musicSources[currentSong.versions.Length + layer];
        source.DOKill();
        GetFadeTween(source, true, fadeTime, true);
    }

    public void DisableLayer(int layer, float fadeTime = 1f) {
        if (state == MusicState.UNLOADED)
            return;
        
        // set the layer in the shouldClipPlay list
        shouldClipPlay[currentSong.versions.Length + layer] = false;

        // don't change volumes of any audio sources if we're not supposed to be playing music
        if (state != MusicState.PLAYING)
            return;

        AudioSource source = musicSources[currentSong.versions.Length + layer];
        source.DOKill();
        GetFadeTween(source, false, fadeTime, true);
    }


    private Tween GetFadeTween(AudioSource audioSource, bool shouldPlay, float duration, bool hasLayers) {
        Ease ease = Ease.Linear;
        if (hasLayers) {
            ease = shouldPlay ? Ease.OutQuad : Ease.InQuad;
        }

        audioSource.DOKill();
        return audioSource.DOFade(shouldPlay ? musicVolume : 0, duration).SetEase(ease);
    }

}
