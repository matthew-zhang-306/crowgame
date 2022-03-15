using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Song", menuName = "SOs/Song")]
public class SongSO : ScriptableObject
{
    public float loopStartTime;
    public float loopEndTime;

    public AudioClip[] versions;
    public AudioClip[] layers;


    public AudioClip GetClip(int index) {
        if (index < 0 || index >= versions.Length + layers.Length) {
            Debug.LogError("SongSO.GetClip() index out of bounds (" + index + ")");
            return null;
        }

        if (index < versions.Length) {
            return versions[index];
        }
        else {
            return layers[index - versions.Length];
        }
    }

    public int GetNumClips() {
        return versions.Length + layers.Length;
    }
}

public enum SongVersion {
    CALM = 0,
    TENSE = 1,
    CALMMENU = 2,
    TENSEMENU = 3
}