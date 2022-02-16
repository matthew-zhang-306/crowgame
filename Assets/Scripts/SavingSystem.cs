using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavingSystem : MonoBehaviour
{
    private float playerXPos;
    private float playerYPos;
    private float playerZPos;
    public GameObject player;
    private bool isMoved;
    private bool isSaved;
    // Start is called before the first frame update
    void Start()
    {
        isMoved = false;
        //SavePlayerPosition();
        if (Managers.ScenesManager.IsHubSceneLoaded() && !PlayerPrefsX.GetBool("FirstTime", false)){
            SavePlayerPosition();
            PlayerPrefsX.SetBool("FirstTime", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // count as first time playing again
            PlayerPrefsX.SetBool("FirstTime", false);
        }

        if (Managers.ScenesManager.IsHubSceneLoaded() && !isMoved)
        {
            SetPlayerPosition();
            isMoved = true;
        }
    }

    public void SavePlayerPosition()
    {
        PlayerPrefsX.SetVector3("playerPos", player.transform.position);
        Debug.Log("Saved");
        Debug.Log("Player Pos: " + player.transform.position.x + ", " + player.transform.position.y + ", " + player.transform.position.z);
    }

    public void SetPlayerPosition()
    {
        // todo: get camera to center on player immediately
        player.transform.position = PlayerPrefsX.GetVector3("playerPos", player.transform.position);
        Debug.Log("Moved");
        Debug.Log("Player Pos: " + player.transform.position.x + ", " + player.transform.position.y + ", " + player.transform.position.z);
    }
}
