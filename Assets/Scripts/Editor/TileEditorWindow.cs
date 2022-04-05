using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class TileEditorWindow : EditorWindow
{
    [MenuItem("Tools/Tile Editor")]
    public static void OpenWindow() {
        GetWindow<TileEditorWindow>();
    }


    public List<GameObject> tilePrefabs;
    public bool isEditorActive = true;
    public GameObject selectedTile;

    SerializedObject so;
    SerializedProperty isEditorActiveProp;
    SerializedProperty selectedTileProp;
    
    private void OnEnable() {
        so = new SerializedObject(this);
        RefreshTiles();
        isEditorActiveProp = so.FindProperty("isEditorActive");
        selectedTileProp = so.FindProperty("selectedTile");

        SceneView.duringSceneGui += DuringSceneGUI;
    }

    private void OnDisable() {
        SceneView.duringSceneGui -= DuringSceneGUI;
    }

    private void DuringSceneGUI(SceneView sceneView) {
        Camera camera = sceneView.camera;

        if (Event.current.type == EventType.MouseMove) {
            sceneView.Repaint();
        }

        if (!isEditorActive) {
            return;
        }

        DrawTileSelect();

        // do the actual tile editing stuff in the main scene
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hit, 999, LayerMask.GetMask("Tile"))) {
            TileParameters tile = hit.collider.GetComponent<TileParameters>();
            if (tile != null) {
                // this is a tile and we should look next to it
                Vector3 adjPos = hit.point + hit.normal * 0.5f;
                adjPos = new Vector3(Mathf.Round(adjPos.x), Mathf.Round(adjPos.y), Mathf.Round(adjPos.z));
                Handles.DrawWireCube(adjPos, Vector3.one);

                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space) {
                    // place the object here
                    if (selectedTile == null) {
                        Debug.LogError("no selected tile");
                        return;
                    }

                    GameObject newTile = (GameObject)PrefabUtility.InstantiatePrefab(selectedTile, tile.parent.parent);
                    Undo.RegisterCreatedObjectUndo(newTile, newTile.name);
                    newTile.transform.position = adjPos;
                }

                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.X) {
                    // delete this tile
                    Undo.DestroyObjectImmediate(tile.parent.gameObject);
                }

                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.C) {
                    // rotate this tile
                    
                    if (tile.rotateMode == TileParameters.RotateMode.AROUNDY) {
                        tile.parent.transform.RotateAround(tile.parent.transform.position, Vector3.up, 90);
                    }
                    else if (tile.rotateMode == TileParameters.RotateMode.SIXDIRECTIONAL) {
                        Vector2 currentFacing = tile.transform.forward;
                        // todo: implement
                    }


                }
            }
        }

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.S) {
            SnapSelection();
        }

    }

    private void DrawTileSelect() {
        Handles.BeginGUI();
        Rect rect = new Rect(8, 8, 48, 48);

        foreach (GameObject tilePrefab in tilePrefabs) {
            if (tilePrefab == null) {
                // can't draw anything for this tile
                GUI.Label(rect, "Invalid prefab");
            }
            else {
                var content = new GUIContent(AssetPreview.GetAssetPreview(tilePrefab), tilePrefab.name);
                GUI.backgroundColor = tilePrefab == selectedTile ? Color.green : Color.white;
                
                if (GUI.Button(rect, content)) {
                    selectedTile = tilePrefab;
                }
            }

            // advance to the next rectangle
            if (rect.x + rect.width + 2 > 225) {
                rect.x = 8;
                rect.y += rect.height + 2;
            }
            else {
                rect.x += rect.width + 2;
            }
        }

        GUI.backgroundColor = Color.white;
        Handles.EndGUI();
    }

    private void OnGUI() {
        so.Update();
        EditorGUILayout.PropertyField(isEditorActiveProp);
        EditorGUILayout.PropertyField(selectedTileProp);

        if (GUILayout.Button("Refresh Tile List")) {
            RefreshTiles();
            selectedTileProp.objectReferenceValue = null;
        }

        if (GUILayout.Button("Snap Selected To Grid")) {
            SnapSelection();
        }

        if (so.ApplyModifiedProperties()) {
            SceneView.RepaintAll();
        }

        
    }


    public void RefreshTiles() {
        tilePrefabs = new List<GameObject>();

        string[] guids = AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/Prefabs/Tiles"});
        if (guids == null || guids.Length == 0) {
            return;
        }
        
        foreach (string guid in guids) {
            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
            tilePrefabs.Add(asset);
        }
    }


    public void SnapSelection() {
        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Snap Selection");
        var undoGroupIndex = Undo.GetCurrentGroup();

        foreach (Object obj in Selection.objects) {
            GameObject go = obj as GameObject;
            TileParameters tile = go?.GetComponent<TileParameters>() ?? go?.GetComponentInChildren<TileParameters>(true);
            if (tile != null) {
                // snap this tile to the grid
                Undo.RecordObject(tile.parent.transform, "");
                
                // we're going to do the rounding manually, because we need a midpoint rounding policy that c# doesn't natively support
                System.Func<float, float> manualRound = (value) => {
                    if (Mathf.Abs(0.5f - (value.Mod(1))) <= 0.01) {
                        // midpoint rounding strategy: floor
                        return Mathf.Floor(value);
                    }
                    return Mathf.Round(value);
                };
                tile.parent.transform.position = new Vector3(
                    manualRound(tile.parent.transform.position.x),
                    manualRound(tile.parent.transform.position.y),
                    manualRound(tile.parent.transform.position.z)
                );
            }
        }

        Undo.CollapseUndoOperations(undoGroupIndex);
    }
}
