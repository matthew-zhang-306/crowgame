using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "SOs/Level List")]
public class LevelListSO : ScriptableObject
{
    public int starsPerLevel;
    public int mainHubIndex;
    public HubDef[] hubs;
    public LevelDef[] levels;

    public int numLevels => levels.Length;
    public int numStars => levels.Length * starsPerLevel;

    public SongSO[] levelSongs;
}

[Serializable]
public class HubDef : SceneDef
{
    // currently doesn't need anything extra
}

[Serializable]
public class LevelDef : SceneDef
{
    public string zodiacAnimal;
    public int difficulty;
    public int hubIndex;
}

[Serializable]
public class SceneDef
{
    public string sceneName;
    public string displayName;
    public int musicIndex;
}   