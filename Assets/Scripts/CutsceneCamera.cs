using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneCamera : MonoBehaviour
{
    public GameObject bridgeOneCam;
    private Animator bridgeAnim;
    public GameObject bridge;
    
    private void Start()
    {
        bridgeAnim = bridge.GetComponent<Animator>();
        bridgeOneCam.SetActive(false);
        if (PlayerPrefsX.GetBool("isBridgeOpened", false) == false)
        {
            Debug.Log("isBridgeOpened is false");
            if ((Managers.ProgressManager.IsStarCollected(0, 0) || Managers.ProgressManager.IsStarCollected(0, 1)) &&
                (Managers.ProgressManager.IsStarCollected(1, 0) || Managers.ProgressManager.IsStarCollected(1, 1)) &&
                (Managers.ProgressManager.IsStarCollected(2, 0) || Managers.ProgressManager.IsStarCollected(2, 1))
            ) 
            {
                bridge.transform.localScale = new Vector3(0, 0, 0);
                Invoke("TurnOnCam", 0.5f);
                bridge.SetActive(true);
                bridgeAnim.Play("animateBridge");
                PlayerPrefsX.SetBool("isBridgeOpened", true);
                Debug.Log("OpenBridge");
            }
        }
        else
        {
            bridge.SetActive(true);
        }
    }

    private void TurnOnCam()
    {
        bridgeOneCam.SetActive(true);
    }
    
}
