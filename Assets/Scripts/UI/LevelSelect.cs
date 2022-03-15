using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelSelect : MonoBehaviour
{
    private Button button;
    
    public LevelListSO levelList;
    public int levelIndex;
    public LevelDef targetLevel => levelList.levels[levelIndex];

    public static System.Action OnLevelSelect;


    private void OnEnable()
    {
        button = GetComponent<Button>();

        if (Managers.ProgressManager.GetLevelVisited(levelIndex))
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void PlayLevel() {
        Managers.PauseMenu.Resume();
        OnLevelSelect?.Invoke();
        DOTween.Sequence().InsertCallback(
            1.0f, () => Managers.ScenesManager.ChangeScene(targetLevel.sceneName));
    }
}
