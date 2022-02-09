using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    [Header("Physics Parameters")]
    public float friction;
    public float riseAccel;
    public float maxRiseSpeed;
    public float maxFallSpeed;
    public float gravity;
    
    protected Vector3 groundNormal;
    protected float groundY;
    protected float groundDistance;
    protected Rigidbody groundRigidbody;
    protected Vector3 previousGroundVelocity;
    protected int wallMask;
    protected new Rigidbody rigidbody;
    protected new Collider collider;
    protected float currentGravity;

    protected Tornado currentTornado;
    

    protected virtual void Awake() {
        wallMask = LayerMask.GetMask("Wall", "Box");
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    protected virtual void FixedUpdate() {
        CheckForGround();
        HandleMovement();
    }

    protected virtual void CheckForGround() {
        groundNormal = Vector3.zero;
        groundRigidbody = null;
        if (Physics.BoxCast(collider.bounds.center + Vector3.up * 0.1f, collider.bounds.extents - Vector3.one * 0.03f, Vector3.down, out RaycastHit hit, Quaternion.identity, 0.25f, wallMask)) {
            Debug.DrawRay(transform.position, hit.normal * 2f, Color.red, Time.fixedDeltaTime);

            groundY = hit.point.y;
            groundNormal = hit.normal;
            groundDistance = hit.distance - 0.11f;
        
            groundRigidbody = hit.rigidbody ?? hit.collider.GetComponentInParent<Rigidbody>();
        }
    }

    protected virtual void HandleMovement() {
        Vector3 hVelocity = rigidbody.velocity.WithY(0);
        float vVelocity = rigidbody.velocity.y;

        hVelocity -= previousGroundVelocity.WithY(0);
        vVelocity -= previousGroundVelocity.y;
        
        hVelocity = Vector3.MoveTowards(hVelocity, Vector3.zero, friction);
        if (groundNormal == Vector3.zero && currentTornado == null) {
            hVelocity = Vector3.zero;
        }

        if (currentTornado != null) {
            vVelocity += riseAccel * Time.deltaTime;
            vVelocity = Mathf.Min(vVelocity, maxRiseSpeed * Time.deltaTime);
        }
        else if (groundNormal == Vector3.zero) {
            vVelocity -= gravity * Time.deltaTime;
            if (vVelocity < -maxFallSpeed)
                vVelocity = -maxFallSpeed;
        }
        else {
            vVelocity -= groundDistance / Time.fixedDeltaTime;
        }

        previousGroundVelocity = Vector3.zero;
        if (groundRigidbody != null) {
            hVelocity += groundRigidbody.velocity.WithY(0);
            vVelocity += groundRigidbody.velocity.y;
            previousGroundVelocity = groundRigidbody.velocity;
        }
        else if (currentTornado != null) {
            hVelocity += currentTornado.rigidbody.velocity.WithY(0);
            vVelocity += currentTornado.rigidbody.velocity.y;
            previousGroundVelocity = currentTornado.rigidbody.velocity;
        }
        rigidbody.velocity = hVelocity.WithY(vVelocity);
    }



}
