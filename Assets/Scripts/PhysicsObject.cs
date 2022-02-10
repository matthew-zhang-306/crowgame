using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PhysicsObject : MonoBehaviour
{
    [Header("Physics Parameters")]
    public float friction;
    public float maxFallSpeed;
    public float gravity;

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

    protected Tornado currentTornado;
    protected Coroutine tornadoRoutine;
    

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
            /*vVelocity += riseAccel * Time.deltaTime;
            vVelocity = Mathf.Min(vVelocity, maxRiseSpeed * Time.deltaTime);*/
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

            rigidbody.velocity = rigidbody.velocity.WithY(0);
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

            rigidbody.velocity = rigidbody.velocity.WithY(0);
        }
    }



}
