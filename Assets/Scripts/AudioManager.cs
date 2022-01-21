using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// responsible for managing sound effects and music
// sound effects can be played by name and music can persist through scene transitions
public class AudioManager : MonoBehaviour
{
    public void Init() {
        // do stuff here
    }


    private void Update() {
        // loop music, destroy sound game objects, etc
    }


    public void PlaySound(string soundName, float volumeMultiplier = 1f) {
        // todo
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
