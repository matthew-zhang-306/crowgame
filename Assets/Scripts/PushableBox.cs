using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox : PhysicsObject
{
    public float pushSpeed;
    private float peckedTimer;

    protected override void Awake() {
        base.Awake();
    }

    protected override void FixedUpdate() {
        // base.FixedUpdate();
        Vector3 hVelocity = rigidbody.velocity.WithY(0);
        float vVelocity = rigidbody.velocity.y;

        CheckForGround();
        if (groundRigidbody != null) {
            hVelocity -= previousGroundVelocity.WithY(0);
            vVelocity -= previousGroundVelocity.y;
        }
        
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

        if (groundRigidbody != null) {
            hVelocity += groundRigidbody.velocity.WithY(0);
            vVelocity += groundRigidbody.velocity.y;
            previousGroundVelocity = groundRigidbody.velocity;
        }
        rigidbody.velocity = hVelocity.WithY(vVelocity);

        peckedTimer = Mathf.Max(peckedTimer - Time.deltaTime, 0f);
        if (peckedTimer == 0f) {
            rigidbody.mass = 1000f;
        }
    }


    protected void OnTriggerEnter(Collider other) {
        if (peckedTimer == 0f && other.CompareTag("Peck") && (groundNormal != Vector3.zero || currentTornado != null)) {
            Vector3 pushDirection = other.transform.forward;
            Debug.DrawRay(collider.bounds.center, other.transform.forward * 1f, Color.yellow, 0.2f);
            
            // move
            rigidbody.velocity = (pushDirection * pushSpeed).WithY(rigidbody.velocity.y);
            peckedTimer = 0.18f;
            rigidbody.mass = 10f;
        }
    }



    public void EnterTornado(Tornado tornado) {
        currentTornado = tornado;
    }

    public void ExitTornado() {
        currentTornado = null;
    }
    
}
