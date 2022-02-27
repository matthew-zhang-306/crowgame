using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneCamera : MonoBehaviour
{
    public GameObject cutsceneCam;
    public GameObject bridge;
    
    private void Start()
    {
        if (PlayerPrefsX.GetBool("isBridgeOpened", false) == false)
        {
            Debug.Log("isBridgeOpened is false");
            if ((Managers.ProgressManager.IsStarCollected(0, 0) || Managers.ProgressManager.IsStarCollected(0, 1)) &&
                (Managers.ProgressManager.IsStarCollected(1, 0) || Managers.ProgressManager.IsStarCollected(1, 1)) &&
                (Managers.ProgressManager.IsStarCollected(2, 0) || Managers.ProgressManager.IsStarCollected(2, 1))
            ) {
                cutsceneCam.SetActive(true);
                PlayerPrefsX.SetBool("isBridgeOpened", true);
                Debug.Log("OpenBridge");
            }
        }
        else
        {
            bridge.SetActive(true);
        }
    }
    
}
