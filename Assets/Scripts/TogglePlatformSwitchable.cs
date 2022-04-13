using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePlatformSwitchable : BaseSwitchable
{
    public new Collider collider;
    public MeshRenderer tangibleMesh;

    private void Awake() {
        tangibleMesh.enabled = false;
        collider.enabled = false;
    }

    public override void Switch() {
        base.Switch();

        tangibleMesh.enabled = State;
        collider.enabled = State;
    }


    public void SetStartingState(TileEditorContext context) {
        startingState = !startingState;

        tangibleMesh.enabled = startingState;
        collider.enabled = startingState;
    }
}
