using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TutorialTrigger : MonoBehaviour
{

    [Header("Parameters")]
    public string XboxButton;

    public string button;
    public string message;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var indicator = other.GetComponent<PlayerMovement>()?.actionIndicator;

            #if UNITY_WSA
                //xbox version of text
                button = XboxButton;
            #endif

            indicator?.Show(button, message);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var indicator = other.GetComponent<PlayerMovement>()?.actionIndicator;
            indicator?.Hide();
        }
    }
}


