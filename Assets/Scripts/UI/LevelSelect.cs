using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    private Button button;
    public string nameOfScene;
    // Start is called before the first frame update
    void Start()
    {
        // set button interactable only if the scene was visited before
        button = this.gameObject.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt(nameOfScene, 0) == 1)
        {
            button.interactable = true;
            //Debug.Log("PlayerPrefs: " + nameOfScene + PlayerPrefs.GetInt(nameOfScene, 0));
        }
        else
        {
            button.interactable = false;
        }
    }
}
