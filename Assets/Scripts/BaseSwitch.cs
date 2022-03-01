using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSwitch : MonoBehaviour
{
    public BaseSwitchable[] targets;
    public Color gizmosColor;

    public virtual void Switch() {
        foreach (BaseSwitchable target in targets) {
            if (target != null) {
                target.Switch();
            }
            else {
                Debug.LogError("Switch " + this + " has a null target");
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = gizmosColor;

        foreach (BaseSwitchable target in targets) {
            if (target != null) {
                Gizmos.DrawLine(transform.position, target.transform.position);
            }
        }
    }
}
