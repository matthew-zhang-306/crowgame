using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TutorialTrigger : MonoBehaviour
{

    [Header("References")]
    public GameObject TutorialCanvas;
    public GameObject TutorialText;

    [Header("Parameters")]
    public string message;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("collided with player");
            TutorialCanvas.SetActive(true);
            TutorialText.GetComponent<Text>().text = message;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("player left collider");
        if (other.tag == "Player")
        {
            TutorialCanvas.SetActive(false);
        }
    }
}


