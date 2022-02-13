using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    public PositionSO cameraPositionSO;

    private void FixedUpdate() {
        // since we have the camera angle we'll do the sprite rotation now
        float rotateAngle = Vector3.SignedAngle(transform.forward, cameraPositionSO.direction.WithY(0), Vector3.up);
        transform.Rotate(new Vector3(0, rotateAngle, 0));
    }
}
