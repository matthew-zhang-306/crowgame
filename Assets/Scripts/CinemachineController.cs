using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera PlayerCam;
    private Transform Container;
    private bool isInterior;
    private int camInput;
    private int oldCamInput;
    private int activeCam;
    // Start is called before the first frame update
    void Start()
    {
        Container = this.gameObject.transform;
        activeCam = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float camInputAnalog = Input.GetAxis("Camera");
        if (Mathf.Abs(camInputAnalog) > 0.2f)
        {
            camInput = (int)Mathf.Sign(camInputAnalog);
        }
        else
        {
            camInput = 0;
        }
    }

    private void FixedUpdate()
    {
        if (isInterior)
        {
            if (camInput != oldCamInput && camInput != 0)
            {
                //Debug.Log(camInput);
                if (camInput == -1)
                {
                    PrevCam();
                }
                if (camInput == 1)
                {
                    NextCam();
                }
            }
            oldCamInput = camInput;
        }
    }

    private void SwitchPriority()
    {
        if (!isInterior)
        {
            PlayerCam.Priority = 1;
            //InteriorCam.Priority = 0;
            activeCam = 0;
            Container.GetChild(activeCam).GetComponent<CinemachineVirtualCamera>().Priority = 0;
        }
        else
        {
            PlayerCam.Priority = 0;
            //InteriorCam.Priority = 1;
            Container.GetChild(activeCam).GetComponent<CinemachineVirtualCamera>().Priority = 1;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            isInterior = true;
            SwitchPriority();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isInterior = false;
            SwitchPriority();
        }
    }

    private void NextCam()
    {
        activeCam++;
        //Debug.Log(activeCam);
        if (activeCam >= Container.childCount)
        {
            activeCam = 0;
        }
        Container.GetChild(activeCam).GetComponent<CinemachineVirtualCamera>().Priority = 1;
        for (int i = 0; i < Container.childCount; i++)
        {
            if (i != activeCam)
            {
                Container.GetChild(i).GetComponent<CinemachineVirtualCamera>().Priority = 0;
            }
        }
    }

    private void PrevCam()
    {
        activeCam--;
        //Debug.Log(activeCam);
        if (activeCam < 0)
        {
            activeCam = Container.childCount - 1;
        }
        Container.GetChild(activeCam).GetComponent<CinemachineVirtualCamera>().Priority = 1;
        for (int i = 0; i < Container.childCount; i++)
        {
            if (i != activeCam)
            {
                Container.GetChild(i).GetComponent<CinemachineVirtualCamera>().Priority = 0;
            }
        }
    }
}
