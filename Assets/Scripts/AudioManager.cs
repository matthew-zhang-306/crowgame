using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

// responsible for managing sound effects and music
// sound effects can be played by name and music can persist through scene transitions


public class AudioManager : MonoBehaviour
{
    public AudioClip[] sounds;
    AudioSource audioSource;

    public void Init() {
        // do stuff here
        
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void Update() {
        // loop music, destroy sound game objects, etc
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

    public void PlayMusic(string musicName, float fadeInTime = 0f) {
        // todo
    }

    public void StopMusic(string musicName, float fadeOutTime = 1f) {
        // todo
    }

}
