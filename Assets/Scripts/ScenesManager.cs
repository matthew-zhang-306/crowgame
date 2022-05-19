using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public enum SceneType {
        MENU,
        HUB,
        PUZZLE
    }
    [HideInInspector] public SceneType sceneType;
    [HideInInspector] public int levelNumber;
    [HideInInspector] public string nameOfScene;
    public LevelListSO levelList;
    public SceneTransition sceneTransition;

    public SceneDef currentSceneDef { get {
        if (sceneType == SceneType.HUB)
            return levelList.hubs[levelNumber];
        if (sceneType == SceneType.PUZZLE)
            return levelList.levels[levelNumber];
        return null;
    }}

    private string _destinationExit;
    public string destinationExit {
        get { return _destinationExit; }
        set { Debug.Log("setting " + _destinationExit); _destinationExit = value; }
    }

    public bool HasChangedScenes { get; private set; } // true if a scene change has taken place
    public bool IsTransitioning { get; private set; } // true if we are in the process of changing scenes
    bool didTransitionOut;

    private bool resetInput;
    private bool oldResetInput;

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
        sceneType = FetchSceneData(scene);

        if (sceneType == SceneType.PUZZLE || sceneType == SceneType.HUB) {
            Managers.ProgressManager.SetSavePosition(scene.name, destinationExit);
        }

        if (sceneType == SceneType.PUZZLE)
        {
            Managers.ProgressManager.SetLevelVisited(levelNumber, true);
        }

        if (sceneType == SceneType.MENU)
        {
            Managers.PauseMenu.enabled = false;
            gameObject.GetComponentInChildren<RestartSlider>().gameObject.transform.localScale = new Vector3(0, 0, 0);
            gameObject.GetComponentInChildren<RestartSlider>().enabled = false;
        }
        else {
            Managers.PauseMenu.enabled = true;
            gameObject.GetComponentInChildren<RestartSlider>().enabled = true;
        }
        
        if (IsEndSceneLoaded())
        {
            GameObject.FindGameObjectWithTag("BlackPanel").GetComponent<FadePanel>().fadeIn();
        }

        IsTransitioning = false;
        if (didTransitionOut) {
            // remove the scene transition
            sceneTransition.TransitionIn(0.5f);
            didTransitionOut = false;
        }
    }


    // sets nameOfScene and levelNumber appropriately, then returns the correct value for sceneType
    private SceneType FetchSceneData(Scene scene) {
        nameOfScene = scene.name;

        levelNumber = levelList.hubs.ToList().FindIndex(0, levelList.hubs.Length, h => h.sceneName == scene.name);
        if (levelNumber >= 0) {
            if (levelNumber > 0)
            {
                PlayerPrefsX.SetBool("hasPlayed", true);
            }
            // we are in a hub level
            return SceneType.HUB;
        }

        levelNumber = levelList.levels.ToList().FindIndex(0, levelList.levels.Length, l => l.sceneName == scene.name);
        if (levelNumber >= 0) {
            // we are in a puzzle level
            return SceneType.PUZZLE;
        }

#if UNITY_EDITOR
        // if we're in the editor, when testing a brand new scene we'll use the scene name to infer its type
        if (nameOfScene.ToLower().Contains("hub")) {
            return SceneType.HUB;
        }
        else if (nameOfScene.ToLower().Contains("p_")) {
            return SceneType.PUZZLE;
        }
#endif
        
        return SceneType.MENU;
    }


    private void Update() {
        oldResetInput = resetInput;
        resetInput = Input.GetAxisRaw("Reset") > 0;
        
#if UNITY_EDITOR
        // debug reset
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R)) {
            if (IsHubSceneLoaded()) {
                // resetting in the hub world? we probably want to warp the player back to the very beginning
                Managers.ProgressManager.SetSavePosition(nameOfScene);
            }
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
        HasChangedScenes = true;
        if (transitionTime > 0f) {

            //play scene transition sound
            Managers.AudioManager.PlaySound("scene_transition");

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
        return sceneType == SceneType.HUB;
    }

    // returns true if some level scene is currently loaded
    public bool IsPuzzleSceneLoaded() {
        return sceneType == SceneType.PUZZLE;
    }

    // returns true if endcutscene is currently loaded
    public bool IsEndSceneLoaded()
    {
        return SceneManager.GetActiveScene().name == "EndCutscene";
    }
}
