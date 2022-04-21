using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubSpawnHandler : MonoBehaviour
{
    public class ExitDef {
        public enum ExitType {
            UNDEFINED,
            WARP,
            LOADZONE
        }
        public ExitType exitType;
        public int id;
    }
    private static ExitDef PreviousExit;

    public WarpAltar[] warpAltars;
    public LoadingZone[] loadingZones;

    public PlayerMovement player;

    private void Start()
    {
        int musicIndex = Managers.ScenesManager.currentSceneDef.musicIndex;

        if (PreviousExit != null) {
            if (PreviousExit.exitType == ExitDef.ExitType.WARP) {
                // spawn at a warp altar
                int warpIndex = System.Array.FindIndex(warpAltars, 0, warpAltars.Length, wa => wa.levelIndex == PreviousExit.id);
                if (warpIndex >= 0) {
                    player.transform.position = warpAltars[warpIndex].PlayerPosition;
                }

                musicIndex = Managers.ScenesManager.levelList.levels[PreviousExit.id].musicIndex;
            }
            else if (PreviousExit.exitType == ExitDef.ExitType.LOADZONE) {
                // spawn at a loading zone
                int loadIndex = System.Array.FindIndex(loadingZones, 0, loadingZones.Length, lz => lz.id == PreviousExit.id);
                if (loadIndex >= 0) {
                    player.transform.position = loadingZones[loadIndex].PlayerPosition;
                    musicIndex = loadingZones[loadIndex].musicIndex;
                }
            }
        }
        PreviousExit = null;

        Managers.AudioManager.SetSong(Managers.ScenesManager.levelList.levelSongs[musicIndex], 0);
    }


    public static void SetExit(MonoBehaviour exitObject, int id) {
        ExitDef exitDef = new ExitDef();
        if (exitObject is WarpAltar warp) {
            exitDef.exitType = ExitDef.ExitType.WARP;
        }
        // if (exitObject is LoadingZone loadingZone) {
        //     exitDef.exitType = ExitDef.ExitType.LOADZONE;
        // }
        exitDef.id = id;
        PreviousExit = exitDef;
    }
}
