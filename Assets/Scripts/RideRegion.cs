using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideRegion : MonoBehaviour
{
    public new Collider collider;
    public PhysicsObject physicsObject;


    protected void OnTriggerEnter(Collider other) {
        PhysicsObject obj = other.GetComponent<PhysicsObject>();

        if (obj != null && obj != physicsObject) {
            physicsObject.SetRiding(obj);
        }
    }

    protected void OnTriggerExit(Collider other) {
        PhysicsObject obj = other.GetComponent<PhysicsObject>();

        if (obj != null && obj != physicsObject) {
            physicsObject.SetNotRiding(obj);
        }
    }
}
