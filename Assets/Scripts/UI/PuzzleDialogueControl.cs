using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PuzzleDialogueControl : MonoBehaviour
{
    public PuzzleDialogueSO puzzleSO;
    public GameObject DialogueCanvas;
    [SerializeField] private GameObject confirmationButtons;
    public TextMeshProUGUI zodiacName;
    public TextMeshProUGUI zodiacDialogue;
    public GameObject leaveButton;
    public Image dialogueImage;
    private bool isRunning;
    private bool finishedIntro;
    private bool introActive;
    public bool triggeredDialogue;
    private PlayerMovement playerMovementScript;
    private CameraController camControllerScript;
    private int levelNumber => Managers.ScenesManager.levelNumber;

    // Start is called before the first frame update
    void Start()
    {
        playerMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        camControllerScript = GameObject.FindGameObjectWithTag("PlayerCam").GetComponent<CameraController>();
        finishedIntro = false;
        introActive = false;
        triggeredDialogue = false;
        Debug.Log(levelNumber);
        Debug.Log(Managers.ProgressManager.GetLevelVisited(levelNumber));
        if (Managers.ProgressManager.GetLevelVisited(levelNumber))
        {
            introActive = true;
            DialogueCanvas.SetActive(true);
            zodiacName.text = puzzleSO.zodiacs[levelNumber].zodiacOneName;
            dialogueImage.sprite = puzzleSO.zodiacs[levelNumber].zodiacOneSprite;
            zodiacDialogue.text = "";
            string dialogue = puzzleSO.zodiacs[levelNumber].IntroDialogueOne;
            StartCoroutine(TypeDialogue(dialogue, 0.001f));
        }
    }

    private void DisplayNextSentence()
    {
        if (!finishedIntro)
        {
            Debug.Log("Next Intro Dialogue");
            StopAllCoroutines();
            zodiacName.text = puzzleSO.zodiacs[levelNumber].zodiacTwoName;
            dialogueImage.sprite = puzzleSO.zodiacs[levelNumber].zodiacTwoSprite;
            zodiacDialogue.text = "";
            string nextDialogue = puzzleSO.zodiacs[levelNumber].IntroDialogueTwo;
            StartCoroutine(TypeDialogue(nextDialogue, 0.001f));
            finishedIntro = true;
        }
        else
        {
            DialogueCanvas.SetActive(false);
            introActive = false;
        }
    }

    private void Update()
    {
        if (!isRunning && Input.GetAxisRaw("Action1") > 0 && introActive)
        {
            DisplayNextSentence();
        }

        if (DialogueCanvas.activeInHierarchy)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity = Vector3.zero;
            playerMovementScript.enabled = false;
            camControllerScript.enabled = false;
        }
        else
        {
            playerMovementScript.enabled = true;
            camControllerScript.enabled = true;
        }
    }


    public void ExitPuzzle()
    {
        if (Managers.ProgressManager.IsStarCollected(levelNumber, 0) && Managers.ProgressManager.IsStarCollected(levelNumber, 1))
        {
            Debug.Log("Trigger Two Star Dialogue");
            DialogueCanvas.SetActive(true);
            zodiacName.text = puzzleSO.zodiacs[levelNumber].zodiacTwoName;
            dialogueImage.sprite = puzzleSO.zodiacs[levelNumber].zodiacTwoSprite;
            zodiacDialogue.text = "";
            string dialogue = puzzleSO.zodiacs[levelNumber].TwoStarDialogue;
            StartCoroutine(TypeDialogue(dialogue, 0.001f));
            Invoke("ShowButtons", 1);
        }
        else if (Managers.ProgressManager.IsStarCollected(levelNumber, 0) || Managers.ProgressManager.IsStarCollected(levelNumber, 1))
        {
            Debug.Log("Trigger One Star Dialogue");
            //triggeredDialogue = true;
            DialogueCanvas.SetActive(true);
            zodiacName.text = puzzleSO.zodiacs[levelNumber].zodiacOneName;
            dialogueImage.sprite = puzzleSO.zodiacs[levelNumber].zodiacOneSprite;
            zodiacDialogue.text = "";
            string dialogue = puzzleSO.zodiacs[levelNumber].OneStarDialogue;
            StartCoroutine(TypeDialogue(dialogue, 0.001f));
            Invoke("ShowButtons", 1);
        }
        else if (!Managers.ProgressManager.IsStarCollected(levelNumber, 0) && !Managers.ProgressManager.IsStarCollected(levelNumber, 1))
        {
            Debug.Log("Leave Puzzle Without Collecting Stars");
            LeaveToHub();
        }
    }
    private IEnumerator TypeDialogue(string dialogue, float speed)
    {
        isRunning = true;
        foreach (char letter in dialogue.ToCharArray())
        {
            zodiacDialogue.text += letter;
            yield return new WaitForSeconds(speed);
        }
        isRunning = false;
    }

    private void ShowButtons()
    {
        confirmationButtons.SetActive(true);
        EventSystem.current.SetSelectedGameObject(leaveButton);
    }

    public void LeaveToHub()
    {
        DOTween.Sequence().InsertCallback(
                1.0f, () => Managers.ScenesManager.ChangeScene("Hub-World"));
    }

    public void StayInPuzzle()
    {
        confirmationButtons.SetActive(false);
        triggeredDialogue = false;
        DialogueCanvas.SetActive(false);
    }
}
