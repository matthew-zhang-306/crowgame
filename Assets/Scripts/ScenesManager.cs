using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    [HideInInspector] public int levelNumber;
    public LevelListSO levelList;
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
        levelNumber = levelList.levels.ToList().FindIndex(0, levelList.levels.Length, l => l.sceneName == scene.name);

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


    public bool IsHubSceneLoaded() {
        return SceneManager.GetActiveScene().name == levelList.hub.sceneName;
    }

    public bool IsPuzzleSceneLoaded() {
        return SceneManager.GetActiveScene().name.StartsWith("P_");
    }

}
