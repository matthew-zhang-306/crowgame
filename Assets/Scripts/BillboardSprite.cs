using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    [Tooltip("TRUE if you want the billboarding to account for the pitch of the camera")]
    public bool shouldRoll;
    public PositionSO cameraPositionSO;

    private void FixedUpdate() {
        if (shouldRoll) {
            transform.LookAt(transform.position + cameraPositionSO.direction, Vector3.up);
        }
        else {
            // rotate the sprite to face the camera
            float rotateAngle = Vector3.SignedAngle(transform.forward, cameraPositionSO.direction.WithY(0), Vector3.up);
            transform.RotateAround(transform.position, Vector3.up, rotateAngle);
        }
    }
}
