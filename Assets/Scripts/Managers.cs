using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// all singletons will be stored here
public class Managers : MonoBehaviour
{
    Managers instance;
    public static AudioManager AudioManager;
    public static ScenesManager ScenesManager;
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
            DontDestroyOnLoad(gameObject);
        
            AudioManager = GetComponentInChildren<AudioManager>();
            AudioManager?.Init();
            ScenesManager = GetComponentInChildren<ScenesManager>();
            ScenesManager?.Init();

            // add more manager initializations below
        }
    }
}
