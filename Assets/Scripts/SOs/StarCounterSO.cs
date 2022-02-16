using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StarCounter", menuName = "SOs/StarCounter")]
public class StarCounterSO : ScriptableObject
{
    // the number of stars the player currently has when in a particular puzzle.
    // this is always zero when entering a puzzle, increases for each star collected, and gets reset to zero when exiting or resetting.
    public float currentCount;
}
