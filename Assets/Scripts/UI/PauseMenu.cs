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
    public GameObject audioMenuUI;
    public GameObject graphicsMenuUI;
    public GameObject levelsMenuUI;
    public GameObject controlsMenuUI;

    [Header("References to buttons, for navigation")]
    public GameObject pauseFirstButton;
    public GameObject settingsFirstButton;
    public GameObject audioFirstButton;
    public GameObject graphicsFirstButton;
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
        settingsMenuUI.SetActive(false);
        currentMenu.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        currentMenu = null;
    }

    public void Pause()
    {
        settingsMenuUI.SetActive(false);
        currentMenu = PauseMenuUI;
        PauseMenuUI.SetActive(true);
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
        settingsMenuUI.SetActive(false);
        controlsMenuUI.SetActive(true);
        currentMenu = controlsMenuUI;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(controlsFirstButton);
        onBackInput = CloseControls;
    }

    public void CloseControls()
    {
        controlsMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
        currentMenu = settingsMenuUI;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
        onBackInput = null;
    }

    public void LoadAudio()
    {
        //switch to the audio menu
        settingsMenuUI.SetActive(false);
        audioMenuUI.SetActive(true);
        currentMenu = audioMenuUI;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(audioFirstButton);
        onBackInput = CloseAudio;
    }

    public void CloseAudio()
    {
        audioMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
        currentMenu = settingsMenuUI;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
        onBackInput = null;
    }

    public void LoadGraphics()
    {
        //switch to the graphics menu
        settingsMenuUI.SetActive(false);
        graphicsMenuUI.SetActive(true);
        currentMenu = graphicsMenuUI;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(graphicsFirstButton);
        onBackInput = CloseGraphics;
    }

    public void CloseGraphics()
    {
        graphicsMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
        currentMenu = settingsMenuUI;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
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
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
    }

    public void buttonClick()
    {
        Managers.AudioManager.PlaySound("ui_button_press");
    }

    public void buttonHover()
    {
        Managers.AudioManager.PlaySound("ui_button_hover");
    }

    // set the control description based on which platform we are using
    public void SetControls()
    {
        string ControlDescription = "Arrows / WASD - Movement\n" +
                                    "Q / E - Rotate Camera\n" +
                                    "Space - Peck\n" +
                                    "F - Tornado\n" +
                                    "R - Restart Level\n" +
                                    "T - Telescope (HUB ONLY)\n" +
                                    "ESC - Pause";
        //using xbox
        #if UNITY_WSA
            ControlDescription =    "Joystick - Movement\n" +
                                    "LB / RB - Rotate Camera\n" +
                                    "A - Peck\n" +
                                    "X - Tornado\n" +
                                    "Y - Restart Level\n" +
                                    "Select - Telescope (HUB ONLY)\n" +
                                    "Start - Pause";
        #endif

        controlsText.text = ControlDescription;
    }
}
