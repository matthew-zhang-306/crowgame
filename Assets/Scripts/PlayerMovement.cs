using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Parameters")]
    public float maxSpeed;
    public float acceleration;
    public float maxFallSpeed;
    public float gravity;

    new Rigidbody rigidbody;
    new Collider collider;
    Vector3 groundNormal;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }


    private void FixedUpdate() {
        groundNormal = Vector3.zero;
        if (Physics.BoxCast(collider.bounds.center + Vector3.up * 0.1f, collider.bounds.extents, Vector3.down, out RaycastHit hit, Quaternion.identity, 0.15f)) {
            Debug.DrawRay(transform.position, hit.normal * 2f, Color.red, Time.fixedDeltaTime);
            groundNormal = hit.normal;
        }
        
        HandleMovement();
    }



    private void HandleMovement() {
        Vector3 velocity = rigidbody.velocity;
        
        Vector3 horizontalVel = velocity.WithY(0);
        Vector3 targetHorizontalVel = new Vector3(
            Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")
        ).normalized;
        // TODO: rotate targetHorizontalVel depending on camera direction
        horizontalVel = Vector3.MoveTowards(horizontalVel, targetHorizontalVel * maxSpeed, acceleration * Time.deltaTime);
        velocity.x = horizontalVel.x;
        velocity.z = horizontalVel.z;
        if (groundNormal != Vector3.zero) {
            Vector3 groundNormalHorizontal = groundNormal.WithY(0);
            float horizontalFactor = -Vector3.Dot(groundNormalHorizontal, horizontalVel.normalized);
            Vector3 groundNormalUp = groundNormal - groundNormalHorizontal;
            if (groundNormalUp.magnitude != 0)
                velocity.y = horizontalVel.magnitude * horizontalFactor / groundNormalUp.magnitude;
            else
                velocity.y = 0;

            // similar triangle relationship
            // groundNormalUp / groundNormalProjected = horizontalVelocity / what we want
            // what we want * groundNormalUp = horizontalVel * groundNormalProjected
            Debug.DrawRay(transform.position, horizontalVel, Color.magenta, Time.fixedDeltaTime);
            Debug.DrawRay(transform.position + horizontalVel, new Vector3(0, velocity.y, 0), Color.magenta, Time.fixedDeltaTime);
        }
        
        if (groundNormal == Vector3.zero) {
            velocity += Vector3.down * gravity * Time.deltaTime;
            if (velocity.y < -maxFallSpeed)
                velocity.y = maxFallSpeed;
        }

        Debug.DrawRay(transform.position, velocity, Color.cyan, Time.fixedDeltaTime);
        rigidbody.velocity = velocity;
    }

}
