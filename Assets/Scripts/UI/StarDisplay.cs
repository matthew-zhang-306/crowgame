using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StarDisplay : MonoBehaviour
{
    public LevelListSO levelList;

    private void Update() {
        // go through the stars and update all of them
        for (int i = 0; i < transform.childCount; i++) {
            Transform starT = transform.GetChild(i);

            //if a certain star in a certain scene is collected then activate the collected star image
            starT.GetChild(1).gameObject.SetActive(
                Managers.ProgressManager.IsStarCollected(Managers.ScenesManager.levelNumber, i)
            );
        }
    }
}
