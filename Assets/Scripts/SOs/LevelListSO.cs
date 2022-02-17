using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "SOs/Level List")]
public class LevelListSO : ScriptableObject
{
    public int starsPerLevel;
    public LevelDef hub;
    public LevelDef[] levels;

    public int numLevels => levels.Length;
    public int numStars => levels.Length * starsPerLevel;
}

[Serializable]
public class LevelDef
{
    public string sceneName;
    public string displayName;
    public string zodiacAnimal;
    public int difficulty;
}