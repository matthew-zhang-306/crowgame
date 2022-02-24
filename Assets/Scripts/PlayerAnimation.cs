using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody myRB;
    // way the player is facing
    // 1 = front
    // 2 = left
    // 3 = right
    // 4 = back
    //private int direction;

    private float peckTime = 0.15f;
    private float peckCount = 0.15f;
    private bool isPecking;
    private float gustTime = 0.25f;
    private float gustCount = 0.25f;
    private bool isGusting;

    public bool isInsideTornado = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        myRB = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       //  Debug.Log("Axis: " + Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical") + Input.GetAxisRaw("Action1") + Input.GetAxisRaw("Action2"));
        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));

        // set the last move float to last input
        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            anim.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));
            anim.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
        }

        // pecking animation timer
        if (isPecking)
        {
            // myRB.velocity = Vector2.zero;
            peckCount -= Time.deltaTime;
            if (peckCount <= 0)
            {
                anim.SetBool("isPecking", false);
                isPecking = false;
            }
        }

        if (Input.GetAxisRaw("Action1") > 0)
        {
            peckCount = peckTime;
            anim.SetBool("isPecking", true);
            isPecking = true;
        }

        // Gust wind animation timer
        if (isGusting)
        {
            // myRB.velocity = Vector2.zero;
            gustCount -= Time.deltaTime;
            if (gustCount <= 0)
            {
                anim.SetBool("isGusting", false);
                isGusting = false;
            }
        }

        if (Input.GetAxisRaw("Action2") > 0)
        {
            gustCount = gustTime;
            anim.SetBool("isGusting", true);
            isGusting = true;
        }

        if (isInsideTornado)
        {
            anim.SetBool("isRising", true);
        }
        else
        {
            anim.SetBool("isRising", false);
        }

        if (myRB.velocity.y < -0.1)
        {
            anim.SetBool("isFalling", true);
        }
        else
        {
            anim.SetBool("isFalling", false);
        }


        /*
        Debug.Log("Velocity: " + GetComponentInParent<Rigidbody>().velocity);
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
        }*/
    }
}
