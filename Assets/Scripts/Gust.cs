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
    public GameObject Player;
    public GameObject tornadoMarker;

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

            if (Vector3.Distance(startPosition, transform.position) > maxDistance) {
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
        Vector3 markerSize = tornadoMarker.GetComponent<TornadoMarker>().MarkerImage.transform.localScale;
        Vector3 thePosition = transform.position;

        //use player from scene, not prefab
        GameObject playerInScene = GameObject.Find("Player");
        bool rayHit = playerInScene.GetComponent<PlayerMovement>().boxCastHit;
        GameObject wallCollider = GameObject.Find("wallCollider");
        bool playerColliding = wallCollider.GetComponent<WallCollisionCheck>().colliding;

        if (hitSurface != null && hitSurface.layer == LayerMask.NameToLayer("Box")) {
            // want to spawn the tornado inside the box
            thePosition = hitSurface.transform.position;
            
        }

        else if(rayHit)
        {
            Vector3 backward = -1 * transform.forward;
            
            thePosition = thePosition + Vector3.Scale(backward, tornadoPrefab.transform.localScale / 2)
             + Vector3.Scale(transform.forward, markerSize / 2);

            Debug.Log("boxcast hit");
        }
        else if (!playerColliding)
        {

            //acount for size of tornado prefab
            thePosition = thePosition + Vector3.Scale(transform.forward, tornadoPrefab.transform.localScale / 2);
            Debug.Log("boxcast not hit");
        }

        GameObject.Instantiate(tornadoPrefab, thePosition, Quaternion.identity, null);
        Destroy(gameObject);
    }

}
