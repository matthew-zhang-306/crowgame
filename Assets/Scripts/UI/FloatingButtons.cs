using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatingButtons : MonoBehaviour
{
    public void OnSelected()
    {
        transform.DOScale(new Vector3(1.1f, 1.1f, 1.2f), 0.5f);
    }

    public void OnDeselected()
    {
        transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
    }
}
