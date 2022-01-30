using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    [SerializeField] private GameObject door = null;

    private void OnTriggerEnter(Collider other)
    {
        //for both pressure plate and button will move door when pressed
        //if a button is used the door logic may be reverse
        if (door.GetComponent<DoorManager>().getDoorStatus())
        {
            Debug.Log("closing the door");
            door.GetComponent<Animator>().Play("DoorClose", 0, 0f);
        }
        else
        {
            Debug.Log("opening the door");
            door.GetComponent<Animator>().Play("DoorOpen", 0, 0f);
        }
        door.GetComponent<DoorManager>().switchDoor();
    }

    private void OnTriggerExit(Collider other)
    {
        //if a pressure plate door only stays open while player inside
        //if a button nothing needs to be done on exit, stay in position

        if (this.CompareTag("PressurePlate"))
        {

            if (door.GetComponent<DoorManager>().getDoorStatus())
            {
                door.GetComponent<Animator>().Play("DoorClose", 0, 0f);
            }
            else
            {
                door.GetComponent<Animator>().Play("DoorOpen", 0, 0f);
            }
            door.GetComponent<DoorManager>().switchDoor();
        }
    }
}
