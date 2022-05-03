using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

// partial code credit to Brackeys on dialogue systems
public class DialogueManager : MonoBehaviour
{
    // First in First out
    private Queue<string> sentences;
    //public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nextText;
    private bool oldInput;
    private bool input;
    private bool isRunning;
    public List<GameObject> panelImages = new List<GameObject>();
    private int currentPanel = -1;
    public GameObject firstButton;
    public string nextSceneName;
    public float textSpeed;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        EventSystem.current.SetSelectedGameObject(firstButton);

        #if UNITY_WSA
            //xbox version of text
            nextText.text = "Press A";
        #endif
    }

    private void Update()
    {
        oldInput = input;
        input = Input.GetAxisRaw("Action1") > 0 || Input.GetAxisRaw("Submit") > 0;

        if (input && !oldInput)
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //nameText.text = dialogue.name;
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
            // consume the input
            oldInput = input;

            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            Managers.AudioManager.PlaySound("ui_button_press");
            string sentence = sentences.Dequeue();
            //dialogueText.text = sentence;
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
            DisplayNextPanel();
        }
    }

    public void DisplayNextPanel()
    {
        currentPanel++;
        if (currentPanel >= panelImages.Count)
        {
            currentPanel = panelImages.Count - 1;
        }
        panelImages[currentPanel].SetActive(true);
        for (int i = 0; i < panelImages.Count; i++)
        {
            if (i != currentPanel)
            {
                panelImages[i].SetActive(false);
            }
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isRunning = true;
        dialogueText.text = sentence;
        dialogueText.maxVisibleCharacters = 0;
        
        for (float t = 0; dialogueText.maxVisibleCharacters < sentence.Length; t += Time.deltaTime) {
            dialogueText.maxVisibleCharacters = (int)(t * textSpeed);

            if (input && !oldInput) {
                // consume input
                oldInput = input;
                dialogueText.maxVisibleCharacters = sentence.Length;
            }

            yield return null;
        }
        
        isRunning = false;
    }
    
    public void EndDialogue()
    {
        //Debug.Log("End of Conversation");
        if (SceneManager.GetActiveScene().name == "EndCutscene")
        {
            GameObject menuBtn = this.transform.GetChild(0).gameObject;
            GameObject quitBtn = this.transform.GetChild(1).gameObject;
            menuBtn.SetActive(true);
            quitBtn.SetActive(true);
            EventSystem.current.SetSelectedGameObject(menuBtn);
        }
        else
        {
            Managers.ScenesManager.ChangeScene(nextSceneName);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnMenu()
    {
        Managers.ScenesManager.ChangeScene("Menu");
    }
}

