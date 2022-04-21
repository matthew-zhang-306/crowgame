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
        anim.SetFloat("MoveX", Input.GetAxis("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxis("Vertical"));

        // set the last move float to last input
        if (Input.GetAxis("Horizontal") > 0.5f || Input.GetAxis("Horizontal") < -0.5f || Input.GetAxis("Vertical") > 0.5f || Input.GetAxis("Vertical") < -0.5f)
        {
            anim.SetFloat("LastMoveX", Input.GetAxis("Horizontal"));
            anim.SetFloat("LastMoveY", Input.GetAxis("Vertical"));
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

        if (myRB.velocity.y < -1)
        {
            anim.SetBool("isFalling", true);
        }
        else
        {
            anim.SetBool("isFalling", false);
        }

    }
}
