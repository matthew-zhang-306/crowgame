using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// partial code credit to Brackeys on dialogue systems
public class DialogueManager : MonoBehaviour
{
    // First in First out
    private Queue<string> sentences;
    public Text nameText;
    public Text dialogueText;
    private bool pressed;
    private bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        // dialogue moves faster is space is pressed
        if (Input.GetKey(KeyCode.Space))
        {
            pressed = true;
        }
        else
        {
            pressed = false;
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (!isRunning)
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }
            string sentence = sentences.Dequeue();
            //dialogueText.text = sentence;
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isRunning = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            if (pressed == true)
            {
                //Debug.Log("check");
                yield return new WaitForSeconds(0.0001f);
            }
            else
            {
                //Debug.Log("not pressed");
                yield return new WaitForSeconds(0.001f);
            }
        }
        isRunning = false;
    }
    public void EndDialogue()
    {
        //Debug.Log("End of Conversation");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

