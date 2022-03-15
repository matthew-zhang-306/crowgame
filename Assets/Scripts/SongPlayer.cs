using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour
{
    public SongSO song;
    public bool shouldPlayOnStart;

    private void Start() {
        if (shouldPlayOnStart) {
            PlaySong();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PlaySong();
        }
    }


    private void PlaySong() {
        if (song != null) {
            Managers.AudioManager.SetSong(song);
        }
        else {
            Managers.AudioManager.StopSong();
        }
    }
}
