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
        public String zodiacOneName;
        public String zodiacTwoName;
        public Sprite zodiacOneSprite;
        public Sprite zodiacTwoSprite;
        [TextArea (5, 10)]
        public String IntroDialogueOne;
        [TextArea (5, 10)]
        public String IntroDialogueTwo;
        [TextArea (5, 10)]
        public String OneStarDialogue;
        [TextArea (5, 10)]
        public String TwoStarDialogue;
    }
}
