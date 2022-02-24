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
