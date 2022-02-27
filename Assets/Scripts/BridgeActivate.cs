using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeActivate : MonoBehaviour
{
    public GameObject bridge;
    private Animator bridgeAnim;
    // Start is called before the first frame update
    void Start()
    {
        bridgeAnim = bridge.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayBridgeAnimation()
    {
        bridge.transform.localScale = new Vector3(0, 0, 0);
        bridge.SetActive(true);
        bridgeAnim.Play("animateBridge");
    }

    public void DeactivateCam()
    {
        this.gameObject.SetActive(false);
    }
}
