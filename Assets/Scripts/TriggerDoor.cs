using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    // this should be a DoorManager reference but i'm going to break things if i change it
    [SerializeField] private GameObject door = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box")) {
            door.GetComponent<DoorManager>().Switch();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box")) { 
            //if a pressure plate door only stays open while player inside
            //if a button nothing needs to be done on exit, stay in position
            if (this.CompareTag("PressurePlate"))
            {
                door.GetComponent<DoorManager>().Switch();
            }
        }
    }
}
