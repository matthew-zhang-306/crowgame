using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorManager : BaseSwitchable
{
    public Vector3 openOffset;
    private Vector3 closedPosition;
    private Vector3 openPosition;

    public new Collider collider;
    public Transform visualTransform;

    private void Awake() {
        closedPosition = transform.position;
        openPosition = transform.position + openOffset;

        if (startingState) {
            // we set this here to have it start open without animating
            // later, in Start(), Switch() will be called for real
            visualTransform.position = openPosition;
        }
    }


    public override void Switch() {
        base.Switch();

        collider.transform.position = State ? openPosition : closedPosition;
        visualTransform.DOKill();
        visualTransform.DOMove(State ? openPosition : closedPosition, 1.0f)
            .SetEase(Ease.InOutCubic);
    }

    private void OnDrawGizmos() {
        if (collider == null)
            return;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(collider.bounds.center + openOffset, collider.bounds.size);
    }
}
