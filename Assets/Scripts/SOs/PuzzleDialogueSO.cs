using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PuzzleDialogue", menuName = "SOs/PuzzleDialogue")]
public class PuzzleDialogueSO : ScriptableObject
{
    public Zodiacs[] zodiacs;

    [Serializable]
    public class Zodiacs
    {
        public String zodiacName;
        public Boolean firstStarCollected;
        public Boolean secondStarCollected;
        [TextArea]
        public String[] IntroDialogue;
        [TextArea]
        public String[] OneStarDialogue;
        [TextArea]
        public String[] TwoStarDialogue;
    }
}
