using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicsControl : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Toggle vSyncToggle;
    public Resolutions[] resolutions;
    public TextMeshProUGUI resolutionText;
    public int selectedRes;

    // Start is called before the first frame update
    void Start()
    {
        // start settings with the current screen resolutions and settings
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                selectedRes = i;
                resolutionText.text = resolutions[selectedRes].width + "X" + resolutions[selectedRes].height;
            }
        }
        fullscreenToggle.isOn = Screen.fullScreen;
        if (QualitySettings.vSyncCount == 0)
        {
            vSyncToggle.isOn = false;
        }
        else
        {
            vSyncToggle.isOn = true;
        }
    }

    // change to fullscreen when toggle is on
    public void ApplyFullscreen()
    {
        if (fullscreenToggle.isOn)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }
    }

    // go to previous resolution
    public void PrevResolution()
    {
        if (selectedRes > 0)
        {
            selectedRes--;
        }
        else
        {
            selectedRes = resolutions.Length - 1;
        }
        resolutionText.text = resolutions[selectedRes].width + "X" + resolutions[selectedRes].height;
    }

    // go to next resolution
    public void NextResolution()
    {
        if (selectedRes < resolutions.Length - 1)
        {
            selectedRes++;
        }
        else
        {
            selectedRes = 0;
        }
        resolutionText.text = resolutions[selectedRes].width + "X" + resolutions[selectedRes].height;
    }

    // set the screen resolutions based on width and height of item in resolutions array
    public void SetResolution()
    {
        Screen.SetResolution(resolutions[selectedRes].width, resolutions[selectedRes].height, fullscreenToggle.isOn);
    }


    // vertical sync count controled be toggle
    public void ApplyVSync()
    {
        if (vSyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    // apply the changes
    public void ApplyChanges()
    {
        ApplyVSync();
        ApplyFullscreen();
        SetResolution();
    }
}
