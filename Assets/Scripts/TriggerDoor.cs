using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;
    [SerializeField] private GameObject door = null;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            //for both pressure plate and button will move door when pressed
            //if a button is used the door logic may be reversed
            //Debug.Log("entering " + this.tag + doorOpen);
            if (door.GetComponent<DoorManager>().getDoorStatus())
            {
                Debug.Log("closing the door");
                myDoor.Play("DoorClose", 0, 0f);
            }
            else
            {
                Debug.Log("opening the door");
                myDoor.Play("DoorOpen", 0, 0f);
            }
            door.GetComponent<DoorManager>().switchDoor();
            //Debug.Log("entering " + this.tag + doorOpen);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        //if a pressure plate door only stays open while player inside
        //if a button nothing needs to be done on exit, stay in position
        //Debug.Log("leaving" + this.tag + doorOpen);
        if (other.CompareTag("Player"))
        {
            if (this.CompareTag("PressurePlate"))
            {

                //Debug.Log("leaving" + doorOpen);
                if (door.GetComponent<DoorManager>().getDoorStatus())
                {
                    Debug.Log("closing the door");
                    myDoor.Play("DoorClose", 0, 0f);
                }
                else
                {
                    Debug.Log("opening the door");
                    myDoor.Play("DoorOpen", 0, 0f);
                }
                door.GetComponent<DoorManager>().switchDoor();
            }
        }
       
        //Debug.Log("leaving" + this.tag + doorOpen);
    }
}
