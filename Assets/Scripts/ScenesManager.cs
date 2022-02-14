using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public SceneTransition sceneTransition;
    public bool IsTransitioning { get; private set; }
    bool didTransitionOut;

    public void Init() {
        // do stuff here
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        // do stuff here
        IsTransitioning = false;
        if (didTransitionOut) {
            sceneTransition.TransitionIn(0.5f);
            didTransitionOut = false;
        }
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


    public void ChangeScene(string sceneName, float transitionTime = 0.5f) {
        if (IsTransitioning) {
            return;
        }

        IsTransitioning = true;
        if (transitionTime > 0f) {
            didTransitionOut = true;
            sceneTransition.TransitionOut(transitionTime, () => {
                this.Invoke(() => SceneManager.LoadSceneAsync(sceneName), 0.2f);
            });
        }
        else {
            SceneManager.LoadScene(sceneName);
        }
    }


    // use this for debugging only
    public void ReloadScene() {
        if (IsTransitioning) {
            return;
        }

        IsTransitioning = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public string GetSceneName() {
        return SceneManager.GetActiveScene().name;
    }

}
