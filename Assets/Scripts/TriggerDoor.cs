using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;

    //when the player collides with the button, open the door 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            myDoor.Play("DoorOpen", 0, 0f);
        }
    }

    //when the player steps off the button, close the door
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            myDoor.Play("DoorClose", 0, 0f);
        }
    }
}
