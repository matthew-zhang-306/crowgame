using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadePanel : MonoBehaviour
{
    public void fadeIn()
    {
        this.gameObject.GetComponent<Animator>().Play("FadeIn");
    }

    public void fadeOut()
    {
        this.gameObject.GetComponent<Animator>().Play("FadeOut");
    }

    public void nextScene()
    {
        SceneManager.LoadScene("EndCutscene");
    }
}
