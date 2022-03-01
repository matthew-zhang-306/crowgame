using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera PlayerCam;
    [SerializeField] private CinemachineVirtualCamera InteriorCam;
    private bool isInterior;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SwitchPriority()
    {
        if (!isInterior)
        {
            PlayerCam.Priority = 1;
            InteriorCam.Priority = 0;
        }
        else
        {
            PlayerCam.Priority = 0;
            InteriorCam.Priority = 1;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "InteriorTrigger")
        {
            isInterior = true;
            SwitchPriority();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "InteriorTrigger")
        {
            isInterior = false;
            SwitchPriority();
        }
    }

}
