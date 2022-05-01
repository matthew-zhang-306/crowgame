using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// all singletons will be stored here
public class Managers : MonoBehaviour
{
    static Managers instance;
    public static AudioManager AudioManager;
    public static ScenesManager ScenesManager;
    public static ProgressManager ProgressManager;
    public static PauseMenu PauseMenu;
    // etc

    private void Awake() {
        if (instance != null && instance != this) {
            // enforce singleton
            Destroy(gameObject);
            return;
        }

        if (instance == null) {
            // initialize singleton
            instance = this;
            Debug.Log(gameObject);
            DontDestroyOnLoad(gameObject);
        
            AudioManager = GetComponentInChildren<AudioManager>();
            ScenesManager = GetComponentInChildren<ScenesManager>();
            ProgressManager = GetComponentInChildren<ProgressManager>();
            PauseMenu = GetComponentInChildren<PauseMenu>();

            AudioManager?.Init();
            ScenesManager?.Init();
            ProgressManager?.Init();
        
            // add more initializations as needed
        }
    }
}
