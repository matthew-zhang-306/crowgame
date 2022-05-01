using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubSpawnHandler : MonoBehaviour
{
    public ExitPoint[] exitPoints;
    public PlayerMovement player;

    private void Start()
    {
        // int musicIndex = Managers.ScenesManager.currentSceneDef.musicIndex;
        Debug.Log("the exit is " + Managers.ScenesManager.destinationExit);
        
        if (Managers.ScenesManager.destinationExit != null && Managers.ScenesManager.destinationExit != "") {
            int exit = System.Array.FindIndex(exitPoints, 0, exitPoints.Length,
                exit => { Debug.Log(exit.exitName + " " + Managers.ScenesManager.destinationExit); return exit.exitName == Managers.ScenesManager.destinationExit; }
            );
            if (exit >= 0) {
                player.transform.position = exitPoints[exit].transform.position;
            }
        }
        // Managers.AudioManager.SetSong(Managers.ScenesManager.levelList.levelSongs[musicIndex], 0);
    }
}
