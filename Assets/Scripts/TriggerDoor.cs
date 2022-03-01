using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoor : BaseSwitch
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box")) {
            Switch();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box")) { 
            //if a pressure plate door only stays open while player inside
            //if a button nothing needs to be done on exit, stay in position
            if (this.CompareTag("PressurePlate"))
            {
                Switch();
            }
        }
    }
}
