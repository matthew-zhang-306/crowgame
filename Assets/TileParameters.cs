using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public TileAction[] actions;
}

[System.Serializable]
public struct TileAction {
    public string name;
    public UnityEvent<TileEditorContext> action;
}

[System.Serializable]
public struct TileEditorContext {
    public TileParameters hoveredTile;
    public GameObject selectedTilePrefab;
    public Vector3 adjacentPosition;
    public TileParameters selectedTile;
}
