using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gust : MonoBehaviour
{
    [Header("Parameters")]
    public float moveSpeed;
    public float maxDistance;

    [Header("References")]
    public GameObject tornadoPrefab;

    Vector3 startPosition;
    int wallMask;
    bool isMoving;

    private void Awake() {
        wallMask = LayerMask.GetMask("Wall", "Box");
        isMoving = true;
        startPosition = transform.position;
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
        
        GameObject.Instantiate(tornadoPrefab, thePosition, Quaternion.identity, null);
        Destroy(gameObject);
    }

}
