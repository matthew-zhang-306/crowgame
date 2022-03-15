using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSongPlayer : MonoBehaviour
{
    private void Start()
    {
        LevelListSO levelList = Managers.ScenesManager.levelList;
        LevelDef level = levelList.levels[Managers.ScenesManager.levelNumber];
        Managers.AudioManager.SetSong(levelList.levelSongs[level.musicIndex], 1);
    }
}
