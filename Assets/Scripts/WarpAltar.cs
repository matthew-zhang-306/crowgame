using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpAltar : MonoBehaviour
{
    public string targetScene;
    public string destinationName;
    private PlayerMovement playerInside;

    public delegate void AltarDelegate(WarpAltar altar);
    public static AltarDelegate OnAltarEnter;
    public static AltarDelegate OnAltarExit;
    public static AltarDelegate OnAltarWarp;


    private void Update() {
        if (playerInside != null && Input.GetAxisRaw("Action1") > 0) {
            OnAltarWarp?.Invoke(this);
            Managers.ScenesManager.ChangeScene(targetScene, 2f);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            OnAltarEnter?.Invoke(this);
            playerInside = other.GetComponent<PlayerMovement>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            OnAltarExit?.Invoke(this);
            playerInside = null;
        }
    }
}
