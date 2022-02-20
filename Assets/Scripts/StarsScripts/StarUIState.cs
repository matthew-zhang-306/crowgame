using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarUIState : MonoBehaviour
{
    public StarTrackerSO starTracker;
    public int sceneIndex;
    public int starNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStarUI();
    }

    public void UpdateStarUI()
    {
        //if a certain star in a certain scene is collected then activate the collected star image
        if (starTracker.levels[sceneIndex].starsCollected[starNumber] == 1)
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
