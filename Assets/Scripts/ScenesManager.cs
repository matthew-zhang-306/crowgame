using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public bool IsTransitioning { get; private set; }

    public void Init() {
        // do stuff here
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        // do stuff here
    }


    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Equals)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (Input.GetKeyDown(KeyCode.Minus)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }


    public void ChangeScene(string sceneName, float transitionTime) {
        if (IsTransitioning) {
            return;
        }

        IsTransitioning = true;

        // todo: implement transition
        // todo: make scene load asynchronous
        SceneManager.LoadScene(sceneName);
    }


    // use this for debugging only
    public void ReloadScene() {
        if (IsTransitioning) {
            return;
        }

        IsTransitioning = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
