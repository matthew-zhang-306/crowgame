using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "HubDialogue", menuName = "SOs/HubDialogue")]
public class HubDialogueSO : ScriptableObject
{
    public Zodiacs[] zodiacs;


    [Serializable]
    public class Zodiacs
    {
        public String zodiacName;
        [TextArea (10, 15)]
        public String[] Dialogue;
    }
}
