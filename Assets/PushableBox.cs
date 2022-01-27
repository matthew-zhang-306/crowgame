using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox : MonoBehaviour
{
    public float maxPushSpeed;
    public float gravity;
    private float currentGravity;

    Vector3 intendedPosition;
    new Rigidbody rigidbody;
    new Collider collider;

    Vector3 groundNormal;
    float groundY;
    

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        intendedPosition = transform.position;
    }


    private void FixedUpdate() {
        groundNormal = Vector3.zero;
        groundY = 0f;
        if (Physics.BoxCast(collider.bounds.center + Vector3.up * 0.1f, collider.bounds.extents - Vector3.one * 0.03f, Vector3.down, out RaycastHit hit, Quaternion.identity, 0.25f)) {
            Debug.DrawRay(transform.position, hit.normal * 2f, Color.red, Time.fixedDeltaTime);
            groundY = hit.point.y;
            groundNormal = hit.normal;
        }

        if (groundNormal == Vector3.zero) {
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
            if (!Physics.BoxCast(collider.bounds.center, Vector3.one * 0.4f, pushDirection, out RaycastHit hit, Quaternion.identity, 1f)) {
                // move
                intendedPosition += pushDirection;
            }
        }
    }


    private void OnDrawGizmos() {
        if (rigidbody != null) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(intendedPosition, 0.2f);
        }
    }
}
