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
    // Start is called before the first frame update
    void Start()
    {
        dialogueCamera.SetActive(false);
        dialogueCanvas.SetActive(false);
        isTalking = false;
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
        if (isTalking)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.J))
            {
                dialogueCamera.SetActive(true);
                dialogueCanvas.SetActive(true);
                Debug.Log("Talk with " + this.gameObject.name);
                if (!zodiacDialogue.zodiacs[zodiacIndex].haveTalkedTo)
                {
                    dialogueText.text = "";
                    string dialogue = zodiacDialogue.zodiacs[zodiacIndex].firstDialogue;
                    zodiacDialogue.zodiacs[zodiacIndex].haveTalkedTo = true;
                    StartCoroutine(TypeDialogue(dialogue, 0.01f));
                }
                else
                {
                    dialogueText.text = "";
                    int dialogueIndex = Random.Range(0, zodiacDialogue.zodiacs[zodiacIndex].randomDialogues.Length);
                    string dialogue = zodiacDialogue.zodiacs[zodiacIndex].randomDialogues[dialogueIndex];
                    StartCoroutine(TypeDialogue(dialogue, 0.01f));
                }
            }
        }
    }

    private IEnumerator TypeDialogue(string dialogue, float speed)
    {
        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isTalking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Exited " + this.gameObject.name + " space.");
            isTalking = false;
            dialogueCamera.SetActive(false);
            dialogueCanvas.SetActive(false);
            dialogueText.text = "";
        }
    }
}
