using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox : PhysicsObject
{
    public float pushSpeed;
    private float peckedTimer;
    public RideRegion rideRegion;

    private Vector3 prevHVelocity;
    private Vector3 lastPosition;

    protected override void Awake() {
        base.Awake();
        lastPosition = transform.position;
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        
        prevHVelocity = rigidbody.velocity.WithY(0);
        
        peckedTimer = Mathf.Max(peckedTimer - Time.deltaTime, 0f);
        if (peckedTimer == 0f) {
            rigidbody.mass = 1000f;
        }
    }


    protected override Vector3 HandleHorizontalMovement(Vector3 hVelocity) {
        if (peckedTimer > 0f) {
            // while the box has been pecked, ignore any rides and apply friction
            hVelocity = Vector3.MoveTowards(hVelocity, Vector3.zero, friction);
            return hVelocity;
        }

        hVelocity = base.HandleHorizontalMovement(hVelocity);
        return hVelocity;
    }


    public override Vector3 GetRidePoint(PhysicsObject rider) {
        return rideRegion.transform.position;
    }


    protected void OnTriggerEnter(Collider other)
    {
        // base.OnTriggerEnter(other);

        if (peckedTimer == 0f && other.CompareTag("Peck") && (groundNormal != Vector3.zero || currentTornado != null))
        {
            Managers.AudioManager.PlaySound("Peck");
            Instantiate(Resources.Load("Peck"), other.transform.position, other.transform.rotation);
            Vector3 pushDirection = other.transform.forward;
            Debug.DrawRay(collider.bounds.center, other.transform.forward * 1f, Color.yellow, 0.2f);

            // check if the grid space is open
            if (Physics.BoxCast(transform.position, Vector3.one * 0.3f, pushDirection, Quaternion.identity, 1f, wallMask)) {
                // can't move there
                return;
            }

            // move
            rigidbody.velocity = (pushDirection * pushSpeed).WithY(rigidbody.velocity.y);
            peckedTimer = 0.18f;
            rigidbody.mass = 10f;

            //set last position on each peck
            //lastPosition = transform.position;
        }

        else if (other.CompareTag("DeathBox"))
        {
            ResetBoxPosition();
        }
    }

    //sets box's position to the position of the last time it was pecked
    public void ResetBoxPosition()
    {
        transform.position = lastPosition;
    }
}
