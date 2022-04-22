using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WarpAltar : MonoBehaviour
{
    public LevelListSO levelList;

    public bool isTargetPuzzle;
    public int targetLevelIndex;
    public string targetExitName;

    public ExitPoint exitPoint;

    public SceneDef targetLevel { get {
        if (Managers.ScenesManager.IsPuzzleSceneLoaded()) {
            // this warp pad is in a puzzle, so it needs to go back to its corresponding hub
            return levelList.hubs[levelList.levels[Managers.ScenesManager.levelNumber].hubIndex];
        }
        else {
            // use the level index that we specified
            if (isTargetPuzzle) {
                return levelList.levels[targetLevelIndex];
            }
            else {
                return levelList.hubs[targetLevelIndex];
            }
        }
    }}

    private PlayerMovement playerInside;
    private bool isWarping;

    public delegate void AltarDelegate(WarpAltar altar);
    public static AltarDelegate OnAltarEnter;
    public static AltarDelegate OnAltarExit;
    public static AltarDelegate OnAltarWarp;

    private WarpAltarRock[] rocks;

    private void Awake() {
        rocks = GetComponentsInChildren<WarpAltarRock>();
        if (isTargetPuzzle) {
            // the exit point linked to this warp altar should be named based on the level number that it targets
            exitPoint.exitName = targetLevelIndex + "";
        }
    }

    private void Start() {
        if (Managers.ScenesManager.IsPuzzleSceneLoaded()) {
            // have target exit match the level
            targetLevelIndex = (Managers.ScenesManager.currentSceneDef as LevelDef).hubIndex;
            targetExitName = Managers.ScenesManager.levelNumber + "";
        }
    }


    private void Update() {
        if (!isWarping && !PauseMenu.isTelescopeOn && !PauseMenu.gamePaused && playerInside != null && Input.GetAxisRaw("Action1") > 0) {
            if (Managers.ScenesManager.IsPuzzleSceneLoaded())
            {
                GameObject puzzleControlGo = GameObject.FindGameObjectWithTag("PuzzleDialogue");
                puzzleControlGo.GetComponent<PuzzleEndController>().ExitPuzzle(Warp);
            }
            else
            {
                Warp();
            }
        }
    }

    public void Warp() {
        isWarping = true;
        OnAltarWarp?.Invoke(this);

        Managers.ScenesManager.SetDestinationExit(targetExitName);        
        DOTween.Sequence().InsertCallback(
            1.0f, () => Managers.ScenesManager.ChangeScene(targetLevel.sceneName)
        );
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


}
