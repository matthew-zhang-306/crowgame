using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZodiacDialogue", menuName = "SOs/ZodiacDialogue")]
public class ZodiacDialogueSO : ScriptableObject
{
    public Zodiacs[] zodiacs;


    [Serializable]
    public class Zodiacs
    {
        public String zodiacName;
        public Boolean haveTalkedTo;
        [TextArea]
        public String firstDialogue;
        [TextArea]
        public String[] randomDialogues;
        [TextArea]
        public String xboxText;

    }
}
