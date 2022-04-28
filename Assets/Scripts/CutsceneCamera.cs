using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneCamera : MonoBehaviour
{
    public GameObject bridgeOneCam;
    private Animator bridgeAnim;
    public GameObject bridge;

    public GameObject bridgeTwoCam;
    private Animator bridgeAnimTwo;
    public GameObject bridgeTwo;

    public GameObject bridgeThreeCam;
    private Animator bridgeAnimThree;
    public GameObject bridgeThree;

    private void Start()
    {
        bridgeAnim = bridge.GetComponent<Animator>();
        bridgeAnimTwo = bridgeTwo.GetComponent<Animator>();
        bridgeAnimThree = bridgeThree.GetComponent<Animator>();
        bridgeOneCam.SetActive(false);
        bridgeTwoCam.SetActive(false);
        bridgeThreeCam.SetActive(false);
        bridge.SetActive(false);
        bridgeTwo.SetActive(false);
        bridgeThree.SetActive(false);
        if (PlayerPrefsX.GetBool("isBridgeOpened", false) == false)
        {
            Debug.Log("isBridgeOpened is false");
            if ((Managers.ProgressManager.IsStarCollected(0, 0) || Managers.ProgressManager.IsStarCollected(0, 1)) &&
                (Managers.ProgressManager.IsStarCollected(1, 0) || Managers.ProgressManager.IsStarCollected(1, 1))) 
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

        if (PlayerPrefsX.GetBool("isBridgeOpenedTwo", false) == false)
        {
            Debug.Log("isBridgeOpenedTwo is false");
            if ((Managers.ProgressManager.IsStarCollected(0, 0) && Managers.ProgressManager.IsStarCollected(0, 1)) &&
                (Managers.ProgressManager.IsStarCollected(1, 0) && Managers.ProgressManager.IsStarCollected(1, 1)) &&
                (Managers.ProgressManager.IsStarCollected(2, 0) && Managers.ProgressManager.IsStarCollected(2, 1)) &&
                (Managers.ProgressManager.IsStarCollected(3, 0) && Managers.ProgressManager.IsStarCollected(3, 1)) &&
                (Managers.ProgressManager.IsStarCollected(4, 0) && Managers.ProgressManager.IsStarCollected(4, 1)))
            {
                bridgeTwo.transform.localScale = new Vector3(0, 0, 0);
                Invoke("TurnOnCamTwo", 1f);
                bridgeTwo.SetActive(true);
                bridgeAnimTwo.Play("animateBridge");
                PlayerPrefsX.SetBool("isBridgeOpenedTwo", true);
                Debug.Log("OpenBridgeTwo");
            }
        }
        else
        {
            bridgeTwo.SetActive(true);
        }

        if (PlayerPrefsX.GetBool("isBridgeOpenedThree", false) == false)
        {
            Debug.Log("isBridgeOpenedThree is false");
            if (Managers.ProgressManager.IsAllCollected())
            {
                bridgeThree.transform.localScale = new Vector3(0, 0, 0);
                Invoke("TurnOnCamThree", 1f);
                bridgeThree.SetActive(true);
                bridgeAnimThree.Play("animateBridge");
                PlayerPrefsX.SetBool("isBridgeOpenedThree", true);
                Debug.Log("OpenBridgeThree");
            }
        }
        else
        {
            bridgeThree.SetActive(true);
        }
    }

    private void TurnOnCam()
    {
        bridgeOneCam.SetActive(true);
    }

    private void TurnOnCamTwo()
    {
        bridgeTwoCam.SetActive(true);
    }

    private void TurnOnCamThree()
    {
        bridgeThreeCam.SetActive(true);
    }

}
