using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ZodiacController : MonoBehaviour
{
    [SerializeField] private GameObject dialogueCamera;
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    public ZodiacDialogueSO zodiacDialogue;
    private int zodiacIndex;
    private bool isTalking;
    private bool isRunning;
    private Animator anim;

    private bool input;
    private bool oldInput;
    private bool currentTalk;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.gameObject.GetComponentInChildren<Animator>();
        dialogueCamera.SetActive(false);
        dialogueCanvas.SetActive(false);
        isTalking = false;
        currentTalk = false;
        dialogueText.text = "";
        for(int i = 0; i < zodiacDialogue.zodiacs.Length; i++)
        {
            if (this.gameObject.name == zodiacDialogue.zodiacs[i].zodiacName)
            {
                zodiacIndex = i;
                Debug.Log(this.gameObject.name + " is in index " + zodiacIndex);
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
                Debug.Log("Talk with " + this.gameObject.name);
                if (!zodiacDialogue.zodiacs[zodiacIndex].haveTalkedTo)
                {
                    if (!isRunning)
                    {
                        if (!currentTalk)
                        {
                            dialogueText.text = "";
                            string dialogue = zodiacDialogue.zodiacs[zodiacIndex].firstDialogue;
                            zodiacDialogue.zodiacs[zodiacIndex].haveTalkedTo = true;
                            StartCoroutine(TypeDialogue(dialogue, 0.01f));
                            currentTalk = true;
                        }
                        else
                        {
                            EndTalk();
                        }

                    }
                }
                else
                {
                    if (!isRunning)
                    {
                        if (!currentTalk)
                        {
                            dialogueText.text = "";
                            int dialogueIndex = Random.Range(0, zodiacDialogue.zodiacs[zodiacIndex].randomDialogues.Length);
                            string dialogue = zodiacDialogue.zodiacs[zodiacIndex].randomDialogues[dialogueIndex];
                            StartCoroutine(TypeDialogue(dialogue, 0.01f));
                            currentTalk = true;
                        }
                        else
                        {
                            EndTalk();
                        }
                    }
                }
                Debug.Log("Current Talk: " + currentTalk);
            }
        }
    }

    private IEnumerator TypeDialogue(string dialogue, float speed)
    {
        isRunning = true;
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
        anim.SetBool("isTalking", false);
        Debug.Log("Exited " + this.gameObject.name + " space.");
        dialogueCamera.SetActive(false);
        dialogueCanvas.SetActive(false);
        dialogueText.text = "";
        currentTalk = false;
    }
}
