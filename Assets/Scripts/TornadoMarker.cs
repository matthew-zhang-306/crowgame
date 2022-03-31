using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoMarker : MonoBehaviour
{

    public GameObject MarkerImage;
    int wallMask;

    public void Awake()
    {
        deactivateMarker();
        wallMask = LayerMask.GetMask("Wall");
    }

    public void UpdatePosition(PlayerMovement player)
    {
        Vector3 markerPos = Gust.CalculateTornadoPlacement(
            player.transform.position,
            player.rotateTransform.forward,
            player.tornadoSpawnDistance,
            wallMask
        );
        
        if (markerPos != Vector3.positiveInfinity) {
            MarkerImage.SetActive(true);
            MarkerImage.transform.position = markerPos.WithY(transform.position.y);
        }
        else {
            MarkerImage.SetActive(false);
        }
        
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
