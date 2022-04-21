using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingZone : MonoBehaviour
{
    public LevelListSO levelList;

    public int id;
    public int destinationId;
    public int targetHubIndex;

    public Vector3 playerPosOffset;
    public Vector3 PlayerPosition => transform.position + playerPosOffset;
    public Vector3 walkDirection;
    
    public int musicIndex;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerMovement>()?.SetLoadingZone(this);
            
            HubSpawnHandler.SetExit(this, id);
            Managers.ScenesManager.ChangeScene(levelList.hubs[targetHubIndex].sceneName);
        }
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(PlayerPosition, 1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(PlayerPosition, walkDirection * 2f);
    }
}
