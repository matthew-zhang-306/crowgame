using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoMarker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void setTornadoMarker(Vector3 position)
    {

        //TornadoMarker.SetActive(true);
        //Debug.Log(TornadoMarker.active);
        transform.position = position;
        //Debug.Log("Setting the position of marker");

    }

    public void unsetTornadoMarker()
    {
        //TornadoMarker.SetActive(false);
    }
}
