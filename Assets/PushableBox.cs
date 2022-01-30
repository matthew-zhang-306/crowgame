using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox : GridAlignedObject
{
    Tornado currentTornado;
    

    protected override void Awake() {
        base.Awake();
    }


    protected override void FixedUpdate() {
        base.FixedUpdate();
    }

    protected override void AlignIntendedPosition()
    {
        if (currentTornado != null) {
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
        if (other.CompareTag("Peck") && groundNormal != Vector3.zero) {
            Vector3 pushDirection = other.transform.forward;
            Debug.DrawRay(collider.bounds.center, other.transform.forward * 1f, Color.yellow, 0.2f);
            if (!Physics.BoxCast(collider.bounds.center, Vector3.one * 0.4f, pushDirection, out RaycastHit hit, Quaternion.identity, 1f, wallMask)) {
                // move
                intendedPosition += pushDirection;
                currentRide = null;
            }
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
