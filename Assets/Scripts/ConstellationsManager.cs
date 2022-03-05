using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationsManager : MonoBehaviour
{
    public Transform starContainer; // all of the stars are children of this object
    public Transform starContainerTwo;
    public Transform ravenConstellation;
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
        for (int i = 0; i < starContainer.childCount; i++)
        {
            Transform starT = starContainerTwo.GetChild(i);
            starT.GetChild(0).gameObject.SetActive(
                Managers.ProgressManager.IsStarCollected((i + 12) / levelList.starsPerLevel, (i + 12) % levelList.starsPerLevel)
            );
        }
        for (int i = 0; i < ravenConstellation.childCount; i++)
        {
            Transform starT = ravenConstellation.GetChild(i);
            if ((Managers.ProgressManager.IsStarCollected(i, 0)) &&
                (Managers.ProgressManager.IsStarCollected(i, 1)))
            {
                starT.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeTelescopeState();
        }
        /* button click set star for testing
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Managers.ProgressManager.SetStarCollected(11, 0, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Managers.ProgressManager.SetStarCollected(11, 1, true);
        }*/
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
