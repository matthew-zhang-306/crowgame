using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox : GridAlignedObject
{
    public float pushSpeed;
    public float friction;
    public float riseAccel;
    public float maxRiseSpeed;
    public float maxFallSpeed;

    Tornado currentTornado;

    private float peckedTimer;

    protected override void Awake() {
        base.Awake();
    }

    protected override void FixedUpdate() {
        // base.FixedUpdate();
        Vector3 hVelocity = rigidbody.velocity.WithY(0);
        float vVelocity = rigidbody.velocity.y;

        CheckForGround();
        
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

        rigidbody.velocity = hVelocity.WithY(vVelocity);

        peckedTimer = Mathf.Max(peckedTimer - Time.deltaTime, 0f);
        if (peckedTimer == 0f) {
            rigidbody.mass = 1000f;
        }
    }

    protected override void AlignIntendedPosition()
    {
        if (currentTornado != null) {
            // check if the box wants to be in the tornado
            /*
            Vector3 roundedIntendedPosition = intendedPosition.WithY(0).RoundToNearest(1);
            Vector3 tornadoPosition = currentTornado.IntendedPosition.WithY(0).RoundToNearest(1);
            
            if (roundedIntendedPosition == tornadoPosition) {
                intendedPosition = currentTornado.transform.position.WithY(intendedPosition.y);
            }
            */

            intendedPosition = currentTornado.transform.position.WithY(intendedPosition.y);
        }
        base.AlignIntendedPosition();
    }

    protected override void SetIntendedY()
    {
        if (currentTornado != null && currentRide == null) {
            intendedPosition.y = currentTornado.Top.y;
        }
        else {
            base.SetIntendedY();
        }
    }


    protected override void OnTriggerEnter(Collider other) {
        if (peckedTimer == 0f && other.CompareTag("Peck") && (groundNormal != Vector3.zero || currentTornado != null)) {
            Vector3 pushDirection = other.transform.forward;
            Debug.DrawRay(collider.bounds.center, other.transform.forward * 1f, Color.yellow, 0.2f);
            
            // move
            intendedPosition += pushDirection;
            rigidbody.velocity = (pushDirection * pushSpeed).WithY(rigidbody.velocity.y);
            peckedTimer = 0.18f;
            rigidbody.mass = 10f;
        }

        base.OnTriggerEnter(other);
    }



    public void EnterTornado(Tornado tornado) {
        currentTornado = tornado;
        currentRide = null;
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
