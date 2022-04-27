using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSongPlayer : MonoBehaviour
{
    private void Start()
    {
        LevelListSO levelList = Managers.ScenesManager.levelList;
        LevelDef level = Managers.ScenesManager.currentSceneDef as LevelDef;
        Managers.AudioManager.SetSong(levelList.levelSongs[level.musicIndex], 1);
    }
}
