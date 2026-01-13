using UnityEditor;
using UnityEngine;

namespace Tools {
    public class ReplaceWithPrefab : EditorWindow {
        private GameObject _prefab;

        [MenuItem("Tools/Replace With Prefab")]
        private static void ShowWindow() {
            GetWindow<ReplaceWithPrefab>("Replace With Prefab");
        }

        private void OnGUI() {
            GUILayout.Label("Select prefab to replace with", EditorStyles.boldLabel);
            _prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", _prefab, typeof(GameObject), false);

            if (GUILayout.Button("Replace Selected") && _prefab) ReplaceSelected(_prefab);
        }

        private void ReplaceSelected(GameObject prefabObject) {
            foreach (GameObject go in Selection.gameObjects) {
                Vector3 pos = go.transform.position;                            // Save changes
                Quaternion rot = go.transform.rotation;
                Vector3 scale = go.transform.localScale;
                Transform parent = go.transform.parent;

                Undo.DestroyObjectImmediate(go);                                // Delete old object

                GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefabObject, parent);  // Create instance
                newObj.transform.SetPositionAndRotation(pos, rot);
                newObj.transform.localScale = scale;

                Undo.RegisterCreatedObjectUndo(newObj, "Replace With Prefab");
            }
        }
    }
}
