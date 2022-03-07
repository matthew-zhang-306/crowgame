using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [Header("I goddamn love UI programming")]
    public GameObject playButton;
    public GameObject optionsButton;
    public GameObject creditsButton;
    public GameObject optionsFirstSelected;
    public GameObject creditsFirstSelected;
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;

    private bool backInput;
    private bool oldBackInput;
    private System.Action onBackInput;

    private void Start() {
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    private void Update() {
        oldBackInput = backInput;
        backInput = Input.GetAxisRaw("Cancel") > 0;

        if (backInput && !oldBackInput) {
            onBackInput?.Invoke();
        }
    }

    
    public void OpenOptions() {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
        
        EventSystem.current.SetSelectedGameObject(optionsFirstSelected);
        onBackInput = CloseOptions;
    }
    public void CloseOptions() {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(optionsButton);
        onBackInput = null;
    }
    public void OpenCredits() {
        mainPanel.SetActive(false);
        creditsPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(creditsFirstSelected);
        onBackInput = CloseCredits;
    }
    public void CloseCredits() {
        mainPanel.SetActive(true);
        creditsPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(creditsButton);
        onBackInput = null;
    }

}
