using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private bool convStart = false;

    public void Update()
    {
        if (convStart == false)
        {
            TriggerDialogue();
            convStart = true;
        }
    }
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);   
    }
}
