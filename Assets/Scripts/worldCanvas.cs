using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldCanvas : MonoBehaviour
{

    public GameObject TornadoMarker;

    public void Awake()
    {
        TornadoMarker.SetActive(false);
    }

    public void setTornadoMarker(Vector3 position)
    {

        TornadoMarker.SetActive(true);
        Debug.Log(TornadoMarker.activeSelf);
        TornadoMarker.transform.position = position;
        Debug.Log("Setting the position of marker");
        
    }

    public void unsetTornadoMarker()
    {
        TornadoMarker.SetActive(false);
        //Object.Destroy(TornadoMarker);
    }

}
