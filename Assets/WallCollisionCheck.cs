using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollisionCheck : MonoBehaviour
{
    public bool colliding;
    int wallMask;

    public void Awake()
    {
        wallMask = LayerMask.GetMask("Wall", "Box");
    }
    public void OnTriggerEnter(Collider other)
    {
        if (Helpers.LayerMaskContains(wallMask, other.gameObject.layer))
        {
            Debug.Log("Colliding with wall");
            colliding = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        colliding = false;
    }
}
