using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;
    public static bool isTelescopeOn = false;
    public GameObject PauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject levelsMenuUI;
    public GameObject controlsMenuUI;
    public AudioMixer audioMixer;

    private GameObject currentMenu;
    

    // Update is called once per frame

    private void Start()
    {
        currentMenu = null;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !isTelescopeOn)
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
        //switch to the settings menu
        PauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
        currentMenu = settingsMenuUI;
        
    }

    public void LoadLevelSelect()
    {
        //switch to the level select menu (which has its own script)
        PauseMenuUI.SetActive(false);
        levelsMenuUI.SetActive(true);
        currentMenu = levelsMenuUI;

    }
    public void LoadControls()
    {
        //switch to the controls menu
        PauseMenuUI.SetActive(false);
        controlsMenuUI.SetActive(true);
        currentMenu = controlsMenuUI;

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
