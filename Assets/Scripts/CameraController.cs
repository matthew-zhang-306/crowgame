using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotateIncrement;
    private float intendedRotation;
    private float currentRotation;

    private bool leftInput;
    private bool oldLeftInput;
    private bool rightInput;
    private bool oldRightInput;

    private void Update() {
        leftInput = Input.GetAxisRaw("LeftShift") > 0;
        rightInput = Input.GetAxisRaw("RightShift") > 0;
    }

    private void FixedUpdate() {
        if (leftInput && !oldLeftInput) {
            intendedRotation += rotateIncrement;
        }
        if (rightInput && !oldRightInput) {
            intendedRotation -= rotateIncrement;
        }

        currentRotation = Mathf.Lerp(currentRotation, intendedRotation, 0.2f);
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            currentRotation,
            transform.rotation.eulerAngles.z
        );

        oldLeftInput = leftInput;
        oldRightInput = rightInput;
    }

}
