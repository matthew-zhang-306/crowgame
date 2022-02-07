using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WarpUI : MonoBehaviour
{
    public TextMeshProUGUI text;


    private void Awake() {
        text.text = "";
    }

    private void OnEnable() {
        WarpAltar.OnAltarEnter += ShowUI;
        WarpAltar.OnAltarExit  += HideUI;
        WarpAltar.OnAltarWarp  += HideUI;
    }
    private void OnDisable() {
        WarpAltar.OnAltarEnter -= ShowUI;
        WarpAltar.OnAltarExit  -= HideUI;
        WarpAltar.OnAltarWarp  -= HideUI;
    }


    private void ShowUI(WarpAltar altar) {
        text.text = "Warp to " + altar.destinationName;
    }

    private void HideUI(WarpAltar altar) {
        text.text = "";
    }


}
