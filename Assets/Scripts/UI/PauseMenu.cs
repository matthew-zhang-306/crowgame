using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;
    public static bool isTelescopeOn = false;
    public AudioMixer audioMixer;

    [Header("Menus")]
    public GameObject PauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject levelsMenuUI;
    public GameObject controlsMenuUI;

    [Header("References to buttons, for navigation")]
    public GameObject pauseFirstButton;
    public GameObject settingsFirstButton;
    public GameObject levelsFirstButton;
    public GameObject controlsFirstButton;

    private GameObject currentMenu;

    private bool pauseInput;
    private bool oldPauseInput;
    

    // Update is called once per frame

    private void Start()
    {
        currentMenu = null;
    }

    void Update()
    {
        oldPauseInput = pauseInput;
        pauseInput = Input.GetAxisRaw("Pause") > 0;

        if (pauseInput && !oldPauseInput && !isTelescopeOn && !Managers.ScenesManager.IsTransitioning)
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

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    public void LoadSettings()
    {
        //switch to the settings menu
        PauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
        currentMenu = settingsMenuUI;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);   
    }

    public void LoadLevelSelect()
    {
        //switch to the level select menu (which has its own script)
        PauseMenuUI.SetActive(false);
        levelsMenuUI.SetActive(true);
        currentMenu = levelsMenuUI;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(levelsFirstButton);
    }
    public void LoadControls()
    {
        //switch to the controls menu
        PauseMenuUI.SetActive(false);
        controlsMenuUI.SetActive(true);
        currentMenu = controlsMenuUI;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(controlsFirstButton);
    }
    public void QuitGame()
    {
        Application.Quit();
    }


    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);
    }
}
