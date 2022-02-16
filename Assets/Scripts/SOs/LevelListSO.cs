using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "SOs/Level List")]
public class LevelListSO : ScriptableObject
{
    public int starsPerLevel;
    public LevelSO[] levels;

    public int numLevels => levels.Length;
    public int numStars => levels.Length * starsPerLevel;
}