using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(waiter());           
        }


        IEnumerator waiter()
        {
            //play sound and wait to finish before loading next scene
            FindObjectOfType<AudioManager>().PlaySound("Victory");

            yield return new WaitForSeconds(1);

            SceneManager.LoadScene("Hub-World");
        }
    }
}
