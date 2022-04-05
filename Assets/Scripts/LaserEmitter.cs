using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    
    public Transform laserContainer;
    private int hitMask;
    private bool playingLaser;

    public float maxDistance;

    // for debugging
    private bool hasHit;
    private float lastDistance;
    private Vector3 lastHit;


    private void Start() {
        playingLaser = false;
        hitMask = LayerMask.GetMask("Wall", "Box", "Player");
    }


    private void FixedUpdate() {
        float distance = maxDistance;

        // determine how far the laser goes by raycasting
        if (Physics.BoxCast(
            GetRaycastPosition(),
            GetRaycastSize(),
            laserContainer.transform.right,
            out RaycastHit hit,
            laserContainer.rotation,
            distance,
            hitMask
        )) {
            hasHit = true;
            lastHit = hit.point;
            lastDistance = hit.distance;

            distance = hit.distance;   
            if (hit.collider.CompareTag("Player")) {
                // player is getting hit with a laser
                Debug.Log("Playing laser");
                if (!playingLaser)
                {
                    Managers.AudioManager.PlaySound("laser_hit");
                    playingLaser = true;
                }
                hit.collider.GetComponent<PlayerMovement>()?.Die();
            }
        }
        else {
            hasHit = false;
        }

        // have the line extend forward the right length
        laserContainer.transform.localScale = new Vector3(distance, 1f, 1f);
    }

    private Vector3 GetRaycastPosition() {
        return laserContainer.transform.position - laserContainer.transform.right * 0.05f;
    }

    private Vector3 GetRaycastSize() {
        return new Vector3(0.1f, 0.95f, 0.1f) / 2;
        /*
            laserContainer.transform.right * 0.1f +
            laserContainer.transform.forward * 0.1f +
            laserContainer.transform.up * 0.95f;
        */
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetRaycastPosition(), 0.2f);
        if (hasHit) {
            Gizmos.DrawWireSphere(lastHit, 0.2f);
            Gizmos.DrawWireCube(GetRaycastPosition() + laserContainer.transform.right * lastDistance, GetRaycastSize());
        }
    }
}
