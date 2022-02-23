using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoMarker : MonoBehaviour
{

    public GameObject MarkerImage;

    public void Awake()
    {
        MarkerImage.SetActive(false);
    }

    public void setTornadoMarker(Vector3 position)
    {
        MarkerImage.transform.position = position; 
    }

    public void activateMarker()
    {
        MarkerImage.SetActive(true);
    }

    public void deactivateMarker()
    {
        MarkerImage.SetActive(false);
    }

}
