using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSwitch : MonoBehaviour
{
    public List<BaseSwitchable> targets;
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


    // takes a tile
    // if the tile corresponds to a switchable that is not currently a target, it adds this as a target
    // if the tile corresponds to a switchable that is currently a target, it removes this from the targets
    public void UpdateTileTarget(TileEditorContext context) {
        // look for a switchable attached to this tile
        BaseSwitchable switchable = null;
        if (context.hoveredTile != null)
            switchable = context.hoveredTile.parent.GetComponent<BaseSwitchable>()
                ?? context.hoveredTile.parent.GetComponentInChildren<BaseSwitchable>();
        
        Debug.Log(switchable);

        if (switchable != null) {
            if (targets.Contains(switchable)) {
                // this switchable is currently a target, so change that
                targets.Remove(switchable);
            }
            else {
                // this switchable is not a target, so add it
                targets.Add(switchable);
            }
        }
    }
}
