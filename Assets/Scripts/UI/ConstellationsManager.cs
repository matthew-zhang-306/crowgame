using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstellationsManager : MonoBehaviour
{
    public CanvasGroup telescopeGroup;
    public Transform starContainerOne; // all of the stars are children of this object
    //public Transform starContainerTwo;
    public Button starPaperOne;
    //public Button starPaperTwo;
    public Transform ravenConstellation;
    private Animator telescopeAnim;
    private PlayerMovement playerMovementScript;
    private CameraController camControllerScript;

    private bool menuInput;
    private bool oldMenuInput;
    //private bool paperInput;
    //private bool oldPaperInput;


    void Start()
    {
        playerMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        camControllerScript = GameObject.FindGameObjectWithTag("PlayerCam").GetComponent<CameraController>();
        //isTelescopeOpened = false;
        telescopeAnim = GetComponent<Animator>();
        telescopeGroup.interactable = false;

        LevelListSO levelList = Managers.ProgressManager.levelList;
        for (int i = 0; i < starContainerOne.childCount; i++) {
            Transform starT = starContainerOne.GetChild(i);
            starT.GetChild(0).gameObject.SetActive(
                Managers.ProgressManager.IsStarCollected(i / levelList.starsPerLevel, i % levelList.starsPerLevel)
            );
        }
        //for (int i = 0; i < starContainerOne.childCount; i++)
        //{
        //    Transform starT = starContainerTwo.GetChild(i);
        //    starT.GetChild(0).gameObject.SetActive(
        //        Managers.ProgressManager.IsStarCollected((i + 12) / levelList.starsPerLevel, (i + 12) % levelList.starsPerLevel)
        //    );
        //}
        for (int i = 0; i < ravenConstellation.childCount - 1; i += 2)
        {
            Transform starT = ravenConstellation.GetChild(i);
            Transform starTwo = ravenConstellation.GetChild(i + 1);
            if (Managers.ProgressManager.IsStarCollected(i/2, 0))
            {
                starT.GetChild(1).gameObject.SetActive(true);
            }
            if (Managers.ProgressManager.IsStarCollected(i/2, 1))
            {
                starTwo.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    void Update()
    {
        oldMenuInput = menuInput;
        menuInput = Input.GetAxisRaw("Submenu") > 0;

        //oldPaperInput = paperInput;
        //paperInput = Mathf.Abs(Input.GetAxis("Camera")) > 0.5f;

        if (menuInput && !oldMenuInput)
        {
            ChangeTelescopeState();
        }

        //if (PauseMenu.isTelescopeOn && paperInput && !oldPaperInput) {
        //    ChangePaperState();
        //}
    }

    public void ChangeTelescopeState()
    {
        if (PauseMenu.isTelescopeOn && !PauseMenu.gamePaused)
        {
            telescopeAnim.Play("CloseTelescope");
            playerMovementScript.enabled = true;
            camControllerScript.enabled = true;
            PauseMenu.isTelescopeOn = false;
            telescopeGroup.interactable = false;
        }
        else if (!PauseMenu.isTelescopeOn && !PauseMenu.gamePaused)
        {
            telescopeAnim.Play("OpenTelescope");
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity = Vector3.zero;
            playerMovementScript.enabled = false;
            camControllerScript.enabled = false;
            PauseMenu.isTelescopeOn = true;
            telescopeGroup.interactable = true;
        }
    }

    //public void ChangePaperState()
    //{
    //    // swap sibling indexes to change sort order
    //    int a = starPaperOne.transform.GetSiblingIndex();
    //    int b = starPaperTwo.transform.GetSiblingIndex();
    //    starPaperOne.transform.SetSiblingIndex(b);
    //    starPaperTwo.transform.SetSiblingIndex(a);

    //    // change interactability
    //    starPaperOne.interactable = !starPaperOne.interactable;
    //    starPaperTwo.interactable = !starPaperTwo.interactable;
    //}
}
