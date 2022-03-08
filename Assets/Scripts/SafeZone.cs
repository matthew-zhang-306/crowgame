using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public PositionSO safePositionSO;
    private PlayerMovement currentPlayer;


    private void FixedUpdate() {
        if (currentPlayer != null && currentPlayer.IsOnGround) {
            safePositionSO.position = currentPlayer.transform.position;
        }
    }


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            currentPlayer = player;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            currentPlayer = null;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = new Color(0.5f, 0f, 1f, 0.5f);
        var collider = GetComponent<Collider>();
        if (collider != null) {
            Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
        }
    }
}
