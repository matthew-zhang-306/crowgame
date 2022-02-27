using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneCamera : MonoBehaviour
{
    public GameObject cutsceneCam;
    public GameObject bridge;
    private StarTrackerSO starTracker;
    // Start is called before the first frame update
    void Start()
    {
        starTracker = Managers.ProgressManager.starTracker;
        if (PlayerPrefsX.GetBool("isBridgeOpened", false) == false)
        {
            Debug.Log("isBridgeOpened is false");
            if ((starTracker.levels[0].starsCollected[0] == 1 || starTracker.levels[0].starsCollected[1] == 1) &&
                (starTracker.levels[1].starsCollected[0] == 1 || starTracker.levels[1].starsCollected[1] == 1) &&
                (starTracker.levels[2].starsCollected[0] == 1 || starTracker.levels[2].starsCollected[1] == 1))
            {
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

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
