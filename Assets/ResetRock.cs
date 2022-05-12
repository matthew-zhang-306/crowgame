using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRock : MonoBehaviour
{

    public PushableBox[] boxes;
    

    

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Peck"))
        {
            //being pecked, reset
            Managers.AudioManager.PlaySound("meow");
            ResetBoxes();
        }
    }


    //this is called if the reset rock is pecked
    //reset each box attached to this rest rock to its original position
    public void ResetBoxes()
    {
        for(int i = 0; i < boxes.Length; i++){
            boxes[i].ResetBoxPosition();
        }
    }


}
