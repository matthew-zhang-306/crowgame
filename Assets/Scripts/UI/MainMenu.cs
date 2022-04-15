using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    public GameObject startPanel;
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;
    public GameObject confirmationPanel;
    public GameObject playPanel;

    private Animator animator;
    private bool backInput;
    private bool oldBackInput;
    private System.Action onBackInput;

    private void Start() {
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
        Managers.ProgressManager.ResetPreviousLevel();
        Managers.ProgressManager.ResetStarsCollected();
        Managers.ScenesManager.ResetScene();
    }

    public void NewGame()
    {
        Managers.ProgressManager.ResetPreviousLevel();
        Managers.ProgressManager.ResetStarsCollected();
        Managers.ScenesManager.ChangeScene("IntroCutscene");
    }

    public void LoadGame()
    {
        Managers.ScenesManager.ChangeScene("Hub-World");
    }
}
