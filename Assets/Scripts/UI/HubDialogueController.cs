using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HubDialogueController : MonoBehaviour
{
    [SerializeField] private GameObject dialogueCamera;
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    public HubDialogueSO hubDialogue;
    private int zodiacIndex;
    private bool isTalking;
    private bool isRunning;

    private bool input;
    private bool oldInput;
    private int dialogueIdx;

    // Start is called before the first frame update
    void Start()
    {
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
                //Debug.Log(this.gameObject.name + " is in index " + zodiacIndex);
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
                dialogueCamera.SetActive(true);
                dialogueCanvas.SetActive(true);
                Debug.Log("Talk with " + this.gameObject.name);
                if (!isRunning)
                {
                    DisplayDialogue();
                }
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

    private IEnumerator TypeSentence(string dialogue, float speed = 0.0001f)
    {
        isRunning = true;
        dialogueText.text = "";
        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(speed);
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
        //Debug.Log("Exited " + this.gameObject.name + " space.");
        dialogueCamera.SetActive(false);
        dialogueCanvas.SetActive(false);
        dialogueIdx = -1;
    }
}
