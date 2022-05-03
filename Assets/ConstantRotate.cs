using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotate : MonoBehaviour
{
    public Vector3 axis;
    public float speed;

    private void FixedUpdate() {
        transform.Rotate(axis, speed * Time.deltaTime);
    }
}
