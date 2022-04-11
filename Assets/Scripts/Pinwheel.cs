using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pinwheel : BaseSwitch
{
    public MeshRenderer pinwheelHead;
    public Material[] offMaterials;
    public Material[] onMaterials;

    public float maxRotateSpeed;
    private float currentRotateSpeed;

    private int windMask;
    private bool oldIsOn;

    private void Awake() {
        windMask = LayerMask.GetMask("Wall", "Box", "Tornado");
    }

    private void FixedUpdate() {
        CheckState();

        // do rotation
        pinwheelHead.transform.Rotate(Vector3.right, currentRotateSpeed * Time.deltaTime);
    }


    private void CheckState() {
        bool isOn = false;

        // check if there is a tornado blowing on us from below
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 4f, windMask)) {
            Tornado tornado = hit.collider.GetComponent<Tornado>();
            if (tornado != null && tornado.Top.y > transform.position.y - 0.1f) {
                isOn = true;
            }
        }

        if (isOn != oldIsOn) {
            // this is a change of state
            Switch();
            if (isOn)
            {
                //switching to on, play sound
                Managers.AudioManager.PlaySound("pinwheel", 5f);
            }
           
            pinwheelHead.DOKill();
            pinwheelHead.materials = isOn ? onMaterials : offMaterials;
            DOTween.To(s => currentRotateSpeed = s, currentRotateSpeed, isOn ? maxRotateSpeed : 0, 0.5f)
                .SetLink(pinwheelHead.gameObject).SetTarget(pinwheelHead);
        }

        oldIsOn = isOn;
    }
}
