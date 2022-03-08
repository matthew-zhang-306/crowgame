using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnRegion : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            
            // TODO: add animation
            player?.WarpToSafePosition();
        }
    }
}
