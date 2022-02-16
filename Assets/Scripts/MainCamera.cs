using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public PositionSO cameraPositionSO;

    private void FixedUpdate() {
        cameraPositionSO.position = transform.position;
        cameraPositionSO.direction = transform.forward;
    }
}
