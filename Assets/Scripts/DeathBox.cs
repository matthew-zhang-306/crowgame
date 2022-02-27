using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathBox : MonoBehaviour
{
    //if anything collides with the deathbox the scene is reset

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerMovement>()?.Die();
        }
    }
}
