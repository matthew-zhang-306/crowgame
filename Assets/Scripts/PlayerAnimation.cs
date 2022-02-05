using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    // way the player is facing
    // 1 = front
    // 2 = left
    // 3 = right
    // 4 = back
    private int direction;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        direction = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            anim.SetBool("isFrontWalking", false);
            anim.SetBool("isBackWalking", false);
            anim.SetBool("isRightWalking", false);
            anim.SetBool("isLeftWalking", true);
            direction = 2;
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            anim.SetBool("isFrontWalking", false);
            anim.SetBool("isLeftWalking", false);
            anim.SetBool("isBackWalking", false);
            anim.SetBool("isRightWalking", true);
            direction = 3;
        }
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            anim.SetBool("isBackWalking", false);
            anim.SetBool("isLeftWalking", false);
            anim.SetBool("isRightWalking", false);
            anim.SetBool("isFrontWalking", true);
            direction = 1;
        }
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            anim.SetBool("isFrontWalking", false);
            anim.SetBool("isLeftWalking", false);
            anim.SetBool("isRightWalking", false);
            anim.SetBool("isBackWalking", true);
            direction = 4;
        }
        if (Input.GetKeyDown(KeyCode.X) || (Input.GetKeyDown(KeyCode.K)))//Input.GetAxisRaw("Action1") > 0)
        {
            if (direction == 1)
            {
                anim.SetTrigger("FrontPeck");
                Debug.Log("FrontPeck");
            }
            else if (direction == 2)
            {
                anim.SetTrigger("LeftPeck");
                Debug.Log("LeftPeck");
            }
            else if (direction == 3)
            {
                anim.SetTrigger("RightPeck");
                Debug.Log("RightPeck");
            }
            else if (direction == 4)
            {
                anim.SetTrigger("BackPeck");
                Debug.Log("BackPeck");
            }
        }
        if (Input.GetKeyDown(KeyCode.Z) || (Input.GetKeyDown(KeyCode.J)))//Input.GetAxisRaw("Action2") > 0)
        {
            if (direction == 1)
            {
                anim.SetTrigger("FrontGust");
                Debug.Log("FrontGust");
            }
            else if (direction == 2)
            {
                anim.SetTrigger("LeftGust");
                Debug.Log("LeftGust");
            }
            else if (direction == 3)
            {
                anim.SetTrigger("RightGust");
                Debug.Log("RightGust");
            }
            else if (direction == 4)
            {
                anim.SetTrigger("BackGust");
                Debug.Log("BackGust");
            }
        }
    }
}
