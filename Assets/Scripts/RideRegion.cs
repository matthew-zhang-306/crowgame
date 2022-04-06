using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideRegion : MonoBehaviour
{
    public new Collider collider;
    public PhysicsObject physicsObject;

    protected void OnTriggerEnter(Collider other) {
        PhysicsObject obj = other.GetComponent<PhysicsObject>();

        if (obj != null && obj != physicsObject && obj.transform.position.y > collider.bounds.max.y) {
            physicsObject.AddRider(obj);
        }
    }

    protected void OnTriggerExit(Collider other) {
        PhysicsObject obj = other.GetComponent<PhysicsObject>();

        if (obj != null && obj != physicsObject) {
            physicsObject.RemoveRider(obj);
        }
    }
}
