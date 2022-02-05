using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpAltar : MonoBehaviour
{
    public string targetScene;
    private PlayerMovement playerInside;


    private void Update() {
        if (playerInside != null && Input.GetAxisRaw("Action1") > 0) {
            Managers.ScenesManager.ChangeScene(targetScene, 2f);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerInside = other.GetComponent<PlayerMovement>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            playerInside = null;
        }
    }
}
