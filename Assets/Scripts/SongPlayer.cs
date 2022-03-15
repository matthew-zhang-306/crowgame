using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour
{
    public SongSO song;
    public bool shouldPlayOnStart;

    private void Start() {
        if (shouldPlayOnStart) {
            Managers.AudioManager.SetSong(song);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Managers.AudioManager.SetSong(song);
        }
    }
}
