using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DoorManager : MonoBehaviour
{
    //needed so that doors open status can be globally controlled
    private bool doorOpen = false;
    public void switchDoor()
    {
        doorOpen = !doorOpen;
    }

    public bool getDoorStatus()
    {
        return doorOpen;
    }
}
