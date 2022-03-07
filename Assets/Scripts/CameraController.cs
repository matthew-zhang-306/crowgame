using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotateIncrement;
    private float intendedRotation;
    private float currentRotation;

    private int camInput;
    private int oldCamInput;

    private void Update() {
        float camInputAnalog = Input.GetAxis("Camera");
        if (Mathf.Abs(camInputAnalog) > 0.2f) {
            camInput = (int)Mathf.Sign(camInputAnalog);
        }
        else {
            camInput = 0;
        }
    }

    private void FixedUpdate() {
        if (camInput != oldCamInput && camInput != 0) {
            intendedRotation += camInput * rotateIncrement;
        }

        currentRotation = Mathf.Lerp(currentRotation, intendedRotation, 0.2f);
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            currentRotation,
            transform.rotation.eulerAngles.z
        );

        oldCamInput = camInput;
    }

}
