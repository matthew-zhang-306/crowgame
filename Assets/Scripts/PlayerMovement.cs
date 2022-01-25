using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Parameters")]
    public float maxSpeed;
    public float acceleration;
    public float maxFallSpeed;
    public float gravity;

    Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate() {
        Vector3 velocity = rb.velocity;
        
        Vector2 horizontalVel = velocity.XZ();
        Vector2 targetHorizontalVel = new Vector2(
            Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")
        );
        // TODO: rotate targetHorizontalVel depending on camera direction
        horizontalVel = Vector2.MoveTowards(horizontalVel, targetHorizontalVel * maxSpeed, acceleration * Time.deltaTime);
        velocity.x = horizontalVel.x;
        velocity.z = horizontalVel.y;
        
        velocity += Vector3.down * gravity * Time.deltaTime;

        rb.velocity = velocity;
    }
}
