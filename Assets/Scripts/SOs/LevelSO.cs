using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "SOs/Level")]
public class LevelSO : ScriptableObject
{
    public string sceneName;
    public string displayName;
    public string zodiacAnimal;
    public int difficulty;
}


