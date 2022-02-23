using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorManager : MonoBehaviour
{
    public bool startsOpen = false;
    private bool doorOpen = false;

    public Vector3 openOffset;
    private Vector3 closedPosition;
    private Vector3 openPosition;

    public new Collider collider;
    public Transform visualTransform;

    private void Awake() {
        closedPosition = transform.position;
        openPosition = transform.position + openOffset;

        if (startsOpen) {
            // we set this here to have it start open without animating
            visualTransform.position = openPosition;

            Switch();
        }
    }


    public void Switch() {
        doorOpen = !doorOpen;

        collider.transform.position = doorOpen ? openPosition : closedPosition;
        visualTransform.DOKill();
        visualTransform.DOMove(doorOpen ? openPosition : closedPosition, 1.0f)
            .SetEase(Ease.InOutCubic);
    }

    private void OnDrawGizmos() {
        if (collider == null)
            return;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(collider.bounds.center + openOffset, collider.bounds.size);
    }
}
