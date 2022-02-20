using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StarCollectable : MonoBehaviour
{
    //public StarCounterSO starCounter;
    public StarTrackerSO starTracker;
    public  int sceneIndex;
    public int starNumber;

    private bool collected;

    public float rotateSpeed;
    private float currentRotateSpeed;

    public SpriteRenderer starSR;

    private void Awake() {
        currentRotateSpeed = rotateSpeed;
        if (starTracker.levels[sceneIndex].starsCollected[starNumber] == 1)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (collected)
            return;
        
        if (other.CompareTag("Player")) {
            collected = true;
            //starCounter.currentCount += 1;
            starTracker.levels[sceneIndex].starsCollected[starNumber] = 1;

            DOTween.Sequence()
                .Insert(0, DOTween.To(s => currentRotateSpeed = s, rotateSpeed * 5f, 0f, 1f).SetEase(Ease.InCubic))
                .Insert(0, transform.DOMoveY(transform.position.y + 1f, 0.2f).SetEase(Ease.OutCubic))
                .Insert(0.3f, transform.DOMoveY(transform.position.y - 1f, 0.7f).SetEase(Ease.InOutQuad))
                .Insert(0.5f, starSR.DOFade(0f, 0.5f))
                .Insert(0.5f, transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InExpo));
        }
    }


    private void FixedUpdate() {
        transform.Rotate(new Vector3(0, currentRotateSpeed * Time.deltaTime, 0));
    }
}
