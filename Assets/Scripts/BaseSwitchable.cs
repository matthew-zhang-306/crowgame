using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSwitchable : MonoBehaviour
{
    public bool startingState;
    public bool State { get; private set; }

    protected virtual void Start() {
        if (startingState) {
            Switch();
        }
    }

    public virtual void Switch() {
        State = !State;
    }
}
