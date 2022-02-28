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

    protected override void Awake() {
        base.Awake();

        startPosition = transform.position;
        endPosition = startPosition + destinationOffset;
        StartCoroutine(DoMovement());
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

    IEnumerator DoMovement() {
        while (true) {
            yield return new WaitForSeconds(moveDelay);
            yield return StartCoroutine(Move(startPosition, endPosition));
            yield return new WaitForSeconds(moveDelay);
            yield return StartCoroutine(Move(endPosition, startPosition));
        }
    }

    IEnumerator Move(Vector3 start, Vector3 end) {
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
