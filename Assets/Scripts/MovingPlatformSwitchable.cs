using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformSwitchable : BaseSwitchable
{
    private MovingPlatform movingPlatform;

    private void Awake() {
        movingPlatform = GetComponent<MovingPlatform>();
        if (movingPlatform.isAutomatic) {
            Debug.LogWarning("MovingPlatformSwitchable attached to an automatic moving platform. This won't work as intended so you should turn off the 'isAutomatic' variable on the moving platform");
        }
    }

    public override void Switch() {
        base.Switch();
        movingPlatform.Move(State);
    }
}
