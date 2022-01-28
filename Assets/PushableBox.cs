using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox : MonoBehaviour
{
    public float maxPushSpeed;
    public float gravity;
    private float currentGravity;

    public Transform selfRide;

    Vector3 intendedPosition;
    new Rigidbody rigidbody;
    new Collider collider;
    int wallMask;

    Vector3 groundNormal;
    float groundY;

    Transform currentRide;
    Tornado currentTornado;
    

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        wallMask = LayerMask.GetMask("Wall", "Box");

        intendedPosition = transform.position;
    }


    private void FixedUpdate() {
        // align x and z to anything that the box is riding
        if (currentRide != null) {
            intendedPosition = currentRide.position.WithY(intendedPosition.y);
        }
        else if (currentTornado != null) {
            intendedPosition = currentTornado.transform.position.WithY(intendedPosition.y);
        }

        // check for ground
        groundNormal = Vector3.zero;
        groundY = 0f;
        if (Physics.BoxCast(collider.bounds.center + Vector3.up * 0.1f, collider.bounds.extents - Vector3.one * 0.03f, Vector3.down, out RaycastHit hit, Quaternion.identity, 0.25f, wallMask)) {
            Debug.DrawRay(transform.position, hit.normal * 2f, Color.red, Time.fixedDeltaTime);
            groundY = hit.point.y;
            groundNormal = hit.normal;
        }

        // figure out where the box should go vertically
        if (currentTornado != null && currentRide == null) {
            intendedPosition.y = currentTornado.Top.y;
        }
        else if (groundNormal == Vector3.zero) {
            intendedPosition.y -= currentGravity * Time.deltaTime;
            currentGravity += gravity * Time.deltaTime;
        }
        else {
            intendedPosition.y = groundY + 0.01f;
            currentGravity = 0;
        }

        Vector3 move = intendedPosition - transform.position;
        move = Vector3.ClampMagnitude(move, maxPushSpeed * Time.deltaTime);
        rigidbody.velocity = move / Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Peck") && groundNormal != Vector3.zero) {
            Vector3 pushDirection = other.transform.forward;
            Debug.DrawRay(collider.bounds.center, other.transform.forward * 1f, Color.yellow, 0.2f);
            if (!Physics.BoxCast(collider.bounds.center, Vector3.one * 0.4f, pushDirection, out RaycastHit hit, Quaternion.identity, 1f, wallMask)) {
                // move
                intendedPosition += pushDirection;
                currentRide = null;
            }
        }

        if (other.CompareTag("Ride") && other.transform != selfRide) {
            currentRide = other.transform;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.transform == currentRide) {
            currentRide = null;
        }
    }


    public void EnterTornado(Tornado tornado) {
        currentTornado = tornado;
    }

    public void ExitTornado() {
        currentTornado = null;
    }


    private void OnDrawGizmos() {
        if (rigidbody != null) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(intendedPosition, 0.2f);
        }
    }
}
