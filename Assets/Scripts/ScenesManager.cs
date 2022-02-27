using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    [HideInInspector] public int levelNumber;
    [HideInInspector] public string nameOfScene;
    public LevelListSO levelList;
    public SceneTransition sceneTransition;
    public bool IsTransitioning { get; private set; }
    bool didTransitionOut;

    // called whenever a scene transition takes place
    public static System.Action OnTransition;


    public void Init() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // called whenever a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        // search the levelList for the level number that corresponds to this scene
        levelNumber = levelList.levels.ToList().FindIndex(0, levelList.levels.Length, l => l.sceneName == scene.name);
        if (levelNumber >= 0)
        {
            nameOfScene = levelList.levels[levelNumber].sceneName;
            PlayerPrefs.SetInt(nameOfScene, 1);
        }

        IsTransitioning = false;
        if (didTransitionOut) {
            // remove the scene transition
            sceneTransition.TransitionIn(0.5f);
            didTransitionOut = false;
        }
    }


    private void Update() {
        // in-game reset
        if (Input.GetKeyDown(KeyCode.R)) {
            ResetScene();
        }
        
#if UNITY_EDITOR
        // debug reset
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        // debug scene advance
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Equals)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Minus)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
#endif
    }


    public void ChangeScene(string sceneName, float transitionTime = 0.5f) {
        if (IsTransitioning) {
            return;
        }

        IsTransitioning = true;
        if (transitionTime > 0f) {
            didTransitionOut = true;
            OnTransition?.Invoke();

            // load scene asynchronously after the transition is over
            sceneTransition.TransitionOut(transitionTime, () => {
                this.Invoke(() => SceneManager.LoadSceneAsync(sceneName), 0.2f);
            });
        }
        else {
            // no transition; load next scene immediately
            SceneManager.LoadScene(sceneName);
        }
    }


    public void ResetScene(float transitionTime = 0.5f) {
        ChangeScene(SceneManager.GetActiveScene().name, transitionTime);
    }


    // returns true if the hub is currently loaded
    public bool IsHubSceneLoaded() {
        return SceneManager.GetActiveScene().name == levelList.hub.sceneName;
    }

    // returns true if some level scene is currently loaded
    public bool IsPuzzleSceneLoaded() {
        return SceneManager.GetActiveScene().name.StartsWith("P_");
    }
}
