using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("I goddamn love UI programming")]
    public GameObject startButton;
    public GameObject playButton;
    public GameObject optionsButton;
    public GameObject creditsButton;
    public GameObject optionsFirstSelected;
    public GameObject creditsFirstSelected;
    public GameObject confirmationFirstSelected;
    public GameObject playFirstSelected;
    public GameObject audioFirstSelected;
    public GameObject graphicsFirstSelected;
    public GameObject controlsFirstSelected;

    public GameObject startPanel;
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;
    public GameObject confirmationPanel;
    public GameObject playPanel;
    public GameObject settingsPanel;
    public GameObject audioPanel;
    public GameObject graphicsPanel;
    public GameObject controlsPanel;

    public TextMeshProUGUI controlsText;

    private Animator animator;
    private bool backInput;
    private bool oldBackInput;
    private System.Action onBackInput;

    private void Start() {
        SetControls();
        EventSystem.current.SetSelectedGameObject(startButton);
    }

    private void Update() {
        animator = this.gameObject.GetComponent<Animator>();
        oldBackInput = backInput;
        backInput = Input.GetAxisRaw("Cancel") > 0;

        if (backInput && !oldBackInput) {
            onBackInput?.Invoke();
        }
    }

    public void OpenMainMenu()
    {
        //startPanel.SetActive(false);
        //mainPanel.SetActive(true);
        animator.Play("OpenMainMenu");

        EventSystem.current.SetSelectedGameObject(playButton);
        playButton.transform.localScale = new Vector3(1.1f, 1.1f, 1);
    }

    
    public void OpenOptions() {
        //mainPanel.SetActive(false);
        //optionsPanel.SetActive(true);
        settingsPanel.SetActive(true);
        audioPanel.SetActive(false);
        graphicsPanel.SetActive(false);
        controlsPanel.SetActive(false);
        animator.Play("OpenOptions");
        
        EventSystem.current.SetSelectedGameObject(optionsFirstSelected);
        onBackInput = CloseOptions;
    }
    public void CloseOptions() {
        //mainPanel.SetActive(true);
        //optionsPanel.SetActive(false);
        animator.Play("CloseOptions");

        EventSystem.current.SetSelectedGameObject(optionsButton);
        optionsButton.transform.localScale = new Vector3(1.1f, 1.1f, 1);
        onBackInput = null;
    }

    public void LoadAudio()
    {
        settingsPanel.SetActive(false);
        audioPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(audioFirstSelected);
        onBackInput = CloseAudio;
    }

    public void CloseAudio()
    {
        audioPanel.SetActive(false);
        settingsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(optionsFirstSelected);
        onBackInput = null;
    }

    public void LoadGraphcs()
    {
        settingsPanel.SetActive(false);
        graphicsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(graphicsFirstSelected);
        onBackInput = CloseGraphics;
    }

    public void CloseGraphics()
    {
        graphicsPanel.SetActive(false);
        settingsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(optionsFirstSelected);
        onBackInput = null;
    }

    public void LoadControls()
    {
        settingsPanel.SetActive(false);
        controlsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(controlsFirstSelected);
        onBackInput = CloseControls;
    }

    public void CloseControls()
    {
        controlsPanel.SetActive(false);
        settingsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(optionsFirstSelected);
        onBackInput = null;
    }
    public void OpenCredits() {
        //mainPanel.SetActive(false);
        //creditsPanel.SetActive(true);
        animator.Play("OpenCredits");

        EventSystem.current.SetSelectedGameObject(creditsFirstSelected);
        onBackInput = CloseCredits;
    }
    public void CloseCredits() {
        //mainPanel.SetActive(true);
        //creditsPanel.SetActive(false);
        animator.Play("CloseCredits");

        EventSystem.current.SetSelectedGameObject(creditsButton);
        creditsButton.transform.localScale = new Vector3(1.1f, 1.1f, 1);
        onBackInput = null;
    }

    public void OpenConfirmation()
    {
        confirmationPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(confirmationFirstSelected);
        onBackInput = CloseConfirmation;
    }

    public void CloseConfirmation()
    {
        confirmationPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(playFirstSelected);
    }
    public void OpenPlay()
    {
        animator.Play("OpenPlay");

        EventSystem.current.SetSelectedGameObject(playFirstSelected);
        onBackInput = ClosePlay;
    }
    public void ClosePlay()
    {
        //mainPanel.SetActive(true);
        //optionsPanel.SetActive(false);
        animator.Play("ClosePlay");

        EventSystem.current.SetSelectedGameObject(playButton);
        playButton.transform.localScale = new Vector3(1.1f, 1.1f, 1);
        onBackInput = null;
    }

    public void ResetData()
    {
        Managers.ProgressManager.ResetSavePosition();
        Managers.ProgressManager.ResetStarsCollected();
        Managers.ScenesManager.ResetScene();
    }

    public void NewGame()
    {
        PlayerPrefsX.SetBool("hasPlayed", false);
        Managers.ProgressManager.ResetSavePosition();
        Managers.ProgressManager.ResetStarsCollected();
        Managers.ScenesManager.ChangeScene("IntroCutscene");
    }

    public void LoadGame()
    {
        Managers.ProgressManager.GetSavePosition(out string sceneName, out string exitName);
        Managers.ScenesManager.destinationExit = exitName;
        Managers.ScenesManager.ChangeScene(sceneName);
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
