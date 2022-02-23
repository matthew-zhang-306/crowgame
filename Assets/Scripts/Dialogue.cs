using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    //name of NPC
    public string name;

    [TextArea(10, 15)]
    //strings that will load into queue in dialogue manager
    public string[] sentences;
}
