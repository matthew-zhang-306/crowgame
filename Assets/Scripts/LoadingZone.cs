using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingZone : MonoBehaviour
{
    public LevelListSO levelList;

    public int targetHubIndex;

    public ExitPoint exitPoint;
    public string targetExitName => exitPoint.exitName;

    public Vector3 walkDirection => transform.forward;
    
    public int musicIndex;


    private void Start() {
        if (targetExitName == "unnamed") {
            Debug.LogWarning("You added a LoadingZone to this scene without naming its ExitPoint. You probably intended to name it something so that you can link it to another scene");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerMovement>()?.SetLoadingZone(this);
            
            Managers.ScenesManager.destinationExit = targetExitName;
            Managers.ScenesManager.ChangeScene(levelList.hubs[targetHubIndex].sceneName);
        }
    }

}
