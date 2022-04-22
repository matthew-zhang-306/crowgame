using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoor : BaseSwitch
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box")) {
            Switch();
            //play sound on enter for both pressure plate and button
            Managers.AudioManager.PlaySound("button_press");
            transform.GetComponentInChildren<ParticleSystem>().Stop();
        }
            
           
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box")) { 
            //if a pressure plate door only stays open while player inside
            //if a button nothing needs to be done on exit, stay in position
            if (this.CompareTag("PressurePlate"))
            {
                //only play sound on exit if it is a pressure plate
                Managers.AudioManager.PlaySound("button_press");
                transform.GetComponentInChildren<ParticleSystem>().Play();
                Switch();
            }
        }
    }
}
