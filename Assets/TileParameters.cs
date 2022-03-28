using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileParameters : MonoBehaviour
{
    public enum RotateMode {
        NONE,
        AROUNDY,
        SIXDIRECTIONAL
    }

    public Transform parent;
    public RotateMode rotateMode;
    public Vector3Int size = Vector3Int.one;
}
