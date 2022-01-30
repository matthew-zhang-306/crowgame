using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour
{
    private float posMoved = 0;
    public GameObject door;
    private bool isInside;
    private float initYPos;
    private bool isMoving;

    private void Start()
    {
        initYPos = door.transform.position.y;
        isMoving = false;
    }
    private void Update()
    {
        //Debug.Log(posMoved);
        //Debug.Log(door.transform.position.y);
        //Debug.Log("Init: " + initYPos);
        if (isInside)
        {
            if (door.transform.position.y < initYPos + 1.5f)
            {
                door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y + 0.01f, door.transform.position.z);
                posMoved += 0.01f;
            }

        }
        else
        {
            if (posMoved != 0)
            {
                door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y - 0.01f, door.transform.position.z);
                posMoved -= 0.01f;
                if (posMoved < 0)
                {
                    posMoved = 0;
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        isInside = true;
        Debug.Log("Entered");
    }

    private void OnTriggerExit(Collider other)
    {
        isInside = false;
        Debug.Log("Exited");
    }
}
