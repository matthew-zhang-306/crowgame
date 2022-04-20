using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gong : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Peck"))
        {
            Debug.Log("Pecking gong");
            Managers.AudioManager.PlaySound("gong");
        }
    }
}
