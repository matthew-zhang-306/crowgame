using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavingSystem : MonoBehaviour
{
    public GameObject player;
    public Transform warpAltarContainer;


    // Start is called before the first frame update
    void Start()
    {
        if (Managers.ScenesManager.IsHubSceneLoaded() && Managers.ProgressManager.GetPreviousLevel() >= 0) {
#if UNITY_EDITOR
            if (!Managers.ScenesManager.HasChangedScenes) {
                // nine times out of ten, when we press play in the unity editor, we don't want the player to spawn at their previous save
                // so we avoid doing so
                return;
            }
#endif

            // warp to the corresponding altar
            Transform warp = warpAltarContainer.GetChild(Managers.ProgressManager.GetPreviousLevel());
            player.transform.position = warp.GetComponent<WarpAltar>().PlayerPosition;
        }
    }

    public void SavePlayerPosition()
    {
        PlayerPrefsX.SetVector3("playerPos", player.transform.position);
        Debug.Log("Saved");
        Debug.Log("Player Pos: " + player.transform.position.x + ", " + player.transform.position.y + ", " + player.transform.position.z);
    }

    public void SetPlayerPosition()
    {
        // todo: get camera to center on player immediately
        player.transform.position = PlayerPrefsX.GetVector3("playerPos", player.transform.position);
        Debug.Log("Moved");
        Debug.Log("Player Pos: " + player.transform.position.x + ", " + player.transform.position.y + ", " + player.transform.position.z);
    }
}
