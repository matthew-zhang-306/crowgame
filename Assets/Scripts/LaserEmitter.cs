using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    
    public LineRenderer lineRenderer; // this object positioned where the laser starts
    private int hitMask;

    public float maxDistance;


    private void Start() {
        hitMask = LayerMask.GetMask("Wall", "Box", "Player");
    }


    private void FixedUpdate() {
        float distance = maxDistance;

        // determine how far the laser goes by raycasting
        if (Physics.BoxCast(
            lineRenderer.transform.position,
            Vector3.one * lineRenderer.startWidth,
            lineRenderer.transform.right,
            out RaycastHit hit,
            Quaternion.identity,
            distance,
            hitMask
        )) {
            distance = hit.distance;   
            if (hit.collider.CompareTag("Player")) {
                // player is getting hit with a laser
                Debug.Log("you've been Lasered");
            }
        }

        // have the line extend forward the right length
        lineRenderer.SetPosition(1, Vector3.right * distance);
    }
}
