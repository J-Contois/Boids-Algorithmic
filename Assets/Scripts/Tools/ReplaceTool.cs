using UnityEditor;
using UnityEngine;

namespace Tools {
    public class ReplaceWithPrefab : EditorWindow {
        private GameObject prefab;

        [MenuItem("Tools/Replace With Prefab")]
        private static void ShowWindow() {
            GetWindow<ReplaceWithPrefab>("Replace With Prefab");
        }

        private void OnGUI() {
            GUILayout.Label("Select prefab to replace with", EditorStyles.boldLabel);
            prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

            if (GUILayout.Button("Replace Selected") && prefab != null) {
                ReplaceSelected(prefab);
            }
        }

        private void ReplaceSelected(GameObject prefabObject) {
            foreach (GameObject go in Selection.gameObjects) {
                // Sauvegarde la transformation
                Vector3 pos = go.transform.position;
                Quaternion rot = go.transform.rotation;
                Vector3 scale = go.transform.localScale;
                Transform parent = go.transform.parent;

                // Supprime l'ancien objet
                Undo.DestroyObjectImmediate(go);

                // Crée une instance du prefab
                GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefabObject, parent);
                newObj.transform.SetPositionAndRotation(pos, rot);
                newObj.transform.localScale = scale;

                Undo.RegisterCreatedObjectUndo(newObj, "Replace With Prefab");
            }
        }
    }
}
