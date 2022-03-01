using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : PhysicsObject
{
    [Header("Moving Platform Parameters")]
    public Vector3 destinationOffset;
    private Vector3 startPosition;
    private Vector3 endPosition;

    public float moveTime;
    public float moveDelay;

    public bool isAutomatic = true;
    private bool isTargettingDestination = false; // true while the platform is moving to or sitting at the end position
    private Coroutine moveCoroutine;

    protected override void Awake() {
        base.Awake();

        startPosition = transform.position;
        endPosition = startPosition + destinationOffset;

        if (isAutomatic) {
            StartCoroutine(DoMovement());
        }
    }

    protected override void FixedUpdate()
    {
        // don't do what physicsobject normally does here
    }

    public override void EnterTornado(Tornado tornado)
    {
        // do nothing
    }
    public override void ExitTornado()
    {
        // do nothing
    }


    public void Move() {
        Move(!isTargettingDestination);
    }

    public void Move(bool shouldTargetDestination) {
        if (isAutomatic) {
            Debug.LogError("Cannot manually move an automatic moving platform");
            return;
        }

        if (shouldTargetDestination == isTargettingDestination) {
            // we're already moving there
            return;
        }

        // cancel the previous movement, if any
        if (moveCoroutine != null) {
            StopCoroutine(moveCoroutine);
        }

        isTargettingDestination = shouldTargetDestination;
        moveCoroutine = StartCoroutine(DoMove(transform.position, isTargettingDestination ? endPosition : startPosition));
    }


    IEnumerator DoMovement() {
        while (true) {
            yield return new WaitForSeconds(moveDelay);
            isTargettingDestination = true;
            yield return StartCoroutine(DoMove(startPosition, endPosition));
            
            yield return new WaitForSeconds(moveDelay);
            isTargettingDestination = false;
            yield return StartCoroutine(DoMove(endPosition, startPosition));
        }
    }

    IEnumerator DoMove(Vector3 start, Vector3 end) {
        float t = 0;
        while (t < moveTime) {
            // wait first to sync all movement to the fixed update cycle
            yield return new WaitForFixedUpdate();
             t += Time.fixedDeltaTime;

            // set velocity such that the platform will move to where it wants to be
            Vector3 desiredPos = Vector3.Lerp(start, end, t / moveTime);
            SetRelativeVelocity((desiredPos - transform.position) / Time.fixedDeltaTime);
            transform.position = desiredPos;
        }

        // snap to the end point on the next frame
        yield return new WaitForFixedUpdate();
        SetRelativeVelocity((end - transform.position) / Time.fixedDeltaTime);
    }


    private void OnDrawGizmos() {
        if (collider == null) {
            collider = GetComponent<Collider>();
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + destinationOffset);
        Gizmos.DrawWireCube(collider.bounds.center + destinationOffset, collider.bounds.size);
    }
}
