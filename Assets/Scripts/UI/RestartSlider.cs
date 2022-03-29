using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartSlider : MonoBehaviour
{
    private GameObject sliderObject;
    private Slider restartSlider;
    private bool isRestarted;
    // Start is called before the first frame update
    void Start()
    {
        isRestarted = false;
        sliderObject = this.gameObject;
        restartSlider = this.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRestarted)
        {
            if (Input.GetAxisRaw("Reset") > 0)
            {
                restartSlider.value = Mathf.MoveTowards(restartSlider.value, restartSlider.maxValue, 3f * Time.deltaTime);
            }
            else
            {
                restartSlider.value = Mathf.MoveTowards(restartSlider.value, restartSlider.minValue, 5f * Time.deltaTime);
            }
        }
        if (restartSlider.value == 3)
        {
            isRestarted = true;
            Invoke("ResetRestarted", 2);
            sliderObject.transform.localScale = new Vector3(0, 0, 0);
            Managers.ScenesManager.ResetScene();
            restartSlider.value = 0;
        }
        else if (restartSlider.value == 0)
        {
            sliderObject.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            sliderObject.transform.localScale = new Vector3(0.75f, 0.5f, 0.5f);
        }
    }

    private void ResetRestarted()
    {
        isRestarted = false;
    }

    
}
