using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gust : MonoBehaviour
{
    private static Vector3 GRID_SIZE = new Vector3(1, 0, 1);

    [Header("Parameters")]
    public float moveSpeed;
    public float maxDistance;

    [Header("References")]
    public GameObject tornadoPrefab;

    private PlayerMovement player;
    private TornadoMarker tornadoMarker;

    Vector3 startPosition;
    int wallMask;
    int wallNoBoxMask;
    bool isMoving;


    private void Awake() {
        wallMask = LayerMask.GetMask("Wall", "Box");
        wallNoBoxMask = LayerMask.GetMask("Wall");
        isMoving = true;
        startPosition = transform.position;
    }

    // note: i'm not sure whether SetPlayer gets called before or after Awake
    public void SetPlayer(PlayerMovement player) {
        this.player = player;
        tornadoMarker = player.tornadoMarker;
    }


    private void FixedUpdate() {
        if (isMoving) {
            // check the grid position in front of us
            Vector3 positionInFront = transform.position + transform.forward * 0.6f;
            Vector3 gridPositionInFront = positionInFront.RoundToNearest(GRID_SIZE);
            var colliders = Physics.OverlapBox(gridPositionInFront, Vector3.one * 0.3f, Quaternion.identity, wallNoBoxMask);
            if (colliders.Length > 0) {
                // there's stuff in the way in front of us. let's not go further
                Stop(colliders[0].gameObject);
            }

            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(startPosition, transform.position) >= maxDistance) {
                // gust has gone as far as it should. snap position to exactly maxDistance away from the start position and stop
                transform.position = startPosition + Vector3.ClampMagnitude(transform.position - startPosition, maxDistance);
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
        Vector3 markerSize = tornadoMarker.MarkerImage.transform.localScale;
        Vector3 thePosition = transform.position;

        //use player from scene, not prefab
        bool rayHit = player.boxCastHit;

        if (hitSurface != null && hitSurface.layer == LayerMask.NameToLayer("Box")) {
            Debug.Log("tornado hit box");
            // want to spawn the tornado inside the box
            thePosition = hitSurface.transform.position;
            
        }

        thePosition = thePosition.RoundToNearest(GRID_SIZE);
        // thePosition += new Vector3(0, 0.1f, 0);

        GameObject.Instantiate(tornadoPrefab, thePosition, Quaternion.identity, null);
        Destroy(gameObject);
    }


    // Given an initial position and a direction, figure out where the tornado will end up if a gust was launched out.
    // Returns Vector3.positiveInfinity if there is no place to put the tornado
    // this is equivalent to what the actual gust object is doing, but all of the checks happen in one frame
    public static Vector3 CalculateTornadoPlacement(Vector3 gustPosition, Vector3 direction, float maxDistance, int wallMask, out bool isValid) {
        Collider[] colliders = null;
        Vector3 thePosition = gustPosition;
        isValid = true;

        // guard against infinite loops
        int numLoops = 0;

        // check in front of the gust each half unit until we reach maximum distance
        while (Vector3.Distance(thePosition, gustPosition) < maxDistance) {
            numLoops++;
            if (numLoops > 100) {
                Debug.LogError("Infinite loop detected in CalculateTornadoPlacement(" + gustPosition + ", " + direction + ", " + maxDistance + ")");
                isValid = false;
                break;
            }

            Vector3 nextPosition = thePosition + direction * 0.5f;
            nextPosition = nextPosition.RoundToNearest(GRID_SIZE);

            // check if the next position is open
            colliders = Physics.OverlapBox(nextPosition, Vector3.one * 0.3f, Quaternion.identity, wallMask);
            if (colliders.Length > 0) {
                // there's stuff in the way, so the gust can't move forward
                break;
            }

            // good to go
            thePosition += direction * 0.5f;
        }

        if (Vector3.Distance(thePosition, gustPosition) > maxDistance) {
            // ensure thePosition isn't too far away
            thePosition = gustPosition + direction * maxDistance;
        }
        thePosition = thePosition.RoundToNearest(GRID_SIZE);

        // do one more final check to see if thePosition is actually open
        colliders = Physics.OverlapBox(thePosition, Vector3.one * 0.3f, Quaternion.identity, wallMask);
        if (colliders.Length > 0) {
            // we can't put a tornado here unfortunately
            isValid = false;
        }

        return thePosition;
    }

}
