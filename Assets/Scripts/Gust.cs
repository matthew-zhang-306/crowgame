using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gust : MonoBehaviour
{
    [Header("Parameters")]
    public float moveSpeed;
    public float maxDistance;

    [Header("References")]
    public GameObject tornadoPrefab;
    public GameObject WorldCanvas;
    public GameObject Player;
    public GameObject Visual;

    Vector3 startPosition;
    int wallMask;
    bool isMoving;
    private static bool isCalcGust = false;

    private void Awake() {
        wallMask = LayerMask.GetMask("Wall", "Box");
        isMoving = true;
        startPosition = transform.position;
        WorldCanvas = GameObject.Instantiate(WorldCanvas, Vector3.zero, Quaternion.identity);

        isCalcGust = !isCalcGust; //every other will be a calcGust
        Debug.Log("Is a Calc Gust = " +  isCalcGust.ToString());
        if (isCalcGust)
        {
            Visual.SetActive(false);
            moveSpeed = 50f;
        }
    }


    private void FixedUpdate() {
        if (isMoving) {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(startPosition, transform.position) >= maxDistance) {
                Stop(null);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!isMoving)
            return;
        
        if (Helpers.LayerMaskContains(wallMask, other.gameObject.layer)) {
            Stop(other.gameObject);
        }
    }


    private void Stop(GameObject hitSurface) {
        isMoving = false;
        
        
        Vector3 thePosition = transform.position;
        if (hitSurface != null && hitSurface.layer == LayerMask.NameToLayer("Box")) {
            // want to spawn the tornado inside the box
            thePosition = hitSurface.transform.position;
        }
        else {
            thePosition = new Vector3(Helpers.RoundToNearest(thePosition.x, 1f), thePosition.y, Helpers.RoundToNearest(thePosition.z, 1f));
        }

        if (!isCalcGust)
        {
            //now we send the tornado
            Debug.Log("spawning tornado");
            GameObject.Instantiate(tornadoPrefab, thePosition, Quaternion.identity, null);
        }
        else
        {
            //this gust was sent to see where the marker should go
            Debug.Log("Spawning marker");
            if(WorldCanvas != null)
            {
                WorldCanvas.GetComponent<worldCanvas>().setTornadoMarker(thePosition);
            }
            
        }
        Debug.Log("Destroying gust");
        Destroy(gameObject);
    }

}
