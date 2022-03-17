using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlayerActionIndicator : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI sideText;
    public RectTransform sidePanel;
    private CanvasGroup canvasGroup;

    Tween fadeOutTween;
    Vector2 sideSizeDelta;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        sideSizeDelta = sidePanel.sizeDelta;

        canvasGroup.alpha = 0;
        sidePanel.sizeDelta = sideSizeDelta.WithX(0);
    }

    public void Show(string button, string action) {
        // this is the function that displays the panel
        System.Action DoShow = () => {
            buttonText.text = button;
            sideText.text = action;

            canvasGroup.alpha = 0;
            sidePanel.sizeDelta = sideSizeDelta.WithX(0);

            DOTween.Sequence()
                .Insert(0, canvasGroup.DOFade(1f, 0.5f))
                .Insert(0.25f, sidePanel.DOSizeDelta(sideSizeDelta, 0.5f).SetEase(Ease.OutCubic))
                .SetTarget(this).SetLink(gameObject);
        };
        
        // decide when to run the function based on whether the panel is currently fading out
        if (fadeOutTween?.IsPlaying() ?? false) {
            // run the animation after the fade out is completed
            fadeOutTween.OnComplete(() => DoShow());
        }
        else {
            // we're not fading out so we're free to show asap
            DoShow();
        }
    }

    public void Hide() {
        if (fadeOutTween?.IsPlaying() ?? false) {
            // we're already fading out so we don't need to do much, but
            // if Show() was called after the fade out started, we need to remove the oncomplete callback
            fadeOutTween.OnComplete(() => {});
        }
        else {
            // we're not currently fading out, so we'll start doing that
            // but we could be fading in, so we stop that animation
            this.DOKill();
            fadeOutTween = canvasGroup.DOFade(0f, 0.3f)
                .OnKill(() => fadeOutTween = null); // this makes it so that we don't get dotween warnings for referencing killed tweens
        }
    }
}
