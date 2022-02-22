using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DoorManager : MonoBehaviour
{
    public bool startsOpen = false;
    private bool doorOpen = false;
    
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    
        if (startsOpen) {
            Switch();
        }
    }


    public void Switch() {
        doorOpen = !doorOpen;

        if (doorOpen)
        {
            animator.Play("DoorOpen", 0, 0f);
        }
        else
        {
            animator.Play("DoorClose", 0, 0f);
        }
    }
}
