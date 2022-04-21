using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavingSystem : MonoBehaviour
{
    public GameObject player;
    public Transform warpAltarContainer;

    /*

    // Start is called before the first frame update
    void Start()
    {
        int musicIndex = 0;

        // if there is a previous position saved we should put the player there
        if (Managers.ProgressManager.GetPreviousLevel() >= 0
            // check that there is a corresponding warp altar for this level
            && Managers.ProgressManager.GetPreviousLevel() < warpAltarContainer.childCount
#if UNITY_EDITOR
            // nine times out of ten, when we press play in the unity editor, we don't want the player to spawn at their previous save
            // so if we're in the editor we avoid doing this
            && Managers.ScenesManager.HasChangedScenes
#endif
        ) {

            // warp to the corresponding altar
            Transform warp = warpAltarContainer.GetChild(Managers.ProgressManager.GetPreviousLevel());
            player.transform.position = warp.GetComponent<WarpAltar>().PlayerPosition;
            musicIndex = Managers.ScenesManager.levelList.levels[Managers.ProgressManager.GetPreviousLevel()].musicIndex;
        }

        Managers.AudioManager.SetSong(Managers.ScenesManager.levelList.levelSongs[musicIndex], 0);
    }
    */

}
