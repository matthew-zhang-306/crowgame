using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefsX.GetBool("hasPlayed", false))
        {
            this.gameObject.GetComponent<Button>().interactable = true;
            Debug.Log("played");
        }
        else
        {
            this.gameObject.GetComponent<Button>().interactable = false;
            Debug.Log("not played");
        }
    }
}
