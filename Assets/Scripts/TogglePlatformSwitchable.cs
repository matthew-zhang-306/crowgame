using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TogglePlatformSwitchable : BaseSwitchable
{
    public new Collider collider;
    public MeshRenderer intangibleMesh;
    public MeshRenderer tangibleMesh;
    public GameObject modelContainer;


    private void Awake() {
        tangibleMesh = modelContainer.transform.GetChild(1).GetComponent<MeshRenderer>();
        intangibleMesh = modelContainer.transform.GetChild(0).GetComponent<MeshRenderer>();
        tangibleMesh.enabled = false;
        collider.enabled = false;
    }

    public override void Switch() {
        base.Switch();
        intangibleMesh.gameObject.SetActive(!State);
        tangibleMesh.gameObject.SetActive(State);
        collider.enabled = State;
    }


    public void SetStartingState(TileEditorContext context) {
        
        tangibleMesh = modelContainer.transform.GetChild(1).GetComponent<MeshRenderer>();
        intangibleMesh = modelContainer.transform.GetChild(0).GetComponent<MeshRenderer>();
        
        Debug.Log(!startingState + " " + tangibleMesh.gameObject.name + " " + intangibleMesh.gameObject.name);
        startingState = !startingState;
        intangibleMesh.gameObject.SetActive(!startingState);
        tangibleMesh.gameObject.SetActive(startingState);
        collider.enabled = startingState;

#if UNITY_EDITOR
        PrefabUtility.RecordPrefabInstancePropertyModifications(this);
        PrefabUtility.RecordPrefabInstancePropertyModifications(tangibleMesh.gameObject);
        PrefabUtility.RecordPrefabInstancePropertyModifications(intangibleMesh.gameObject);
#endif
    }


    public void SetModel(TileEditorContext context) {
#if UNITY_EDITOR
        // find models
        var models = new List<GameObject>();

        string[] guids = AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/Prefabs/Model Prefabs/ToggleablePlatforms"});
        if (guids == null || guids.Length == 0) {
            Debug.LogError("Couldn't find any moving platform model prefabs");
            return;
        }
        
        foreach (string guid in guids) {
            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
            models.Add(asset);
        }

        // figure out which one we have and assign the next one
        string meshName = modelContainer.name;
        int modelIndex = models.FindIndex(model => model.name == meshName);
        if (modelIndex < 0) {
            Debug.LogError("Couldn't find moving platform model with name " + meshName);
            return;
        }

        modelIndex = (modelIndex + 1) % models.Count;
        GameObject newModelContainer = (GameObject)PrefabUtility.InstantiatePrefab(models[modelIndex], modelContainer.transform.parent);
        Undo.RegisterCreatedObjectUndo(newModelContainer, newModelContainer.name);

        try {
            Undo.DestroyObjectImmediate(modelContainer);
        } catch {
            Debug.LogWarning("Undo.DestroyObjectImmediate failed so we're deactivating this game object instead");
            modelContainer.SetActive(false);
        }

        modelContainer = newModelContainer;
        SetStartingState(context);
        SetStartingState(context);
#endif
    }
}
