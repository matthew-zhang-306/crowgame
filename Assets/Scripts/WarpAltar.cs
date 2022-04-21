using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WarpAltar : MonoBehaviour
{
    public LevelListSO levelList;

    [Tooltip("-1 = Hub, 0 = First level, 1 = Second level, ...")]
    public int levelIndex;
    public bool isInAHubAndGoesToAnotherHub;

    public SceneDef targetLevel { get {
        if (Managers.ScenesManager.IsPuzzleSceneLoaded()) {
            // this warp pad is in a puzzle, so it needs to go back to its corresponding hub
            return levelList.hubs[levelList.levels[levelIndex].hubIndex];
        }
        else {
            // use the level index that we specified
            if (isInAHubAndGoesToAnotherHub) {
                return levelList.hubs[levelIndex];
            }
            else {
                return levelList.levels[levelIndex];
            }
        }
    }}

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

        // note: for now, a warp altar that goes from a hub to another hub will pass -1 into SetExit.
        // this is because i'm dumb and seriously cannot think of a better way to manage exits to figure out what goes to what across scenes
        // there exist way better solutions to this than the hacky stuff i've done here but i genuinely cannot think of them
        HubSpawnHandler.SetExit(this, isInAHubAndGoesToAnotherHub ? -1 : Managers.ScenesManager.levelNumber);
        
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


    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(PlayerPosition, 0.2f);
    }
}
