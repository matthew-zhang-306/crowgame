using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeActivate : MonoBehaviour
{
    public GameObject bridgeCam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeactivateCam()
    {
        bridgeCam.SetActive(false);
    }
}
