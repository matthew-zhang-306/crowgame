using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatingButtons : MonoBehaviour
{
    private Vector3 initScale;

    private void Start()
    {
        initScale = this.gameObject.transform.localScale;
    }


    public void OnSelected()
    {
        transform.DOScale(new Vector3(initScale.x * 1.1f, initScale.y * 1.1f, initScale.z * 1.2f), 0.25f);
    }

    public void OnDeselected()
    {
        
        transform.DOScale(new Vector3(initScale.x, initScale.y, initScale.z), 0.25f);
    }

    public void OnEnter()
    {
        transform.DOScale(new Vector3(initScale.x, initScale.y, initScale.z), 0.25f);
    }

    public void KillDOTween()
    {
        DOTween.KillAll();
    }
}
