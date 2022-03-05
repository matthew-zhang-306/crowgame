using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationsManager : MonoBehaviour
{
    public Transform starContainer; // all of the stars are children of this object
    public Transform starContainerTwo;
    private Animator telescopeAnim;

    void Start()
    {
        //isTelescopeOpened = false;
        telescopeAnim = GetComponent<Animator>();
    

        LevelListSO levelList = Managers.ProgressManager.levelList;
        for (int i = 0; i < starContainer.childCount; i++) {
            Transform starT = starContainer.GetChild(i);
            starT.GetChild(0).gameObject.SetActive(
                Managers.ProgressManager.IsStarCollected(i / levelList.starsPerLevel, i % levelList.starsPerLevel)
            );
        }
        for (int i = 13; i < starContainer.childCount * 2; i++)
        {
            Transform starT = starContainerTwo.GetChild(i);
            starT.GetChild(0).gameObject.SetActive(
                Managers.ProgressManager.IsStarCollected(i / levelList.starsPerLevel, i % levelList.starsPerLevel)
            );
        }
    }

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
