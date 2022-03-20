using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWhenEntered : BaseSwitch
{
    private void OnEnable() {
        RespawnRegion.OnRespawn += OnRespawn;
    }
    private void OnDisable() {
        RespawnRegion.OnRespawn -= OnRespawn;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !targets[0].State) {
            Switch();
        }
    }

    private void OnRespawn() {
        if (targets[0].State) {
            Switch();
        }
    }
}
