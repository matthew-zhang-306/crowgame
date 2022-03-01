using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePlatformSwitchable : BaseSwitchable
{
    public new Collider collider;
    public MeshRenderer tangibleMesh;

    private void Awake() {
        // switch the platform off. if the starting state is on, it will get changed back in Start()
        Switch();
    }

    public override void Switch() {
        base.Switch();

        tangibleMesh.enabled = State;
        collider.enabled = State;
    }
}
