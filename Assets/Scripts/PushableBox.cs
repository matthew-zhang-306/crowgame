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
        if (other.CompareTag("Peck") && (groundNormal != Vector3.zero || currentTornado != null)) {
            Vector3 pushDirection = other.transform.forward;
            Debug.DrawRay(collider.bounds.center, other.transform.forward * 1f, Color.yellow, 0.2f);
            if (!Physics.BoxCast(collider.bounds.center, Vector3.one * 0.4f, pushDirection, out RaycastHit hit, Quaternion.identity, 1f, wallMask)) {
                // move
                intendedPosition += pushDirection;
                currentRide = null;
                currentTornado = null;
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
