using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public LevelListSO levelList;
    public ZodiacDialogueSO zodiacSO;
    private bool[] starsCollected;

    public string newGameScene;


    public void Init() {
        starsCollected = PlayerPrefsX.GetBoolArray("StarsCollected", false, 0);
        string stars = "";
        foreach (bool star in starsCollected)
        {
            stars += star.ToString();
        }
        Debug.Log(stars);
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
    public void ResetStarsCollected() {
        starsCollected = new bool[levelList.numStars];
        PlayerPrefsX.SetBoolArray("StarsCollected", starsCollected);

        ResetSavePosition();
        ResetVisitedLevels();

        //resets the bridges
        PlayerPrefsX.SetBool("isBridgeOpened", false);
        PlayerPrefsX.SetBool("isBridgeOpenedTwo", false);
        PlayerPrefsX.SetBool("isBridgeOpenedThree", false);
    }


    public void GetSavePosition(out string sceneName, out string exitName) {
        sceneName = PlayerPrefs.GetString("SavePositionScene", newGameScene);
        exitName = PlayerPrefs.GetString("SavePositionExit", "");
    }
    public void SetSavePosition(string sceneName, string exitName = "") {
        PlayerPrefs.SetString("SavePositionScene", sceneName);
        PlayerPrefs.SetString("SavePositionExit", exitName);
    }

    [ContextMenu("Reset Save Position")]
    public void ResetSavePosition() {
        Managers.ScenesManager.destinationExit = "";
        SetSavePosition(newGameScene, "");
    }


    public bool GetLevelVisited(int level) {
        return PlayerPrefsX.GetBool("Visited" + level, false);
    }
    public void SetLevelVisited(int level, bool visited) {
        PlayerPrefsX.SetBool("Visited" + level, visited);        
    }

    public bool IsAllCollected()
    {
        for(int i = 0; i < levelList.numLevels; i++)
        {
            if ((!Managers.ProgressManager.IsStarCollected(i, 0)) || (!Managers.ProgressManager.IsStarCollected(i, 1)))
            {
                return false;
            }
        }
        return true;
    }

    [ContextMenu("Reset Visited Levels")]
    public void ResetVisitedLevels() {
        for (int i = 0; i < levelList.numLevels; i++) {
            SetLevelVisited(i, false);
            PlayerPrefsX.SetBool(i.ToString(), false);
        }
        for (int i = 0; i < zodiacSO.zodiacs.Length; i++)
        {
            zodiacSO.zodiacs[i].haveTalkedTo = false;
        }
    }
}
