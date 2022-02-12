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
        if (PlayerPrefs.GetInt("FirstTime") == 0){
            SavePlayerPosition();
            PlayerPrefs.SetInt("FirstTime", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // count as first time playing again
            PlayerPrefs.SetInt("FirstTime", 0);
        }

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Hub-World") && !isMoved)
        {
            SetPlayerPosition();
            isMoved = true;
        }
    }

    public void SavePlayerPosition()
    {
        PlayerPrefs.SetFloat("playerXPos", player.transform.position.x);
        PlayerPrefs.SetFloat("playerYPos", player.transform.position.y);
        PlayerPrefs.SetFloat("playerZPos", player.transform.position.z);
        Debug.Log("Saved");
        Debug.Log("Player Pos: " + player.transform.position.x + ", " + player.transform.position.y + ", " + player.transform.position.z);
    }

    public void SetPlayerPosition()
    {
        player.transform.position = new Vector3(PlayerPrefs.GetFloat("playerXPos"), PlayerPrefs.GetFloat("playerYPos"), PlayerPrefs.GetFloat("playerZPos"));
        Debug.Log("Moved");
        Debug.Log("Player Pos: " + player.transform.position.x + ", " + player.transform.position.y + ", " + player.transform.position.z);
    }
}
