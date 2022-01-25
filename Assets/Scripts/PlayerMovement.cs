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
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 0.8f)) {
            Debug.DrawRay(transform.position, hitInfo.normal, Color.red, Time.fixedDeltaTime);
        } 
    
        HandleMovement();
    }



    private void HandleMovement() {
        Vector3 velocity = rb.velocity;
        
        Vector2 horizontalVel = velocity.XZ();
        Vector2 targetHorizontalVel = new Vector2(
            Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")
        ).normalized;
        // TODO: rotate targetHorizontalVel depending on camera direction
        horizontalVel = Vector2.MoveTowards(horizontalVel, targetHorizontalVel * maxSpeed, acceleration * Time.deltaTime);
        velocity.x = horizontalVel.x;
        velocity.z = horizontalVel.y;
        
        velocity += Vector3.down * gravity * Time.deltaTime;
        if (velocity.y < -maxFallSpeed)
            velocity.y = maxFallSpeed;

        rb.velocity = velocity;
    }
}
