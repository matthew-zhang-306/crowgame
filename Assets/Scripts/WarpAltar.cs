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

    public Vector3 playerSpawnPosOffset;
    public Vector3 PlayerPosition => transform.position + playerSpawnPosOffset;
    private PlayerMovement playerInside;
    private bool isWarping;

    public delegate void AltarDelegate(WarpAltar altar);
    public static AltarDelegate OnAltarEnter;
    public static AltarDelegate OnAltarExit;
    public static AltarDelegate OnAltarWarp;

    private WarpAltarRock[] rocks;

    private void Awake() {
        rocks = GetComponentsInChildren<WarpAltarRock>();
    }


    private void Update() {
        if (!isWarping && !PauseMenu.isTelescopeOn && !PauseMenu.gamePaused && playerInside != null && Input.GetAxisRaw("Action1") > 0) {
            if (Managers.ScenesManager.IsPuzzleSceneLoaded())
            {
                GameObject puzzleControlGo = GameObject.FindGameObjectWithTag("PuzzleDialogue");
                PuzzleDialogueControl puzzleControlScript = puzzleControlGo.GetComponent<PuzzleDialogueControl>();
                if (!puzzleControlScript.triggeredDialogue)
                {
                    puzzleControlScript.triggeredDialogue = true;
                    puzzleControlScript.ExitPuzzle();
                }
            }
            else
            {
                isWarping = true;
                OnAltarWarp?.Invoke(this);
                DOTween.Sequence().InsertCallback(
                    1.0f, () => Managers.ScenesManager.ChangeScene(targetLevel.sceneName));
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            OnAltarEnter?.Invoke(this);
            playerInside = other.GetComponent<PlayerMovement>();

            string WarpButton = "Space";
            #if UNITY_WSA
                //set xbox version of text
                WarpButton = "A";
            #endif
            playerInside.actionIndicator.Show(WarpButton, "Warp to " + targetLevel.displayName);

            foreach (var rock in rocks) {
                rock.IsLifted = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            OnAltarExit?.Invoke(this);
            playerInside.actionIndicator.Hide();
            playerInside = null;
            foreach (var rock in rocks) {
                rock.IsLifted = false;
            }
        }
    }


    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(PlayerPosition, 0.2f);
    }
}
