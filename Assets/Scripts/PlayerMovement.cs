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

    [Header("References")]
    public Transform rotateTransform;
    public GameObject peckHitbox;

    new Rigidbody rigidbody;
    new Collider collider;
    Vector3 horizontalInput;
    Vector3 groundNormal;
    float groundDistance;


    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        peckHitbox.SetActive(false);
    }


    private void FixedUpdate() {
        groundNormal = Vector3.zero;
        if (Physics.BoxCast(collider.bounds.center + Vector3.up * 0.1f, collider.bounds.extents, Vector3.down, out RaycastHit hit, Quaternion.identity, 0.25f)) {
            Debug.DrawRay(transform.position, hit.normal * 2f, Color.red, Time.fixedDeltaTime);
            groundDistance = hit.distance - 0.11f;
            groundNormal = hit.normal;
        }
        
        GetHorizontalInput();
        HandleMovement();

        if (Input.GetAxisRaw("Action1") > 0 && !peckHitbox.activeInHierarchy) {
            // peck
            peckHitbox.SetActive(true);
            this.Invoke(() => peckHitbox.SetActive(false), 0.2f);
        }
    }


    private void GetHorizontalInput() {
        horizontalInput = new Vector3(
            Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")
        ).normalized;

        // TODO: rotate horizontal input depending on camera direction
    
        if (horizontalInput != Vector3.zero) {
            Vector3 hInputRounded = Quaternion.Euler(0, Helpers.RoundToNearest(Vector3.SignedAngle(Vector3.forward, horizontalInput, Vector3.up), 90f), 0) * Vector3.forward;
            rotateTransform.Rotate(0, Vector3.SignedAngle(rotateTransform.forward, hInputRounded, Vector3.up), 0);
        }
    }


    private void HandleMovement() {
        Vector3 velocity = rigidbody.velocity;
        
        Vector3 horizontalVel = velocity.WithY(0);
        horizontalVel = Vector3.MoveTowards(horizontalVel, horizontalInput * maxSpeed, acceleration * Time.deltaTime);
        velocity.x = horizontalVel.x;
        velocity.z = horizontalVel.z;

        if (groundNormal != Vector3.zero) {
            Vector3 groundNormalHorizontal = groundNormal.WithY(0);
            float horizontalFactor = -Vector3.Dot(groundNormalHorizontal, horizontalVel.normalized);
            Vector3 groundNormalUp = groundNormal - groundNormalHorizontal;
            if (groundNormalUp.magnitude != 0)
                velocity.y = horizontalVel.magnitude * horizontalFactor / groundNormalUp.magnitude;
            else
                velocity.y = 0;
            
            Debug.DrawRay(transform.position, horizontalVel, Color.magenta, Time.fixedDeltaTime);
            Debug.DrawRay(transform.position + horizontalVel, new Vector3(0, velocity.y, 0), Color.magenta, Time.fixedDeltaTime);
        }
        
        if (groundNormal == Vector3.zero) {
            velocity += Vector3.down * gravity * Time.deltaTime;
            if (velocity.y < -maxFallSpeed)
                velocity.y = maxFallSpeed;
        }
        else {
            velocity.y -= groundDistance / Time.fixedDeltaTime;
        }

        Debug.DrawRay(transform.position, velocity, Color.cyan, Time.fixedDeltaTime);
        rigidbody.velocity = velocity;
    }


    private void OnDrawGizmos() {
        if (peckHitbox != null && peckHitbox.activeInHierarchy) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(peckHitbox.transform.position, Vector3.one * 0.5f);
        }
    }

}
