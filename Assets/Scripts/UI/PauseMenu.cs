using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;
    public GameObject PauseMenuUI;
    public GameObject settingsMenuUI;
    private GameObject currentMenu;

    // Update is called once per frame

    private void Start()
    {
        currentMenu = null;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gamePaused)
            {

                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {

        currentMenu.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        currentMenu = null;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        currentMenu = PauseMenuUI;
        Time.timeScale = 0;
        gamePaused = true;
    }

    public void LoadSettings()
    {
        //switch to the settings menu (which has its own script)
        PauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
        currentMenu = settingsMenuUI;
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
