using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "EndPuzzle", menuName = "SOs/EndPuzzle")]
public class EndPuzzleSO : ScriptableObject
{
    public Puzzles[] puzzles;

    [Serializable]
    public class Puzzles
    {
        public String zodiacOneName;
        public String zodiacTwoName;
        public Sprite zodiacOneSprite;
        public Sprite zodiacTwoSprite;
        [TextArea(5, 10)]
        public String OneStarDialogue;
        [TextArea(5, 10)]
        public String TwoStarDialogue;
    }
}
