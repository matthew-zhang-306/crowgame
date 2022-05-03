using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class HubDialogueController : MonoBehaviour
{
    [SerializeField] private GameObject dialogueCamera;
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    public HubDialogueSO hubDialogue;
    private Animator anim;
    private int zodiacIndex;
    private bool isTalking;
    private bool isRunning;

    private bool input;
    private bool oldInput;
    private int dialogueIdx;
    public int textSpeed = 100;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.gameObject.GetComponentInChildren<Animator>();
        dialogueCamera.SetActive(false);
        dialogueCanvas.SetActive(false);
        dialogueIdx = -1;
        isTalking = false;
        dialogueText.text = "";
        for (int i = 0; i < hubDialogue.zodiacs.Length; i++)
        {
            if (this.gameObject.name == hubDialogue.zodiacs[i].zodiacName)
            {
                zodiacIndex = i;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        oldInput = input;
        input = Input.GetAxisRaw("Action1") > 0;

        if (isTalking)
        {
            if (input && !oldInput)
            {
                anim.SetBool("isTalking", true);
                dialogueCamera.SetActive(true);
                dialogueCanvas.SetActive(true);
                Managers.AudioManager.PlaySound("ui_button_hover");
                
                if (!isRunning)
                {
                    DisplayDialogue();
                }
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().actionIndicator.Hide();
            }
        }
    }

    public void DisplayDialogue()
    {
        dialogueIdx++;
        if (!isRunning)
        {
            if (dialogueIdx >= hubDialogue.zodiacs[zodiacIndex].Dialogue.Length)
            {
                EndTalk();
                return;
            }
            StopAllCoroutines();
            StartCoroutine(TypeSentence(hubDialogue.zodiacs[zodiacIndex].Dialogue[dialogueIdx]));
        }
    }

    private IEnumerator TypeSentence(string dialogue)
    {
        isRunning = true;
        dialogueText.text = dialogue;
        dialogueText.maxVisibleCharacters = 0;

        bool input = Input.GetAxisRaw("Action1") > 0;
        bool oldInput = input;

        for (float t = 0; dialogueText.maxVisibleCharacters < dialogue.Length; t += Time.deltaTime)
        {
            dialogueText.maxVisibleCharacters = (int)(t * textSpeed);

            if (input && !oldInput)
            {
                // consume input
                oldInput = input;
                dialogueText.maxVisibleCharacters = dialogue.Length;
            }

            yield return null;
        }
        isRunning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            string TalkButton = "Space";
#if UNITY_WSA
                //xbox version of control
                TalkButton = "A";
#endif
            isTalking = true;
            
            other.GetComponent<PlayerMovement>().actionIndicator.Show(TalkButton, "Talk");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isTalking = false;
            EndTalk();
            other.GetComponent<PlayerMovement>().actionIndicator.Hide();
        }
    }

    private void EndTalk()
    {
        anim.SetBool("isTalking", false);
        dialogueCamera.SetActive(false);
        dialogueCanvas.SetActive(false);
        dialogueIdx = -1;
    }
    private void EndTutorial()
    {
        anim.SetBool("isTalking", false);
        DOTween.Sequence().InsertCallback(
                1.0f, () => Managers.ScenesManager.ChangeScene("Hub-World"));
    }
}
