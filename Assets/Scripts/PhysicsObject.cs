using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class PhysicsObject : MonoBehaviour
{
    [Header("Physics Parameters")]
    public float friction;
    public float maxFallSpeed;
    public float gravity;
    public float rideSnapSpeed;

    [Header("Tornado Physics Parameters")]
    public float tornadoStopAccel;
    public float tornadoRiseTime;

    protected Vector3 groundNormal;
    protected float groundY;
    protected float groundDistance;
    protected Rigidbody groundRigidbody;
    protected Vector3 previousGroundVelocity;
    protected int wallMask;
    protected new Rigidbody rigidbody;
    protected new Collider collider;
    protected float currentGravity;

    protected HashSet<PhysicsObject> allRiding;
    protected HashSet<PhysicsObject> allRides;
    protected Tornado currentTornado;
    protected Coroutine tornadoRoutine;
    

    protected virtual void Awake() {
        wallMask = LayerMask.GetMask("Wall", "Box");
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        allRiding = new HashSet<PhysicsObject>();
        allRides = new HashSet<PhysicsObject>();
    }


    protected virtual void SetRelativeVelocity(Vector3 vel) {
        Vector3 velocity = rigidbody.velocity;
        velocity -= previousGroundVelocity;
        velocity = vel;

        PhysicsObject ride = GetRide();
        previousGroundVelocity = ride != null ? ride.rigidbody.velocity : Vector3.zero;
        velocity += previousGroundVelocity;
        rigidbody.velocity = velocity;

        foreach (PhysicsObject riding in allRiding) {
            riding.SetRelativeVelocity(riding.GetRelativeVelocity());
        }
    }

    protected virtual Vector3 GetRelativeVelocity() {
        return rigidbody.velocity - previousGroundVelocity;
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
        Vector3 hVelocity = GetRelativeVelocity().WithY(0);
        float vVelocity = GetRelativeVelocity().y;
        
        // handle horizontal velocity
        PhysicsObject theRide = GetRide();
        if (allRides.Count > 0) {
            // we're riding something. snap towards its position
            Vector3 rideOffset = theRide.GetRidePoint().WithY(0) - transform.position.WithY(0);
            hVelocity = Vector3.ClampMagnitude(rideOffset, rideSnapSpeed * Time.deltaTime);
        }
        else {
            // apply friction while not riding something
            hVelocity = Vector3.MoveTowards(hVelocity, Vector3.zero, friction);
            if (groundNormal == Vector3.zero && currentTornado == null) {
                hVelocity = Vector3.zero;
            }
        }

        // handle vertical velocity
        if (currentTornado != null) {
            // do nothing! we have a coroutine handling this case
            if (gameObject.name == "PushableBox (1)") {
                Debug.Log("tornado " + vVelocity);
            }
        }
        else if (groundNormal == Vector3.zero) {
            // apply gravity while in the air
            vVelocity -= gravity * Time.deltaTime;
            if (vVelocity < -maxFallSpeed)
                vVelocity = -maxFallSpeed;
            if (gameObject.name == "PushableBox (1)") {
                Debug.Log("gravity " + vVelocity);
            }
        }
        else {
            // stay at the ground height while on the ground
            vVelocity = -groundDistance / Time.fixedDeltaTime;
            if (gameObject.name == "PushableBox (1)") {
                Debug.Log("on ground " + vVelocity + " " + groundDistance);
            }
        }

        SetRelativeVelocity(hVelocity.WithY(vVelocity));
    }


    public virtual void EnterTornado(Tornado tornado) {
        currentTornado = tornado;
        tornadoRoutine = StartCoroutine(DoTornadoPhysics());
    }
    public virtual void ExitTornado() {
        currentTornado = null;
        if (tornadoRoutine != null) {
            StopCoroutine(tornadoRoutine);
        }
    }

    protected IEnumerator DoTornadoPhysics() {
        if (rigidbody.velocity.y <= 0f) {
            // slow to 0
            while (rigidbody.velocity.y < 0f) {
                rigidbody.velocity = rigidbody.velocity.WithY(rigidbody.velocity.y + tornadoStopAccel * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
            }

            // raise w/ ease in/out
            float startY = transform.position.y;
            float timer = 0;
            while (true) {
                float endY = currentTornado.Top.y;
                float desiredY = DOVirtual.EasedValue(startY, endY, timer / tornadoRiseTime, Ease.InOutQuad);
                rigidbody.velocity = rigidbody.velocity.WithY((desiredY - transform.position.y) / Time.fixedDeltaTime);

                yield return new WaitForFixedUpdate();

                if (timer == tornadoRiseTime) {
                    break;
                }
                timer = Mathf.Min(timer + Time.fixedDeltaTime, tornadoRiseTime);
            }
        }
        else {
            bool isSlowing = true;

            // raise a ghost w/ ease in/out
            float startY = transform.position.y;
            float ghostY = transform.position.y;
            float timer = 0;
            while (timer < tornadoRiseTime) {
                float endY = currentTornado.Top.y;
                float desiredY = DOVirtual.EasedValue(startY, endY, timer / tornadoRiseTime, Ease.InOutQuad);
                float desiredYVel = (desiredY - ghostY) / Time.fixedDeltaTime;

                if (isSlowing) {
                    // decrease velocity and check if we can switch to the ghost
                    rigidbody.velocity = rigidbody.velocity.WithY(rigidbody.velocity.y - tornadoStopAccel);
                    if (rigidbody.velocity.y <= desiredYVel) {
                        isSlowing = false;
                    } 
                }
                if (!isSlowing) {
                    rigidbody.velocity = rigidbody.velocity.WithY(desiredYVel);
                }

                yield return new WaitForFixedUpdate();

                ghostY = desiredY;
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
            rigidbody.velocity = rigidbody.velocity.WithY((currentTornado.Top.y - transform.position.y) / Time.fixedDeltaTime);
            yield return 0;
        }
    }


    public virtual Vector3 GetRidePoint() {
        return collider.bounds.center + Vector3.up * collider.bounds.extents.y;
    }


    protected virtual PhysicsObject GetRide() {
        if (allRides.Count == 0) {
            return null;
        }

        PhysicsObject theRide = allRides.FirstOrDefault(p => p is Tornado t);
        if (theRide != null) {
            return theRide;
        }

        foreach (PhysicsObject aRide in allRides) {
            if (theRide == null || (transform.position - theRide.GetRidePoint()).sqrMagnitude >
                (transform.position - aRide.GetRidePoint()).sqrMagnitude)
            {
                theRide = aRide;
            }
        }
        return theRide;
    }

    public virtual void SetRiding(PhysicsObject riding) {
        allRiding.Add(riding);
        riding.allRides.Add(this);
    }

    public virtual void SetNotRiding(PhysicsObject riding) {
        allRiding.Remove(riding);
        riding.allRides.Remove(this);
    }

}
