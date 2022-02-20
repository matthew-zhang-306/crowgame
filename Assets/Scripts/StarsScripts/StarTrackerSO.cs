using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListOfLevels", menuName = "SOs/ListOfLevelsSO")]
public class StarTrackerSO : ScriptableObject
{
    //public Levels hub;
    public Levels[] levels;
}
[Serializable]
public class Levels
{
    public string sceneName;
    public string displayName;
    public string zodiacAnimal;
    public int sceneIndex;
    //public int difficulty;
    //public int starOneCollected;
    //public int starTwoCollected;
    public int[] starsCollected;

}
