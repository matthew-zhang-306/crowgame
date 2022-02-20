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
}

public enum SongVersion {
    CALM = 0,
    TENSE = 1,
    CALMMENU = 2,
    TENSEMENU = 3
}