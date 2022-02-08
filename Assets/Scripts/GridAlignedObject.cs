using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAlignedObject : MonoBehaviour
{
    protected Vector3 intendedPosition;
    protected Vector3 groundNormal;
    protected float groundY;
    protected float groundDistance;
    protected Transform currentRide;
    public Transform selfRide;
    protected int wallMask;
    protected new Rigidbody rigidbody;
    protected new Collider collider;
    protected float currentGravity;
    public float gravity;
    public float maxMoveSpeed;

    public Vector3 IntendedPosition => intendedPosition;

    protected virtual void Awake() {
        wallMask = LayerMask.GetMask("Wall", "Box");
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        intendedPosition = transform.position;
    }

    protected virtual void FixedUpdate() {
        AlignIntendedPosition();
        CheckForGround();   
        SetIntendedY(); 
        MoveToIntendedPosition();
    }


    // align x and z to anything that the box is riding
    protected virtual void AlignIntendedPosition() {
        if (currentRide != null) {
            intendedPosition = currentRide.position.WithY(intendedPosition.y);
        }
        else {
            // todo: actually grid align the intended position
        }
    }

    protected virtual void CheckForGround() {
        groundNormal = Vector3.zero;
        groundY = 0f;
        if (Physics.BoxCast(collider.bounds.center + Vector3.up * 0.1f, collider.bounds.extents - Vector3.one * 0.03f, Vector3.down, out RaycastHit hit, Quaternion.identity, 0.25f, wallMask)) {
            Debug.DrawRay(transform.position, hit.normal * 2f, Color.red, Time.fixedDeltaTime);
            groundY = hit.point.y;
            groundNormal = hit.normal;
            groundDistance = hit.distance - 0.11f;
        }
    }

    protected virtual void SetIntendedY() {
        if (groundNormal == Vector3.zero) {
            intendedPosition.y -= currentGravity * Time.deltaTime;
            currentGravity += gravity * Time.deltaTime;
        }
        else {
            intendedPosition.y = groundY + 0.01f;
            currentGravity = 0;
        }
    }

    protected virtual void MoveToIntendedPosition() {
        Vector3 move = intendedPosition - transform.position;
        move = Vector3.ClampMagnitude(move, maxMoveSpeed * Time.deltaTime);
        rigidbody.velocity = move / Time.deltaTime;
        Debug.DrawRay(transform.position, rigidbody.velocity, Color.cyan, Time.fixedDeltaTime);
    }


    protected virtual void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ride") && (selfRide == null || other.transform != selfRide)) {
            currentRide = other.transform;
        }
    }
    
    protected virtual void OnTriggerExit(Collider other) {
        if (other.transform == currentRide) {
            currentRide = null;
        }
    }   

}
