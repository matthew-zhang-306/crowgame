using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorManager : BaseSwitchable
{
    public Vector3 openOffset;
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool waiting = false;

    public new Collider collider;
    public Transform visualTransform;
    public int timer = 0; //0 is default, if 0 not timed


    private void Awake() {
        closedPosition = transform.position;
        openPosition = transform.position + openOffset;

        if (startingState) {
            // we set this here to have it start open without animating
            // later, in Start(), Switch() will be called for real
            visualTransform.position = openPosition;
        }
    }


    IEnumerator timedDoor()
    {
        waiting = true;
        yield return new WaitForSeconds(timer);
        waiting = false;

        //door goes back down
        Switch(); //hopefully doesnt cause a horrible loop
    }

    public override void Switch() {
        if(!waiting)
        {
            base.Switch();

            collider.transform.position = State ? openPosition : closedPosition;
            visualTransform.DOKill();
            visualTransform.DOMove(State ? openPosition : closedPosition, 1.0f)
                .SetEase(Ease.InOutCubic);

            Managers.AudioManager.PlaySound("door_opening");

            if (timer > 0 && collider.transform.position == openPosition)
            {
                StartCoroutine(timedDoor());
            }
        }
    }
        

    private void OnDrawGizmos() {
        if (collider == null)
            return;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(collider.bounds.center + openOffset, collider.bounds.size);
    }
}
