using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationsManager : MonoBehaviour
{
    //[SerializeField]private bool isTelescopeOpened;
    private Animator telescopeAnim;
    // Start is called before the first frame update
    void Start()
    {
        //isTelescopeOpened = false;
        telescopeAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeTelescopeState();
        }
    }

    public void ChangeTelescopeState()
    {
        if (PauseMenu.isTelescopeOn && !PauseMenu.gamePaused)
        {
            telescopeAnim.Play("CloseTelescope");
            PauseMenu.isTelescopeOn = false;
        }
        else if (!PauseMenu.isTelescopeOn && !PauseMenu.gamePaused)
        {
            telescopeAnim.Play("OpenTelescope");
            PauseMenu.isTelescopeOn = true;
        }
    }
}
