using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : GridAlignedObject
{
    public float topOffset;
    public Vector3 Top => transform.position + Vector3.up * topOffset;

    PushableBox previousBox;


    protected override void FixedUpdate() {
        base.FixedUpdate();
        
        // lift whatever is in the tornado
        bool cast = Physics.Raycast(
            transform.position - new Vector3(0, 0.05f, 0),
            Vector3.up,
            out RaycastHit hit,
            topOffset,
            LayerMask.GetMask("Box")
        );
        if (cast) {
            PushableBox box = hit.rigidbody.GetComponent<PushableBox>();
            if (box != previousBox) {
                previousBox?.ExitTornado();
                box?.EnterTornado(this);
            }
            
            previousBox = box;
        }
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Top, 0.2f);
    }
}
