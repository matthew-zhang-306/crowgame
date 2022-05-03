using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class StarCollectable : MonoBehaviour
{
    public LevelListSO levelList;
    public int sceneIndex;

    public int starNumber;

    private bool collected;

    public float rotateSpeed;
    private float currentRotateSpeed;

    public SpriteRenderer starSR;

    private void Start() {
        currentRotateSpeed = rotateSpeed;
        sceneIndex = Managers.ScenesManager.levelNumber;
        Debug.Log("SceneIndex " + sceneIndex);
        if (Managers.ProgressManager.IsStarCollected(sceneIndex, starNumber))
        {
            this.gameObject.SetActive(false);
        }
    }

    //for testing
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    Managers.ProgressManager.SetStarCollected(sceneIndex, 0, true);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha9))
        //{
        //    Managers.ProgressManager.SetStarCollected(sceneIndex, 1, true);
        //}
    }

    private void OnTriggerEnter(Collider other) {

        if (collected)
            return;
        
        if (other.CompareTag("Player")) {
            collected = true;
            
            Managers.AudioManager.PlaySound("star_collection");
            Managers.ProgressManager.SetStarCollected(sceneIndex, starNumber, true);

            Instantiate(Resources.Load("StarCollected"), transform.position, transform.rotation);

            // animation
            DOTween.Sequence()
                .Insert(0, DOTween.To(s => currentRotateSpeed = s, rotateSpeed * 5f, 0f, 1f).SetEase(Ease.InCubic))
                .Insert(0, transform.DOMoveY(transform.position.y + 1f, 0.2f).SetEase(Ease.OutCubic))
                .Insert(0.3f, transform.DOMoveY(transform.position.y - 1f, 0.7f).SetEase(Ease.InOutQuad))
                .Insert(0.5f, starSR.DOFade(0f, 0.5f))
                .Insert(0.5f, transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InExpo)).OnComplete(SetStarInactive);
        }
    }


    private void FixedUpdate() {
        transform.Rotate(new Vector3(0, currentRotateSpeed * Time.deltaTime, 0));
    }

    private void SetStarInactive()
    {
        this.gameObject.SetActive(false);
        //EndCheck();
        
    }

    private void EndCheck()
    {
        if (Managers.ProgressManager.IsAllCollected())
        {
            GameObject.FindGameObjectWithTag("BlackPanel").GetComponent<FadePanel>().fadeOut();
        }
    }


    private void OnDrawGizmos() {
        
    }

    public void SetStarIndex(TileEditorContext context) {
        starNumber = (starNumber + 1) % levelList.starsPerLevel;
    }
}
