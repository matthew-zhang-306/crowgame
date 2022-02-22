using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WarpAltar : MonoBehaviour
{
    public LevelListSO levelList;

    [Tooltip("-1 = Hub, 0 = First level, 1 = Second level, ...")]
    public int levelIndex;

    public LevelDef targetLevel => levelIndex >= 0 ? levelList.levels[levelIndex] : levelList.hub;

    public SavingSystem savingSystem;
    private PlayerMovement playerInside;

    public delegate void AltarDelegate(WarpAltar altar);
    public static AltarDelegate OnAltarEnter;
    public static AltarDelegate OnAltarExit;
    public static AltarDelegate OnAltarWarp;

    private WarpAltarRock[] rocks;


    private void Awake() {
        rocks = GetComponentsInChildren<WarpAltarRock>();
    }


    private void Update() {
        if (playerInside != null && Input.GetAxisRaw("Action1") > 0) {
            OnAltarWarp?.Invoke(this);
            /*saves the data to bool only if they use the warp altar
            for (int i = 0; i < Managers.ProgressManager.starTracker.levels.Length; i++)
            {
                if (Managers.ScenesManager.GetSceneName() == Managers.ProgressManager.starTracker.levels[i].sceneName)
                {
                    for (int starIndex = 0; starIndex < 2; starIndex++)
                    {
                        if (Managers.ProgressManager.starTracker.levels[i].starsCollected[starIndex] == 1)
                        {
                            Managers.ProgressManager.SetStarCollected(i, starIndex, true);
                            Debug.Log("Collected: Scene: " + i + " Star: " + starIndex);
                            Debug.Log(PlayerPrefsX.GetBoolArray("starsCollected"));
                        }
                    }
                }
            }*/
            
            if (Managers.ScenesManager.IsHubSceneLoaded()) {
                savingSystem.SavePlayerPosition();
            }

            DOTween.Sequence().InsertCallback(
                1.0f, () => Managers.ScenesManager.ChangeScene(targetLevel.sceneName));
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            OnAltarEnter?.Invoke(this);
            playerInside = other.GetComponent<PlayerMovement>();
            foreach (var rock in rocks) {
                rock.IsLifted = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            OnAltarExit?.Invoke(this);
            playerInside = null;
            foreach (var rock in rocks) {
                rock.IsLifted = false;
            }
        }
    }

    public void SceneTrans(string target)
    {
        Managers.PauseMenu.Resume();
        OnAltarWarp?.Invoke(this);
        DOTween.Sequence().InsertCallback(
            1.0f, () => Managers.ScenesManager.ChangeScene(target));
    }
}
