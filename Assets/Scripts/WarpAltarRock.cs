using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpAltarRock : MonoBehaviour
{
    [Header("Parameters")]
    public float minRiseHeight;
    public float maxRiseHeight;
    private float baseHeight;
    public float maxStartRotation;
    public float minRotateSpeed;
    public float maxRotateSpeed;
    public float minBobRange;
    public float maxBobRange;
    public float minBobSpeed;
    public float maxBobSpeed;
    public float gravity;
    
    private Quaternion targetRotation;
    private float targetHeight;
    private float currentHeight;
    private float currentVelocity;
    private float currentRotateSpeed;
    private float currentBobRange;
    private float currentBobSpeed;
    private float currentBobTimer;
    private bool isLifted;
    public bool IsLifted {
        get { return isLifted; }
        set {
            isLifted = value;
            if (isLifted) {
                // randomize
                targetRotation = Quaternion.Euler(
                    Random.Range(-maxStartRotation, maxStartRotation),
                    Random.Range(-maxStartRotation, maxStartRotation),
                    Random.Range(-maxStartRotation, maxStartRotation)
                );
                targetHeight = Random.Range(minRiseHeight, maxRiseHeight) + baseHeight;
                currentRotateSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);
                currentBobRange = Random.Range(minBobRange, maxBobRange);
                currentBobSpeed = Random.Range(minBobSpeed, maxBobSpeed);
                currentBobTimer = Random.Range(0, 1);
            }
        }
    }


    private void Awake() {
        baseHeight = transform.position.y;
    }

    private void FixedUpdate() {
        if (isLifted) {
            currentBobTimer += currentBobSpeed * Time.deltaTime;
            float desiredHeight = targetHeight + Mathf.Sin(currentBobTimer * Mathf.PI) * currentBobRange;
            currentVelocity = Mathf.MoveTowards(currentVelocity, 0.2f * (desiredHeight - currentHeight) / Time.deltaTime, gravity * Time.deltaTime);
            Debug.DrawRay(transform.position, Vector3.up * currentVelocity * Time.deltaTime, Color.yellow, Time.deltaTime);
            currentHeight += currentVelocity * Time.deltaTime;

            targetRotation = Quaternion.Euler(0, currentRotateSpeed * Time.deltaTime, 0) * targetRotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);
        }
        else {
            currentVelocity -= gravity * Time.deltaTime;
            currentHeight += currentVelocity * Time.deltaTime;
            if (currentHeight < baseHeight) {
                currentHeight = baseHeight;
                currentVelocity = 0;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 0.1f);
        }

        transform.position = transform.position.WithY(currentHeight);
    }


    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position.WithY(targetHeight), 0.1f);
    }


}
