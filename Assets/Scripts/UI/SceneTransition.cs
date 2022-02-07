using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneTransition : MonoBehaviour
{
    public Image leftDoor;
    public Image rightDoor;
    public float slidePosition;
    public float basePosition;

    
    // opens the doors
    public Tween TransitionIn(float time, System.Action onComplete = null) {
        this.DOKill();
        return DOTween.Sequence()
            .Insert(0f, leftDoor.rectTransform.DOAnchorPosX(-basePosition, time).SetEase(Ease.OutCubic))
            .Insert(0f, rightDoor.rectTransform.DOAnchorPosX(basePosition, time).SetEase(Ease.OutCubic))
            .OnComplete(() => onComplete?.Invoke());
    }

    // shuts the doors
    public Tween TransitionOut(float time, System.Action onComplete = null) {
        this.DOKill();
        return DOTween.Sequence()
            .Insert(0f, leftDoor.rectTransform.DOAnchorPosX(-slidePosition, time).SetEase(Ease.InQuad))
            .Insert(0f, rightDoor.rectTransform.DOAnchorPosX(slidePosition, time).SetEase(Ease.InQuad))
            .OnComplete(() => onComplete?.Invoke());
    }

}
