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
    public GameObject selectedTile;

    SerializedObject so;
    SerializedProperty selectedTileProp;
    
    private void OnEnable() {
        so = new SerializedObject(this);
        RefreshTiles();
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

        Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hit, LayerMask.GetMask("Tile"))) {
            TileParameters tile = hit.collider.GetComponent<TileParameters>();
            if (tile != null) {
                // this is a tile and we should look next to it
                Vector3 adjPos = hit.point + hit.normal * 0.5f;
                adjPos = new Vector3(Mathf.Round(adjPos.x), Mathf.Round(adjPos.y), Mathf.Round(adjPos.z));
                Handles.DrawWireCube(adjPos, Vector3.one);

                Debug.Log("hitting");

                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space) {
                    // place the object here
                    if (selectedTile == null) {
                        Debug.Log("no selected tile");
                        return;
                    }

                    GameObject newTile = (GameObject)PrefabUtility.InstantiatePrefab(selectedTile, hit.collider.GetComponentInParent<TileEditor>().transform);
                    Undo.RegisterCreatedObjectUndo(newTile, newTile.name);
                    newTile.transform.position = adjPos;
                }

                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.X) {
                    // delete this tile
                    Undo.DestroyObjectImmediate(tile.parent.gameObject);
                }
            }
        }
    }

    private void OnGUI() {
        so.Update();
        EditorGUILayout.PropertyField(selectedTileProp);

        if (GUILayout.Button("Refresh Tile List")) {
            RefreshTiles();
            selectedTileProp.objectReferenceValue = null;
        }

        foreach (GameObject tilePrefab in tilePrefabs) {
            if (tilePrefab == null) {
                GUILayout.Label("Invalid prefab");
            }
            if (GUILayout.Button(tilePrefab.name)) {
                selectedTileProp.objectReferenceValue = tilePrefab;
            }
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
}
