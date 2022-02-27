using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pinwheel : MonoBehaviour
{
    public DoorManager door;

    public SpriteRenderer spriteRenderer;
    public Color offColor;
    public Color onColor;

    public float maxRotateSpeed;
    private float currentRotateSpeed;

    private int windMask;
    private bool oldIsOn;

    private void Awake() {
        windMask = LayerMask.GetMask("Wall", "Box", "Tornado");
        spriteRenderer.color = offColor;
    }

    private void FixedUpdate() {
        CheckState();

        // do rotation
        spriteRenderer.transform.localRotation = Quaternion.Euler(
            0,0, spriteRenderer.transform.localRotation.eulerAngles.z + currentRotateSpeed * Time.deltaTime
        );
    }


    private void CheckState() {
        bool isOn = false;

        // check if there is a tornado blowing on us from below
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 4f, windMask)) {
            Tornado tornado = hit.collider.GetComponent<Tornado>();
            if (tornado != null && tornado.Top.y > transform.position.y) {
                isOn = true;
            }
        }

        if (isOn != oldIsOn) {
            // this is a change of state
            door.Switch();

            spriteRenderer.DOKill();
            spriteRenderer.DOColor(isOn ? onColor : offColor, 0.5f);
            DOTween.To(s => currentRotateSpeed = s, currentRotateSpeed, isOn ? maxRotateSpeed : 0, 0.5f)
                .SetLink(spriteRenderer.gameObject).SetTarget(spriteRenderer);
        }

        oldIsOn = isOn;
    }
}
