using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PuzzleEndController : MonoBehaviour
{
    //public PuzzleDialogueSO puzzleSO;
    public EndPuzzleSO puzzleSO;
    public GameObject DialogueCanvas;
    [SerializeField] private GameObject confirmationButtons;
    //public TextMeshProUGUI zodiacName;
    public TextMeshProUGUI zodiacDialogue;
    public GameObject leaveButton;
    public Image dialogueImage;
    private PlayerMovement playerMovementScript;
    private CameraController camControllerScript;
    private bool isLeaving;
    private int levelNumber => Managers.ScenesManager.levelNumber;

    private System.Action exitAction;

    // Start is called before the first frame update
    void Start()
    {
        isLeaving = false;
        DialogueCanvas.SetActive(false);
        playerMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        camControllerScript = GameObject.FindGameObjectWithTag("PlayerCam").GetComponent<CameraController>();
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Pause") > 0)
        {
            StayInPuzzle();
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


    public void ExitPuzzle(System.Action exitAction)
    {
        this.exitAction = exitAction;

        if (Managers.ProgressManager.IsStarCollected(levelNumber, 0) && Managers.ProgressManager.IsStarCollected(levelNumber, 1))
        {
            Debug.Log("Trigger Two Star Dialogue");
            DialogueCanvas.SetActive(true);
            dialogueImage.sprite = puzzleSO.puzzles[levelNumber].zodiacTwoSprite;
            zodiacDialogue.text = puzzleSO.puzzles[levelNumber].TwoStarDialogue;
            confirmationButtons.SetActive(true);
            confirmationButtons.transform.GetChild(1).gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(leaveButton);
        }
        else if (Managers.ProgressManager.IsStarCollected(levelNumber, 0) || Managers.ProgressManager.IsStarCollected(levelNumber, 1))
        {
            Debug.Log("Trigger One Star Dialogue");
            DialogueCanvas.SetActive(true);
            dialogueImage.sprite = puzzleSO.puzzles[levelNumber].zodiacOneSprite;
            zodiacDialogue.text = puzzleSO.puzzles[levelNumber].OneStarDialogue;
            confirmationButtons.SetActive(true);
            EventSystem.current.SetSelectedGameObject(leaveButton);
        }
        else if (!Managers.ProgressManager.IsStarCollected(levelNumber, 0) && !Managers.ProgressManager.IsStarCollected(levelNumber, 1))
        {
            Debug.Log("Leave Puzzle Without Collecting Stars");
            LeaveToHub();
        }
    }

    public void LeaveToHub()
    {
        if (!isLeaving)
        {
            isLeaving = true;
            Invoke("ResetLeave", 3f);
            DialogueCanvas.SetActive(false);
            
            if (exitAction == null) {
                Debug.LogError("Hi Jiaxin, if this has happened then i have sorely misunderstood how your code works and would like some help");
            }
            exitAction?.Invoke();
            exitAction = null;
        }
    }

    public void StayInPuzzle()
    {
        confirmationButtons.SetActive(false);
        DialogueCanvas.SetActive(false);
    }

    private void ResetLeave()
    {
        isLeaving = false;
    }
}
