using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviousLevelReset : MonoBehaviour
{
    private void Start() {
        Managers.ProgressManager.SetPreviousLevel(-1);
    }
}
