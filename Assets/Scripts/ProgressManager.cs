using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public LevelListSO levelList;
    private bool[] starsCollected;


    public void Init() {
        starsCollected = PlayerPrefsX.GetBoolArray("StarsCollected", false, 0);
        if (starsCollected.Length != levelList.numStars) {
            ResetStarsCollected();
        }
    }


    public bool IsStarCollected(int levelIndex, int starIndex) {
        return starsCollected[levelIndex * levelList.starsPerLevel + starIndex];
    }

    public void SetStarCollected(int levelIndex, int starIndex, bool isCollected) {
        starsCollected[levelIndex * levelList.starsPerLevel + starIndex] = isCollected;
        PlayerPrefsX.SetBoolArray("StarsCollected", starsCollected);
    }


    [ContextMenu("Reset Stars Collected")]
    private void ResetStarsCollected() {
        starsCollected = new bool[levelList.numStars];
        PlayerPrefsX.SetBoolArray("StarsCollected", starsCollected);

        ResetPreviousLevel();
        ResetVisitedLevels();

        //resets the bridges
        PlayerPrefsX.SetBool("isBridgeOpened", false);
    }


    public int GetPreviousLevel() {
        return PlayerPrefs.GetInt("PreviousLevel", -1);
    }
    public void SetPreviousLevel(int level) {
        PlayerPrefs.SetInt("PreviousLevel", level);
    }

    [ContextMenu("Reset Previous Level")]
    public void ResetPreviousLevel() => SetPreviousLevel(-1);


    public bool GetLevelVisited(int level) {
        return PlayerPrefsX.GetBool("Visited" + level, false);
    }
    public void SetLevelVisited(int level, bool visited) {
        PlayerPrefsX.SetBool("Visited" + level, visited);        
    }

    [ContextMenu("Reset Visited Levels")]
    public void ResetVisitedLevels() {
        for (int i = 0; i < levelList.numLevels; i++) {
            SetLevelVisited(i, false);
            PlayerPrefsX.SetBool(i.ToString(), false);
        }
    }
}
