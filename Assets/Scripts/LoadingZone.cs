using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingZone : MonoBehaviour
{
    public LevelListSO levelList;

    public string targetExitName;
    public int targetHubIndex;

    public ExitPoint exitPoint;
    public Vector3 walkDirection => transform.forward;
    
    public int musicIndex;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerMovement>()?.SetLoadingZone(this);
            
            Managers.ScenesManager.SetDestinationExit(targetExitName);
            Managers.ScenesManager.ChangeScene(levelList.hubs[targetHubIndex].sceneName);
        }
    }

}
