using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

// a physicsobject represents anything that moves and collides with things
public class PhysicsObject : MonoBehaviour
{

    [Header("Physics Parameters")]
    public float friction;
    public float maxFallSpeed;
    public float gravity;

    [Header("Ride Physics Parameters")]
    public bool canRide = true;
    public float rideSnapSpeed;

    [Header("Tornado Physics Parameters")]
    public float tornadoStopAccel;
    public float tornadoRiseTime;

    protected Vector3 groundNormal; // normal vector of the ground that the object is on. check this variable to see if we are on the ground or not
    protected float groundY; // y level of the ground that the object is on
    protected float groundDistance; // size of the gap between the object and the ground below it
    protected Rigidbody groundRigidbody; // rigidbody attached to the ground that the object is on
    protected Vector3 groundPoint; // position that the ground collision was detected at
    protected Vector3 previousGroundVelocity; // the velocity of the ground when we last checked

    protected int wallMask; // layer mask for raycasting for walls
    protected new Rigidbody rigidbody;
    protected new Collider collider;
    protected float currentGravity;

    protected HashSet<PhysicsObject> allRiders; // stores all objects that are riding this one
    protected HashSet<PhysicsObject> allCarriers; // stores all objects that are carrying this one
    protected Tornado currentTornado;
    protected Coroutine tornadoRoutine;
    

    protected virtual void Awake() {
        wallMask = LayerMask.GetMask("Wall", "Box");
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        allRiders = new HashSet<PhysicsObject>();
        allCarriers = new HashSet<PhysicsObject>();
    }

    protected virtual void OnEnable() {
        // empty, for now
    }
    protected virtual void OnDisable() {
        // let everyone know that they will no longer be colliding with this object
        foreach (PhysicsObject rider in allRiders.ToList()) {
            this.RemoveRider(rider);
        }
        foreach (PhysicsObject carrier in allCarriers.ToList()) {
            carrier.RemoveRider(this);
        }
    }


    // sets the velocity of the object, relative to the object that is carrying it
    protected virtual void SetRelativeVelocity(Vector3 vel) {
        Vector3 velocity = rigidbody.velocity;
        velocity -= previousGroundVelocity;
        velocity = vel;

        PhysicsObject ride = GetMainCarrier();
        if (this is PushableBox box && currentTornado != null) {
            Debug.Log(ride + " " + ride.rigidbody.velocity);
        }
        previousGroundVelocity = ride != null ? ride.rigidbody.velocity : Vector3.zero;
        velocity += previousGroundVelocity;
        rigidbody.velocity = velocity;

        if (this is PushableBox boxx && currentTornado != null) {
            Debug.Log(vel + " " + rigidbody.velocity);
        }

        // our velocity has changed so we update the velocity of everyone riding us to account for it
        foreach (PhysicsObject riding in allRiders) {
            riding.SetRelativeVelocity(riding.GetRelativeVelocity());
        }
    }

    // gets the velocity of the object, relative to the object that is carrying it
    protected virtual Vector3 GetRelativeVelocity() {
        return rigidbody.velocity - previousGroundVelocity;
    }


    protected virtual void FixedUpdate() {
        CheckForGround();
        HandleMovement();
    }

    // scan for ground below us and update groundNormal and all of the other variables
    protected virtual void CheckForGround() {
        groundNormal = Vector3.zero;
        groundRigidbody = null;

        // boxcast down starting from a location raised by 0.1
        var castHit = Physics.BoxCast(
            collider.bounds.center + Vector3.up * 0.1f, // we raise the location by 0.1 in case the object is slightly inside of the ground it is on
            collider.bounds.extents - new Vector3(0.03f, 0f, 0.03f), // we decrease the box size by 0.03 in the horizontal directions to avoid being able to stand on wall seams
            Vector3.down,
            out RaycastHit hit,
            Quaternion.identity,
            0.25f,
            wallMask
        );

        if (castHit) {
            Debug.DrawRay(transform.position, hit.normal * 2f, Color.red, Time.fixedDeltaTime);

            // we hit ground at this location
            groundY = hit.point.y;
            groundNormal = hit.normal;
            groundDistance = hit.distance - 0.11f; // 0.11 = 0.1 (the distance that the boxcast was raised by) + 0.01 (the minimum distance between adjacent colliders in unity)
            groundPoint = hit.point;

            groundRigidbody = hit.rigidbody ?? hit.collider.GetComponentInParent<Rigidbody>();
        }
    }

    protected virtual void HandleMovement() {
        Vector3 hVelocity = GetRelativeVelocity().WithY(0);
        float vVelocity = GetRelativeVelocity().y;
        
        hVelocity = HandleHorizontalMovement(hVelocity);
        vVelocity = HandleVerticalMovement(vVelocity);

        SetRelativeVelocity(hVelocity.WithY(vVelocity));                    
    }

    // calculates what the object's new horizontal velocity should be based on what it is currently
    protected virtual Vector3 HandleHorizontalMovement(Vector3 hVelocity) {
        PhysicsObject theRide = GetMainCarrier();

        if (theRide != null) {
            // check for theRide's local grid
            Vector3 gridPosition = transform.position.RoundToNearest(Vector3.one, theRide.transform.position.WithY(0));
            Vector3.SmoothDamp(transform.position, gridPosition, ref hVelocity, 0.1f, 999f);
        }
        else if (groundNormal != Vector3.zero) {
            // align to the world grid while on the ground
            Vector3 gridPosition = transform.position.RoundToNearest(Vector3.one);
            Vector3.SmoothDamp(transform.position, gridPosition, ref hVelocity, 0.1f, 999f);
        }

        /*
        if (allCarriers.Count > 0) {
            // we're riding something. snap towards its position
            Vector3 rideOffset = theRide.GetRidePoint(this).WithY(0) - transform.position.WithY(0);
            hVelocity = Vector3.ClampMagnitude(rideOffset, rideSnapSpeed * Time.deltaTime);
        }
        else {
            // apply friction while not riding something
            hVelocity = Vector3.MoveTowards(hVelocity, Vector3.zero, friction);
            if (groundNormal == Vector3.zero && currentTornado == null) {
                hVelocity = Vector3.zero;
            }
        }
        */

        return hVelocity;
    }

    // calculates what the object's new vertical velocity should be based on what it is currently
    protected virtual float HandleVerticalMovement(float vVelocity) {
        if (currentTornado != null) {
            // tornadoes override being in the air
            Mathf.SmoothDamp(transform.position.y, currentTornado.GetRidePoint(this).y, ref vVelocity, 0.1f, 8f);
        }
        else if (groundNormal == Vector3.zero) {
            // apply gravity while in the air
            vVelocity -= gravity * Time.deltaTime;
            if (vVelocity < -maxFallSpeed)
                vVelocity = -maxFallSpeed;
        }
        else {
            var carrier = GetMainCarrier();
            if (carrier != null) {
                // stay at ride height while riding something
                Mathf.SmoothDamp(transform.position.y, carrier.GetRidePoint(this).y, ref vVelocity, 0.1f, 8f);
            }

            // stay at the ground height while on the ground
            vVelocity = -groundDistance / Time.fixedDeltaTime;
        }

        return vVelocity;
    }


    public virtual void EnterTornado(Tornado tornado) {
        currentTornado = tornado;
        // tornadoRoutine = StartCoroutine(DoTornadoPhysics());
    }
    public virtual void ExitTornado() {
        currentTornado = null;
        /*
        if (tornadoRoutine != null) {
            StopCoroutine(tornadoRoutine);
        }
        */
    }

    /*
    // a coroutine responsible for setting the object's y velocity while it is being lifted by a tornado
    protected IEnumerator DoTornadoPhysics() {
        if (rigidbody.velocity.y <= 0f) {
            // the object is moving downward, so start by slowing it down to 0
            while (rigidbody.velocity.y < 0f) {
                rigidbody.velocity = rigidbody.velocity.WithY(rigidbody.velocity.y + tornadoStopAccel * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
            }

            // then raise the object to the top over a fixed period of time
            float startY = transform.position.y;
            float timer = 0;
            while (true) {
                // desiredY is where we want to go
                float endY = currentTornado.GetRidePoint(this).y;
                float desiredY = DOVirtual.EasedValue(startY, endY, timer / tornadoRiseTime, Ease.InOutQuad);
                
                // set velocity correctly and wait for the next physics frame
                rigidbody.velocity = rigidbody.velocity.WithY((desiredY - transform.position.y) / Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();

                // increment timer
                if (timer == tornadoRiseTime) {
                    break;
                }
                timer = Mathf.Min(timer + Time.fixedDeltaTime, tornadoRiseTime);
            }
        }
        else {
            // the object is moving upward
            bool isSlowing = true;

            // strategy: we slow the object down while simulating a "ghost" version of the object that gets raised to the top over a fixed period of time
            // when the object is moving slower than the ghost version of the object, we switch over to the ghost
            float startY = transform.position.y;
            float ghostY = transform.position.y; // this is where the ghost is currently located
            float timer = 0;
            while (timer < tornadoRiseTime) {
                // find out how fast the ghost wants to move this frame
                float endY = currentTornado.GetRidePoint(this).y;
                float desiredY = DOVirtual.EasedValue(startY, endY, timer / tornadoRiseTime, Ease.InOutQuad);
                float desiredYVel = (desiredY - ghostY) / Time.fixedDeltaTime;

                if (isSlowing) {
                    // decrease y velocity
                    rigidbody.velocity = rigidbody.velocity.WithY(rigidbody.velocity.y - tornadoStopAccel);
                    if (rigidbody.velocity.y <= desiredYVel) {
                        // we're now slower than the ghost, so switch over
                        isSlowing = false;
                    } 
                }
                if (!isSlowing) {
                    // set our velocity to match the ghost's velocity
                    rigidbody.velocity = rigidbody.velocity.WithY(desiredYVel);
                }

                // move and wait for the next physics frame
                ghostY = desiredY;
                yield return new WaitForFixedUpdate();

                // increment timer
                if (timer == tornadoRiseTime) {
                    break;
                }
                timer = Mathf.Min(timer + Time.fixedDeltaTime, tornadoRiseTime);
            }

            while (isSlowing) {
                // continue decreasing velocity
                rigidbody.velocity = rigidbody.velocity.WithY(rigidbody.velocity.y - tornadoStopAccel);
                if (rigidbody.velocity.y <= 0) {
                    isSlowing = false;
                }
                yield return new WaitForFixedUpdate();
            }
        }

        while (true) {
            rigidbody.velocity = rigidbody.velocity.WithY((currentTornado.GetRidePoint(this).y - transform.position.y) / Time.fixedDeltaTime);
            yield return 0;
        }
    }
    */


    // returns the position to which anything riding this object should snap to in order to stay on it
    public virtual Vector3 GetRidePoint(PhysicsObject rider) {
        // rider is assumed to be inside allRiders

        return collider.bounds.center + Vector3.up * collider.bounds.extents.y;
    }


    // the physics object can be carried by many objects, but we need to pick one to move with. this method does that
    protected virtual PhysicsObject GetMainCarrier() {
        if (allCarriers.Count == 0) {
            // no carrier to select
            return null;
        }

        // prioritize tornadoes
        PhysicsObject carrier = allCarriers.FirstOrDefault(p => p is Tornado t);
        if (carrier != null) {
            return carrier;
        }

        // otherwise, pick the closest one
        foreach (PhysicsObject c in allCarriers) {
            if (carrier == null || (transform.position - c.GetRidePoint(this)).sqrMagnitude >
                (transform.position - c.GetRidePoint(this)).sqrMagnitude)
            {
                carrier = c;
            }
        }
        return carrier;
    }

    public virtual void AddRider(PhysicsObject rider) {
        if (!rider.canRide) {
            return;
        }

        allRiders.Add(rider);
        rider.allCarriers.Add(this);
    }

    public virtual void RemoveRider(PhysicsObject rider) {
        allRiders.Remove(rider);
        rider.allCarriers.Remove(this);
    }


    protected virtual void OnDrawGizmosSelected() {
        Gizmos.color = Color.white;
        if (allCarriers != null) {
            foreach (PhysicsObject carrier in allCarriers) {
                Gizmos.DrawLine(transform.position, carrier.GetRidePoint(this));
                Gizmos.DrawWireSphere(carrier.GetRidePoint(this), 0.5f);
            }
        }
    }

}
