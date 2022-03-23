using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using TMPro;

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

    public TextMeshProUGUI controlsText;

    private GameObject currentMenu;

    private bool pauseInput;
    private bool oldPauseInput;
    private bool backInput;
    private bool oldBackInput;
    private System.Action onBackInput;


    // Update is called once per frame

    private void Start()
    {
        currentMenu = null;
        SetControls();
    }

    void Update()
    {
        oldPauseInput = pauseInput;
        pauseInput = Input.GetAxisRaw("Pause") > 0;

        oldBackInput = backInput;
        backInput = Input.GetAxisRaw("Cancel") > 0;

        if (backInput && !oldBackInput)
        {
            onBackInput?.Invoke();
        }

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
        onBackInput = CloseSettings;
    }

    public void CloseSettings()
    {
        settingsMenuUI.SetActive(false);
        PauseMenuUI.SetActive(true);
        currentMenu = PauseMenuUI;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        onBackInput = null;
    }

    public void LoadLevelSelect()
    {
        //switch to the level select menu (which has its own script)
        PauseMenuUI.SetActive(false);
        levelsMenuUI.SetActive(true);
        currentMenu = levelsMenuUI;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(levelsFirstButton);
        onBackInput = CloseSelect;
    }

    public void CloseSelect()
    {
        levelsMenuUI.SetActive(false);
        PauseMenuUI.SetActive(true);
        currentMenu = PauseMenuUI;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        onBackInput = null;
    }

    public void LoadControls()
    {
        //switch to the controls menu
        PauseMenuUI.SetActive(false);
        controlsMenuUI.SetActive(true);
        currentMenu = controlsMenuUI;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(controlsFirstButton);
        onBackInput = CloseControls;
    }

    public void CloseControls()
    {
        controlsMenuUI.SetActive(false);
        PauseMenuUI.SetActive(true);
        currentMenu = PauseMenuUI;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        onBackInput = null;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeScene()
    {
        gamePaused = false;
        currentMenu.SetActive(false);
        Time.timeScale = 1f;
        currentMenu = null;
        Managers.ScenesManager.ChangeScene("Menu");
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);
    }

    // set the control description based on which platform we are using
    public void SetControls()
    {
        string ControlDescription = "Arrows/WASD - Movement\n" +
                                    "L / R Shift - Camera\n" +
                                    "Space - Pecking\n" +
                                    "F - Tornado\n" +
                                    "R - Restart Scene\n" +
                                    "T - Open Telescope(HUB ONLY)\n" +
                                    "ESC - Pause";
        //using xbox
        #if UNITY_WSA
            ControlDescription =    "Joystick - Movement\n" +
                                    "LB / RB - Camera\n" +
                                    "A - Pecking\n" +
                                    "X - Tornado\n" +
                                    "Y - Restart Scene\n" +
                                    "[] - Open Telescope(HUB ONLY)\n" +
                                    "= - Pause";
        #endif

        controlsText.text = ControlDescription;
    }
}
